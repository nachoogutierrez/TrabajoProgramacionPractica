namespace DeliveryBoy.Core.Payment
{
    public interface IPago
    {
        string Nombre { get; }
        bool Procesar(decimal monto);
    }
}
