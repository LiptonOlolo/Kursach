﻿using System.ComponentModel;

namespace Kursach.Core.Models
{
    /// <summary>
    /// Права пользователя.
    /// </summary>
    public enum UserMode
    {
        [Description("Администратор")]
        Admin = 0,

        [Description("Только просмотр")]
        Read = 1,

        [Description("Просмотр и редактирование")]
        ReadWrite = 2
    }
}
