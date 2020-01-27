﻿using DevExpress.Mvvm;
using System.Windows.Input;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Указывает, что ViewModel содержит в себе команды добавления, удаления, редактирования данных.
    /// </summary>
    interface IBaseViewModel<T>
    {
        /// <summary>
        /// Команда добавления.
        /// </summary>
        ICommand AddCommand { get; }

        /// <summary>
        /// Команда открытия окна редактирования.
        /// </summary>
        ICommand<T> EditCommand { get; }

        /// <summary>
        /// Команда удаления.
        /// </summary>
        ICommand<T> DeleteCommand { get; }

        /// <summary>
        /// Добавление.
        /// </summary>
        void Add();

        /// <summary>
        /// Редактирование.
        /// </summary>
        void Edit(T obj);

        /// <summary>
        /// Удаление.
        /// </summary>
        void Delete(T obj);
    }
}