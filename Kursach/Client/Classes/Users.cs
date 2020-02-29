﻿using ISTraining_Part.Client.Delegates;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerEvents;
using Microsoft.AspNet.SignalR.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISTraining_Part.Client.Classes
{
    /// <summary>
    /// Управление пользователями.
    /// </summary>
    class Users : Invoker, IUsers
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public Users(IHubConfigurator hubConfigurator) : base(hubConfigurator, HubNames.UsersHub)
        {
            Proxy.On<DbChangeStatus, User>(nameof(IUsersHubEvents.OnChanged),
                (status, user) => OnChanged?.Invoke(status, user));
        }

        /// <summary>
        /// Изменения пользователя.
        /// </summary>
        public event OnChanged<User> OnChanged;

        #region Get region
        /// <summary>
        /// Получение списка всех пользователей.
        /// </summary>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<IEnumerable<User>>> GetUsersAsync()
        {
            return TryInvokeAsync<IEnumerable<User>>();
        }

        /// <summary>
        /// Получить пользователя.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <param name="usePassword">Проверять пароль.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<User>> GetUserAsync(string login, string password, bool usePassword)
        {
            return TryInvokeAsync<User>(args: new object[] { login, password, usePassword });
        }
        #endregion

        #region Log region
        /// <summary>
        /// Получить логи входов пользователя.
        /// </summary>
        /// <param name="userId">ИД пользователя.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<IEnumerable<SignInLog>>> GetSignInLogsAsync(int userId)
        {
            return TryInvokeAsync<IEnumerable<SignInLog>>(args: userId);
        }
        #endregion

        #region CUD region
        /// <summary>
        /// Регистрация нового пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<bool>> AddUserAsync(User user)
        {
            return TryInvokeAsync<bool>(args: user);
        }

        /// <summary>
        /// Сохранить пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<bool>> SaveUserAsync(User user)
        {
            return TryInvokeAsync<bool>(args: user);
        }

        /// <summary>
        /// Удаление пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<bool>> RemoveUserAsync(User user)
        {
            return TryInvokeAsync<bool>(args: user);
        }
        #endregion
    }
}
