namespace DeliveryBoy.Core.Command
{
    public class SetCantidadCommand : ICommand
    {
        private readonly Carrito _carrito;
        private readonly string _sku;
        private readonly int _nueva;
        private int _anterior = 0;
        private bool _aplico = false;

        public SetCantidadCommand(Carrito carrito, string sku, int nueva)
        {
            _carrito = carrito;
            _sku = sku;
            _nueva = nueva;
        }

        public void Execute()
        {
            var snapshot = _carrito.Snapshot();
            var item = System.Linq.Enumerable.FirstOrDefault(snapshot, i => i.Sku == _sku);
            if (item != null)
            {
                _anterior = item.Cantidad;
                _aplico = _carrito.SetCantidad(_sku, _nueva);
            }
            else
            {
                _aplico = false;
            }
        }

        public void Undo()
        {
            if (_aplico)
                _carrito.SetCantidad(_sku, _anterior);
        }
    }
}
