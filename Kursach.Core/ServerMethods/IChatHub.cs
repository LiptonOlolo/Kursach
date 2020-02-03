﻿using System.Threading.Tasks;

namespace Kursach.Core.ServerMethods
{
    /// <summary>
    /// Список методов хаба чата.
    /// </summary>
    public interface IChatHub
    {
        /// <summary>
        /// Отправить сообщение в чат.
        /// </summary>
        /// <param name="text">Текст сообщения.</param>
        void SendMessage(string text);
    }
}
