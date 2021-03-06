﻿using DevExpress.Mvvm;
using DryIoc;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Properties;
using ISTraining_Part.Providers;
using ISTraining_Part.ViewModels.Classes;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using Prism.Regions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ISTraining_Part.ViewModels
{
    /// <summary>
    /// Login view model.
    /// </summary>
    class LoginViewModel : NavigationViewModel
    {
        /// <summary>
        /// Пользователь для авторизации.
        /// </summary>
        public LoginUser LoginUser { get; }

        /// <summary>
        /// Менеджер регионов.
        /// </summary>
        readonly IRegionManager regionManager;

        /// <summary>
        /// Идентификатор диалоговых окон.
        /// </summary>
        readonly IDialogIdentifier dialogIdentifier;

        /// <summary>
        /// Клиент.
        /// </summary>
        readonly IClient client;

        /// <summary>
        /// Поставщик данных.
        /// </summary>
        readonly IDataProvider dataProvider;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public LoginViewModel(IRegionManager regionManager,
                              IClient client,
                              IDataProvider dataProvider,
                              IContainer container)
        {
            this.regionManager = regionManager;
            this.dialogIdentifier = container.ResolveRootDialogIdentifier();
            this.client = client;
            this.dataProvider = dataProvider;

            TryLoginCommand = new AsyncCommand(TryLogin, IsUserValid);

            LoginUser = new LoginUser
            {
                Login = Settings.Default.lastLogin,
                Password = Settings.Default.lastPassword
            };
        }

        /// <summary>
        /// Указывает, прошел ли валидацию пользователь.
        /// </summary>
        /// <returns></returns>
        private bool IsUserValid() => LoginUser.IsValid;

        /// <summary>
        /// Команда попытки входа в систему.
        /// </summary>
        public ICommand TryLoginCommand { get; }

        /// <summary>
        /// Попытка входа в систему.
        /// </summary>
        private async Task TryLogin()
        {
            if (!LoginUser.IsValid)
            {
                await dialogIdentifier.ShowMessageBoxAsync(LoginUser.Error, MaterialMessageBoxButtons.Ok);
                return;
            }

            Settings.Default.lastLogin = LoginUser.Login;
            Settings.Default.lastPassword = LoginUser.Password;

            var res = await client.Login.LoginAsync(LoginUser.Login, LoginUser.Password);
            User user;
            if (res && res.Arg == LoginResponse.Ok)
                user = res.Response;
            else
            {
                string msg;
                switch (res.Arg)
                {
                    case LoginResponse.Ok:
                        msg = "Очень странно, что вы видите это";
                        break;

                    case LoginResponse.Invalid:
                        msg = "Неправильный логин или пароль";
                        break;

                    case LoginResponse.ServerError:
                        msg = "Ошибка сервера";
                        break;

                    default:
                        msg = "Что-то явно пошло не так...";
                        break;
                }

                if (res.Code != ISTrainingPartResponseCode.Ok)
                    msg = res;

                await dialogIdentifier.ShowMessageBoxAsync(msg, MaterialMessageBoxButtons.Ok);
                return;
            }

            if (res && res.Arg == LoginResponse.Ok)
            {
                Consts.LoginStatus = true;

                Logger.Log.Info($"Успешный вход в систему: {{login: {user.Login}, mode: {user.Mode}}}");

                var parameters = NavigationParametersFluent.GetNavigationParameters().SetUser(user).SetValue("fromLogin", true);
                regionManager.RequestNavigateInRootRegion(RegionViews.MainView, parameters);
                regionManager.ReqeustNavigateInMainRegion(RegionViews.GroupsView, parameters);
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            dataProvider.Clear();

            if (navigationContext.Parameters.ContainsKey("fromConnecting"))
            {
                if (Consts.LoginStatus)
                    TryLoginCommand.Execute(null);
            }
        }
    }
}
