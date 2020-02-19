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
    /// Управление сотрудниками.
    /// </summary>
    class Staffs : Invoker, IStaff
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public Staffs(IHubConfigurator hubConfigurator) : base(hubConfigurator, HubNames.StaffHub)
        {
            Proxy.On<DbChangeStatus, Staff>(nameof(IStaffHubEvents.OnChanged),
                (status, staff) => OnChanged?.Invoke(status, staff));
        }

        /// <summary>
        /// Сотрудник изменен.
        /// </summary>
        public event OnChanged<Staff> OnChanged;

        #region Get region
        /// <summary>
        /// Получение всех работников.
        /// </summary>
        /// <returns></returns>
        public Task<KursachResponse<IEnumerable<Staff>>> GetStaffsAsync()
        {
            return TryInvokeAsync<IEnumerable<Staff>>();
        }

        /// <summary>
        /// Получить первого (создать если нет) сотрудника.
        /// </summary>
        /// <returns></returns>
        public Task<KursachResponse<Staff, bool>> GetOrCreateFirstStaffAsync()
        {
            return TryInvokeAsync<Staff, bool>();
        }
        #endregion

        #region CUD region
        /// <summary>
        /// Добавить сотрудника.
        /// </summary>
        /// <param name="staff">Сотрудник.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> AddStaffAsync(Staff staff)
        {
            return TryInvokeAsync<bool>(args: staff);
        }

        /// <summary>
        /// Сохранить сотрудника.
        /// </summary>
        /// <param name="staff">Сотрудник.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> SaveStaffAsync(Staff staff)
        {
            return TryInvokeAsync<bool>(args: staff);
        }

        /// <summary>
        /// Удалить сотрудника.
        /// </summary>
        /// <param name="staff">Сотрудник.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> RemoveStaffAsync(Staff staff)
        {
            return TryInvokeAsync<bool>(args: staff);
        }
        #endregion
    }
}
