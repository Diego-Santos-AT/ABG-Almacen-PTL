using System.Collections.ObjectModel;
using System.Data;

namespace ABGAlmacenPTL.Pages.Generic
{
    /// <summary>
    /// Formulario de selección de datos desde una tabla/recordset
    /// Migrado desde VB6 frmSeleccionTabla2.frm
    /// Creado: Original VB6
    /// Ult. Mod.: 23/04/02 (VB6)
    /// </summary>
    public partial class SeleccionTabla2Page : ContentPage
    {
        private bool _resultado = false;
        private TaskCompletionSource<(bool, object?)>? _tcs;
        private ObservableCollection<SelectionItem> _items = new();
        private ObservableCollection<SelectionItem> _allItems = new();
        private object? _selectedValue;

        public bool Resultado => _resultado;
        public object? SelectedValue => _selectedValue;

        public SeleccionTabla2Page()
        {
            InitializeComponent();
            collectionView.ItemsSource = _items;
        }

        /// <summary>
        /// Muestra el formulario de selección con datos de un DataTable
        /// </summary>
        /// <param name="titulo">Título del formulario</param>
        /// <param name="dataTable">DataTable con los datos</param>
        /// <param name="displayColumn">Nombre de la columna a mostrar</param>
        /// <param name="valueColumn">Nombre de la columna del valor (opcional, usa displayColumn si no se especifica)</param>
        /// <returns>Tupla con (seleccionado: bool, valor: object?)</returns>
        public static async Task<(bool, object?)> ShowAsync(
            string titulo, 
            DataTable dataTable, 
            string displayColumn,
            string? valueColumn = null)
        {
            var page = new SeleccionTabla2Page();
            page.Title = titulo;
            page._tcs = new TaskCompletionSource<(bool, object?)>();

            // Cargar datos
            page.LoadData(dataTable, displayColumn, valueColumn ?? displayColumn);
            
            await Application.Current!.Windows[0].Page!.Navigation.PushModalAsync(page);
            
            return await page._tcs.Task;
        }

        /// <summary>
        /// Muestra el formulario de selección con una lista de items
        /// </summary>
        public static async Task<(bool, object?)> ShowAsync(
            string titulo,
            IEnumerable<SelectionItem> items)
        {
            var page = new SeleccionTabla2Page();
            page.Title = titulo;
            page._tcs = new TaskCompletionSource<(bool, object?)>();

            // Cargar items
            foreach (var item in items)
            {
                page._items.Add(item);
                page._allItems.Add(item);
            }
            
            await Application.Current!.Windows[0].Page!.Navigation.PushModalAsync(page);
            
            return await page._tcs.Task;
        }

        private void LoadData(DataTable dataTable, string displayColumn, string valueColumn)
        {
            _items.Clear();
            _allItems.Clear();

            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                var item = new SelectionItem
                {
                    DisplayText = row[displayColumn]?.ToString() ?? string.Empty,
                    Value = row[valueColumn]
                };
                
                _items.Add(item);
                _allItems.Add(item);
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is SelectionItem selectedItem)
            {
                // Usuario seleccionó un item - cerrar formulario
                _resultado = true;
                _selectedValue = selectedItem.Value;
                _tcs?.SetResult((_resultado, _selectedValue));
                Navigation.PopModalAsync();
            }
        }

        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            FilterItems(e.NewTextValue);
        }

        private void FilterItems(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                // Mostrar todos los items
                _items.Clear();
                foreach (var item in _allItems)
                {
                    _items.Add(item);
                }
            }
            else
            {
                // Filtrar items
                var filtered = _allItems
                    .Where(i => i.DisplayText.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                _items.Clear();
                foreach (var item in filtered)
                {
                    _items.Add(item);
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            // Usuario canceló - cerrar formulario
            _resultado = false;
            _selectedValue = null;
            _tcs?.SetResult((_resultado, _selectedValue));
            Navigation.PopModalAsync();
            return true;
        }
    }

    /// <summary>
    /// Item de selección para el formulario
    /// </summary>
    public class SelectionItem
    {
        public string DisplayText { get; set; } = string.Empty;
        public object? Value { get; set; }
    }
}
