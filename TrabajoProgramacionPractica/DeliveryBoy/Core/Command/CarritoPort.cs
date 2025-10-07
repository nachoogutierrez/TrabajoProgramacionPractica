namespace DeliveryBoy.Core.Command
{
    public class CarritoPort : ICarritoPort
    {
        private readonly Carrito _carrito = new();
        private readonly EditorCarrito _editor = new();

        public decimal Subtotal() => _carrito.Subtotal();

        public void Run(ICommand cmd) => _editor.Run(cmd);

        public void Undo() => _editor.Undo();

        public void Redo() => _editor.Redo();

        public IReadOnlyCollection<Item> Snapshot() => _carrito.Snapshot();

    
        public List<Item> GetItemsSnapshot()
        {
            return Snapshot().ToList();
        }


        public void AgregarItem(string sku, string nombre, decimal precio, int cantidad)
        {
            var item = new Item { Sku = sku, Nombre = nombre, Precio = precio, Cantidad = cantidad };
            Run(new AgregarItemCommand(_carrito, item));
        }

        public void CambiarCantidad(string sku, int nuevaCantidad)
        {
            Run(new SetCantidadCommand(_carrito, sku, nuevaCantidad));
        }

        public void QuitarItem(string sku)
        {
            Run(new QuitarItemCommand(_carrito, sku));
        }
    }
}