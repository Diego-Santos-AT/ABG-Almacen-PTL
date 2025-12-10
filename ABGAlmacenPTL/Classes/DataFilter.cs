using System.Data;

namespace ABGAlmacenPTL.Classes
{
    /// <summary>
    /// Clase para filtrado de datos en recordsets
    /// Migrado desde VB6 clsDataFilter.cls
    /// </summary>
    public class DataFilter
    {
        private DataTable? _sourceTable;
        private DataView? _filteredView;
        private string _filterExpression = string.Empty;
        private string _sortExpression = string.Empty;

        /// <summary>
        /// Tabla de datos origen
        /// </summary>
        public DataTable? SourceTable
        {
            get => _sourceTable;
            set
            {
                _sourceTable = value;
                if (_sourceTable != null)
                {
                    _filteredView = new DataView(_sourceTable);
                    ApplyFilter();
                }
            }
        }

        /// <summary>
        /// Expresión de filtro
        /// </summary>
        public string Filter
        {
            get => _filterExpression;
            set
            {
                _filterExpression = value ?? string.Empty;
                ApplyFilter();
            }
        }

        /// <summary>
        /// Expresión de ordenamiento
        /// </summary>
        public string Sort
        {
            get => _sortExpression;
            set
            {
                _sortExpression = value ?? string.Empty;
                ApplyFilter();
            }
        }

        /// <summary>
        /// Vista filtrada de los datos
        /// </summary>
        public DataView? FilteredView => _filteredView;

        /// <summary>
        /// Número de registros filtrados
        /// </summary>
        public int RecordCount => _filteredView?.Count ?? 0;

        /// <summary>
        /// Aplica el filtro y ordenamiento
        /// </summary>
        private void ApplyFilter()
        {
            if (_filteredView != null)
            {
                _filteredView.RowFilter = _filterExpression;
                _filteredView.Sort = _sortExpression;
            }
        }

        /// <summary>
        /// Limpia el filtro
        /// </summary>
        public void ClearFilter()
        {
            Filter = string.Empty;
            Sort = string.Empty;
        }

        /// <summary>
        /// Obtiene el DataTable filtrado
        /// </summary>
        public DataTable? GetFilteredTable()
        {
            if (_filteredView == null)
            {
                return null;
            }
            
            return _filteredView.ToTable();
        }
    }
}
