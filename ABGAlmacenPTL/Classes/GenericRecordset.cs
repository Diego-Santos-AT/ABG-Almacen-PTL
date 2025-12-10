using System.Data;

namespace ABGAlmacenPTL.Classes
{
    /// <summary>
    /// Clase genérica de Recordset de memoria
    /// Migrado desde VB6 clGenericaRecordset.cls
    /// Fecha creación: 07/12/2000
    /// Autor: A. Moreno Marquéz
    /// </summary>
    public class GenericRecordset : IDisposable
    {
        private DataTable? _dataTable;
        private int _maxParam;
        private int _currentRow;
        private bool _disposed = false;

        /// <summary>
        /// DataTable interno que simula el recordset
        /// </summary>
        public DataTable? DataTable
        {
            get => _dataTable;
            private set => _dataTable = value;
        }

        /// <summary>
        /// Obtiene el número de registros
        /// </summary>
        public int RecordCount => _dataTable?.Rows.Count ?? 0;

        /// <summary>
        /// Indica si estamos al final del recordset
        /// </summary>
        public bool EOF => _currentRow >= RecordCount;

        /// <summary>
        /// Indica si estamos al inicio del recordset
        /// </summary>
        public bool BOF => _currentRow < 0 || RecordCount == 0;

        public GenericRecordset()
        {
            _currentRow = -1;
            _maxParam = 0;
        }

        /// <summary>
        /// Configura la clase y crea la estructura del recordset
        /// Migrado desde VB6: Configura_Clase
        /// </summary>
        /// <param name="columns">Array de columnas (nombre, tipo)</param>
        public void ConfigurarClase(params object[] columns)
        {
            _dataTable = new DataTable();
            
            // Procesar columnas de 2 en 2 (nombre, tipo)
            for (int i = 0; i < columns.Length; i += 2)
            {
                if (i + 1 < columns.Length)
                {
                    string columnName = columns[i].ToString() ?? $"Column{i}";
                    Type columnType = columns[i + 1] as Type ?? typeof(string);
                    
                    _dataTable.Columns.Add(columnName, columnType);
                }
            }
            
            _maxParam = _dataTable.Columns.Count;
        }

        /// <summary>
        /// Añade un nuevo registro
        /// Migrado desde VB6: AddNew
        /// </summary>
        public DataRow AddNew()
        {
            if (_dataTable == null)
            {
                throw new InvalidOperationException("Debe configurar el recordset antes de añadir registros");
            }
            
            var row = _dataTable.NewRow();
            return row;
        }

        /// <summary>
        /// Actualiza el registro actual
        /// Migrado desde VB6: Update
        /// </summary>
        public void Update(DataRow row)
        {
            if (_dataTable == null)
            {
                throw new InvalidOperationException("Debe configurar el recordset antes de actualizar registros");
            }
            
            if (row.RowState == DataRowState.Detached)
            {
                _dataTable.Rows.Add(row);
            }
        }

        /// <summary>
        /// Mueve al primer registro
        /// Migrado desde VB6: MoveFirst
        /// </summary>
        public void MoveFirst()
        {
            if (RecordCount > 0)
            {
                _currentRow = 0;
            }
        }

        /// <summary>
        /// Mueve al último registro
        /// Migrado desde VB6: MoveLast
        /// </summary>
        public void MoveLast()
        {
            if (RecordCount > 0)
            {
                _currentRow = RecordCount - 1;
            }
        }

        /// <summary>
        /// Mueve al siguiente registro
        /// Migrado desde VB6: MoveNext
        /// </summary>
        public void MoveNext()
        {
            if (_currentRow < RecordCount - 1)
            {
                _currentRow++;
            }
            else
            {
                _currentRow = RecordCount; // EOF
            }
        }

        /// <summary>
        /// Mueve al registro anterior
        /// Migrado desde VB6: MovePrevious
        /// </summary>
        public void MovePrevious()
        {
            if (_currentRow > 0)
            {
                _currentRow--;
            }
            else
            {
                _currentRow = -1; // BOF
            }
        }

        /// <summary>
        /// Obtiene el registro actual
        /// </summary>
        public DataRow? CurrentRow
        {
            get
            {
                if (_dataTable == null || _currentRow < 0 || _currentRow >= RecordCount)
                {
                    return null;
                }
                return _dataTable.Rows[_currentRow];
            }
        }

        /// <summary>
        /// Cierra el recordset
        /// Migrado desde VB6: Close
        /// </summary>
        public void Close()
        {
            _dataTable?.Clear();
            _dataTable?.Dispose();
            _dataTable = null;
            _currentRow = -1;
        }

        /// <summary>
        /// Obtiene un valor del registro actual por nombre de columna
        /// </summary>
        public object? this[string columnName]
        {
            get
            {
                var row = CurrentRow;
                if (row == null || !_dataTable!.Columns.Contains(columnName))
                {
                    return null;
                }
                return row[columnName];
            }
            set
            {
                var row = CurrentRow;
                if (row != null && _dataTable!.Columns.Contains(columnName))
                {
                    row[columnName] = value ?? DBNull.Value;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Close();
                }
                _disposed = true;
            }
        }
    }
}
