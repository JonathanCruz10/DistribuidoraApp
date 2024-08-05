namespace DistribuidoraApp.Models
{
    public class Proveedor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<ProductoProveedor> Productos { get; set; } = new List<ProductoProveedor>();
    }
}
