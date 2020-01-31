﻿using DevExpress.Mvvm;
using DryIoc;
using Kursach.Client.Interfaces;
using Kursach.Core;
using Kursach.Core.Models;
using Kursach.Properties;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using Prism.Regions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Login view model.
    /// </summary>
    class LoginViewModel : NavigationViewModel
    {
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
        /// Конструктор.
        /// </summary>
        public LoginViewModel(IRegionManager regionManager,
                              IClient client,
                              IContainer container)
        {
            this.regionManager = regionManager;
            this.dialogIdentifier = container.ResolveRootDialogIdentifier();
            this.client = client;

            TryLoginCommand = new AsyncCommand(TryLogin);

            User = new User
            {
                Login = Settings.Default.lastLogin,
                Password = Settings.Default.lastPassword
            };
        }

        /// <summary>
        /// Команда попытки входа в систему.
        /// </summary>
        public ICommand TryLoginCommand { get; }

        /// <summary>
        /// Попытка входа в систему.
        /// </summary>
        private async Task TryLogin()
        {
            if (!User.IsValid)
            {
                await dialogIdentifier.ShowMessageBoxAsync(User.Error, MaterialMessageBoxButtons.Ok);
                return;
            }

            Settings.Default.lastLogin = User.Login;
            Settings.Default.lastPassword = User.Password;

            var res = await client.Login.LoginAsync(User.Login, User.Password);
            User user = null;
            if (res && res.Error == LoginResponse.Ok)
                user = res.Response;
            else
            {
                string msg = null;
                switch (res.Error)
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

                if (res.Code != KursachResponseCode.Ok)
                    msg = res;

                await dialogIdentifier.ShowMessageBoxAsync(msg, MaterialMessageBoxButtons.Ok);
                return;
            }

            if (res && res.Error == LoginResponse.Ok)
            {
                Consts.LoginStatus = true;

                Logger.Log.Info($"Успешный вход в систему: {{{Logger.GetParamsNamesValues(() => User.Login)}}}");

                NavigationParameters parameters = NavigationParametersFluent.GetNavigationParameters().SetUser(user).SetValue("fromLogin", true);
                regionManager.RequestNavigateInRootRegion(RegionViews.MainView, parameters);
                regionManager.ReqeustNavigateInMainRegion(RegionViews.WelcomeView);
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("fromConnecting"))
            {
                if (Consts.LoginStatus)
                    TryLoginCommand.Execute(null);
            }
        }
    }
}
