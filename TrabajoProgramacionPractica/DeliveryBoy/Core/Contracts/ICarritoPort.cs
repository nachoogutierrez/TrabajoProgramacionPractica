using DeliveryBoy.Core.Command; // ← Agregar este using

namespace DeliveryBoy.Contracts;

public interface ICarritoPort
{
    void AgregarItem(string sku, string nombre, decimal precio, int cantidad);
    void CambiarCantidad(string sku, int nuevaCantidad);
    void QuitarItem(string sku);
    decimal Subtotal();
    System.Collections.Generic.IReadOnlyCollection<Item> Snapshot(); // ← Usar el Item correcto
    void Undo();
    void Redo();
}