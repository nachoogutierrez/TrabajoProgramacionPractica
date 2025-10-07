public class AgregarItemCommand : ICommand
{
    private readonly Carrito _carrito;
    private readonly Item _item;

    public AgregarItemCommand(Carrito carrito, Item item)
    {
        _carrito = carrito;
        _item = item;
    }

    public void Execute()
    {
        _carrito.Agregar(_item); 
    }
}