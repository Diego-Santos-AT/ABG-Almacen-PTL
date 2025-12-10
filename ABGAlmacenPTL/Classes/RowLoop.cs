using System.Data;
using System.Collections;

namespace ABGAlmacenPTL.Classes
{
    /// <summary>
    /// Clase para iteración de filas en recordsets
    /// Migrado desde VB6 clsRowLoop.cls
    /// </summary>
    public class RowLoop : IEnumerable<DataRow>
    {
        private DataTable? _dataTable;
        private DataView? _dataView;
        private int _currentIndex;
        private string _filterExpression = string.Empty;

        /// <summary>
        /// Tabla de datos para iterar
        /// </summary>
        public DataTable? DataTable
        {
            get => _dataTable;
            set
            {
                _dataTable = value;
                _currentIndex = 0;
                if (_dataTable != null)
                {
                    _dataView = new DataView(_dataTable);
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
        /// Índice actual de iteración
        /// </summary>
        public int CurrentIndex
        {
            get => _currentIndex;
            set => _currentIndex = value;
        }

        /// <summary>
        /// Número total de filas
        /// </summary>
        public int RowCount => _dataView?.Count ?? 0;

        /// <summary>
        /// Indica si hay más filas para iterar
        /// </summary>
        public bool HasMore => _currentIndex < RowCount;

        /// <summary>
        /// Fila actual
        /// </summary>
        public DataRow? CurrentRow
        {
            get
            {
                if (_dataView == null || _currentIndex < 0 || _currentIndex >= RowCount)
                {
                    return null;
                }
                return _dataView[_currentIndex].Row;
            }
        }

        /// <summary>
        /// Aplica el filtro
        /// </summary>
        private void ApplyFilter()
        {
            if (_dataView != null)
            {
                _dataView.RowFilter = _filterExpression;
            }
            _currentIndex = 0;
        }

        /// <summary>
        /// Mueve al siguiente registro
        /// </summary>
        /// <returns>True si hay más registros, False si llegó al final</returns>
        public bool MoveNext()
        {
            if (HasMore)
            {
                _currentIndex++;
                return _currentIndex < RowCount;
            }
            return false;
        }

        /// <summary>
        /// Reinicia el iterador al inicio
        /// </summary>
        public void Reset()
        {
            _currentIndex = 0;
        }

        /// <summary>
        /// Obtiene el enumerador genérico
        /// </summary>
        public IEnumerator<DataRow> GetEnumerator()
        {
            if (_dataView == null)
            {
                yield break;
            }

            Reset();
            while (HasMore)
            {
                var row = CurrentRow;
                _currentIndex++;
                if (row != null)
                {
                    yield return row;
                }
            }
        }

        /// <summary>
        /// Obtiene el enumerador no genérico
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
