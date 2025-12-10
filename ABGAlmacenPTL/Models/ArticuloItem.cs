namespace ABGAlmacenPTL.Models;

/// <summary>
/// Item de artículo para el CollectionView de páginas PTL
/// Usado en ConsultaPTL y EmpaquetarBAC
/// </summary>
public class ArticuloItem
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public int Cantidad { get; set; }
}
