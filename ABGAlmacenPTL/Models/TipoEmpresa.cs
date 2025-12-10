namespace ABGAlmacenPTL.Models
{
    /// <summary>
    /// Tipo de datos para Empresa
    /// Migrado desde VB6 Type TipoEmpresa
    /// </summary>
    public class TipoEmpresa
    {
        // --- Codigo de Empresa
        public int Codigo { get; set; }
        
        // --- Nombre de Empresa
        public string Nombre { get; set; } = string.Empty;
        
        // --- Servidor de Empresa
        public string Servidor { get; set; } = string.Empty;
        
        // --- Base de Datos de Empresa
        public string Base { get; set; } = string.Empty;
        
        // --- Usuario de Acceso
        public string Usuario { get; set; } = string.Empty;
        
        // --- Clave de Usuario
        public string Clave { get; set; } = string.Empty;
        
        // --- DSN
        public string Dsn { get; set; } = string.Empty;
        
        // --- DLL
        public string Dll { get; set; } = string.Empty;
        
        // --- Ruta de fotos para Imprimir
        public string RutaFotosImpresion { get; set; } = string.Empty;
        
        // --- Ruta de fotos para consulta
        public string RutaFotosConsulta { get; set; } = string.Empty;
        
        // --- Ruta de informes Crystal Report
        public string RutaInformes { get; set; } = string.Empty;
        
        // --- Logotipo de la empresa (jpg)
        public string Logo { get; set; } = string.Empty;
        
        // --- Codigo de ContraEmpresa por defecto
        public string ContraEmpresa { get; set; } = string.Empty;
        
        // --- Marca de empresa de desarrollo o gestión
        public bool MarcaDesarrollo { get; set; }
        
        // --- Marca de empresa activa o inactiva
        public bool MarcaActiva { get; set; }
        
        // --- Marca de empresa si es Compradora de otras empresas o NO
        public bool MarcaCompradora { get; set; }
        
        // --- Marca de empresa si es Importadora de otras empresas o NO
        public bool MarcaImportadora { get; set; }
        
        // --- Empresa de Contabilidad BT para DIMONI
        public string Contabilidad_BT { get; set; } = string.Empty;
        
        // --- Empresa de Contabilidad TT para DIMONI
        public string Contabilidad_TT { get; set; } = string.Empty;
        
        // --- Codigo EAN de empresa
        public string Ean { get; set; } = string.Empty;
        
        // --- CIF de empresa
        public string CIF { get; set; } = string.Empty;
        
        // --- Población
        public string Poblacion { get; set; } = string.Empty;
        
        // --- Codigo Postal
        public string CodigoPostal { get; set; } = string.Empty;
        
        // --- Dirección
        public string Direccion { get; set; } = string.Empty;
        
        // --- e-mail
        public string EMail { get; set; } = string.Empty;
        
        // --- Dirección web
        public string Web { get; set; } = string.Empty;
        
        // --- Servidor de Empresa para RadioFrecuencia
        public string Servidor_RadioFrecuencia { get; set; } = string.Empty;
        
        // --- Base de Datos de Empresa para RadioFrecuencia
        public string Base_RadioFrecuencia { get; set; } = string.Empty;
        
        // --- Usuario de Acceso para RadioFrecuencia
        public string Usuario_RadioFrecuencia { get; set; } = string.Empty;
        
        // --- Clave de Usuario para RadioFrecuencia
        public string Clave_RadioFrecuencia { get; set; } = string.Empty;
        
        // --- Codigo de Almacen Fisico de la Empresa
        public int Almacen_Fisico { get; set; }
        
        // -- Ubicacion Suelo Aduana por Defecto
        public string Suelo_Aduana { get; set; } = string.Empty;
        
        // -- Ubicacion Suelo Almacen por Defecto
        public string Suelo_Almacen { get; set; } = string.Empty;
        
        // -- Ubicacion Suelo Devoluciones por Defecto
        public string Suelo_Devolucion { get; set; } = string.Empty;
        
        // -- Ubicacion Suelo Compras entre empresas por Defecto
        public string Suelo_Compras { get; set; } = string.Empty;
        
        // -- Servidor de empresa para Gestion de Almacen
        public string Servidor_GestionAlmacen { get; set; } = string.Empty;
        
        // -- Base de Datos de empresa para Gestion de Almacen
        public string Base_GestionAlmacen { get; set; } = string.Empty;
        
        // -- usuario de acceso para Gestion de Almacen
        public string Usuario_GestionAlmacen { get; set; } = string.Empty;
        
        // -- Clave de usuario para Gestion de Almacen
        public string Clave_GestionAlmacen { get; set; } = string.Empty;
    }
}
