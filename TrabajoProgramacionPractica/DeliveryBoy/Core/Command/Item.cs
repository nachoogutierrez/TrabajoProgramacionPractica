namespace DeliveryBoy.Core.Command
{
    public class Item
    {
        public string Sku { get; init; } = "";
        public string Nombre { get; init; } = "";
        public decimal Precio { get; init; }
        public int Cantidad { get; set; }
    }
}
