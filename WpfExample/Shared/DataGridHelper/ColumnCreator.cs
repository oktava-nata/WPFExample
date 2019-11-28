using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace Shared.DataGridHelper
{
    public class ColumnCreator
    {
        public enum WidthValue
        {
            auto,
            small = 90,
            medium = 250,
            star
        }
        DataGrid _grid;
        System.Windows.DataTemplate _DoubleHeaderDataTemlate;

        List<DataGridTextColumn> _addedColumns = new List<DataGridTextColumn>();

        public ColumnCreator(DataGrid grid)
        {
            _grid = grid;
            _DoubleHeaderDataTemlate = (System.Windows.DataTemplate)(App.Current.Resources["DoubleHeaderDataTemlate"]);
        }

        /// <summary>
        /// Добавляет текстовую колонку в таблицу (обычный заголовок столбца)
        /// </summary>
        /// <param name="bindingText">Значение привязки (имя поля)</param>
        /// <param name="header">Текст заголовка колонки</param>
        /// <param name="converter">Конвертер для значений ячейки</param>
        /// <param name="width">Ширина колонки</param>
        public void AddTextColumn(string bindingText, string header, IValueConverter converter, WidthValue width = WidthValue.auto)
        {
            DataGridTextColumn col = new DataGridTextColumn();
            //заголовок
            col.Header = header;            

            SetBaseColumnProperties(bindingText, converter, width, col);
        }

        /// <summary>
        /// Добавляет текстовую колонку в таблицу (Заголовок столбца состоит из 2 строк)
        /// </summary>
        /// <param name="bindingText">Значение привязки (имя поля)</param>
        /// <param name="header1">Текст заголовка колонки (Основной текст)</param>
        /// <param name="header2">Текст заголовка колонки (Добавочная вторая строка)</param>
        /// <param name="converter">Конвертер для значений ячейки</param>
        /// <param name="width">Ширина колонки</param>
        public void AddTextColumn(string bindingText, string header1, string header2, IValueConverter converter, WidthValue width = WidthValue.auto)
        {
            List<string> header = new List<string>() { header1, header2 };
            DataGridTextColumn col = new DataGridTextColumn();
            //заголовок
            col.Header = header;
            col.HeaderTemplate = _DoubleHeaderDataTemlate;

            SetBaseColumnProperties(bindingText, converter, width, col);
        }

        private void SetBaseColumnProperties(string bindingText, IValueConverter converter, WidthValue width, DataGridTextColumn col)
        {
            //задаем привязку
            Binding binding = new Binding(bindingText);
            //задаем конвертер
            binding.Converter = converter;

            col.Binding = binding;
            //задаем сортировку
            //col.SortMemberPath = "Number";

            switch (width)
            {
                case WidthValue.auto:
                    col.MinWidth = (int)WidthValue.small;
                    break;
                case WidthValue.star:
                    col.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                    col.MinWidth = (int)WidthValue.small;
                    break;
                default:
                    col.Width = (int)width;
                    col.MinWidth = (int)width;
                    break;
            }
            _grid.Columns.Add(col);
            _addedColumns.Add(col);
        }

        public void RemoveAllAddedColumns()
        {
            foreach (var col in _addedColumns)
            {
                _grid.Columns.Remove(col);
            }
            _addedColumns.Clear();
        }
    }
}
