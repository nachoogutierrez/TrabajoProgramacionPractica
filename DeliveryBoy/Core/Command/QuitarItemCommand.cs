namespace DeliveryBoy.Core.Command
{
    public class QuitarItemCommand : ICommand
    {
        private readonly Carrito _carrito;
        private readonly string _sku;
        private Item? _backup;

        public QuitarItemCommand(Carrito carrito, string sku)
        {
            _carrito = carrito;
            _sku = sku;
        }

        public void Execute() => _backup = _carrito.Quitar(_sku);

        public void Undo()
        {
            if (_backup != null)
                _carrito.Agregar(_backup);
        }
    }
}
