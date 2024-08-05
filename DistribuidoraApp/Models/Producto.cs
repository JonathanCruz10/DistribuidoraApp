namespace DistribuidoraApp.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public int TipoProductoId { get; set; }
        public bool EsActivo { get; set; }
        public decimal? Precio { get; set; }
        public List<ProductoProveedor> Proveedores { get; set; } = new List<ProductoProveedor>();
    }


}