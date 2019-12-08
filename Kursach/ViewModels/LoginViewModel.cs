﻿using DevExpress.Mvvm;
using Kursach.DataBase;
using Kursach.Models;
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
    class LoginViewModel : ViewModelBase
    {
        /// <summary>
        /// Пользователь для авторизации.
        /// </summary>
        public LoginUser User { get; }

        /// <summary>
        /// Менеджер регионов.
        /// </summary>
        readonly IRegionManager regionManager;

        /// <summary>
        /// Идентификатор диалоговых окон.
        /// </summary>
        readonly IDialogIdentifier dialogIdentifier;

        /// <summary>
        /// База данных.
        /// </summary>
        readonly IDataBase dataBase;

        /// <summary>
        /// Ctor.
        /// </summary>
        public LoginViewModel(IRegionManager regionManager, IDialogIdentifier dialogIdentifier, IDataBase dataBase)
        {
            this.regionManager = regionManager;
            this.dialogIdentifier = dialogIdentifier;
            this.dataBase = dataBase;

            TryLoginCommand = new AsyncCommand(TryLogin);

            User = new LoginUser();
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

            var user = await dataBase.GetUserAsync(User.Login, User.Password, true);

            if (user == null)
            {
                await dialogIdentifier.ShowMessageBoxAsync("Неверный логин или пароль", MaterialMessageBoxButtons.Ok);
                Logger.Log.Info($"Неудачная попытка входа в систему: {{login: {User.Login}}}");
                return;
            }
            else
            {
                Logger.Log.Info($"Успешный вход в систему: {{login: {User.Login}}}");
                await dataBase.AddSignInLogAsync(user);

                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("user", user);
                parameters.Add("fromLogin", true);
                regionManager.RequestNavigateInRootRegion(RegionViews.MainView, parameters);
                regionManager.RequstNavigateInMainRegion(RegionViews.WelcomeView);
            }
        }
    }
}
