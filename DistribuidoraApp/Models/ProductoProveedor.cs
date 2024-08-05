namespace DistribuidoraApp.Models
    
{
    public class ProductoProveedor
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int ProveedorId { get; set; }
        public string ClaveProducto { get; set; }
        public decimal Costo { get; set; }
        public string NombreProveedor { get; set; } // Para mostrar en la UI
    }
}
