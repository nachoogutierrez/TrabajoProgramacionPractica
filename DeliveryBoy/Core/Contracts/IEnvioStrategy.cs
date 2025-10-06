namespace DeliveryGo.Core.Contracts;

public interface IEnvioStrategy
{
    decimal Calcular(decimal subtotal);
    string Nombre { get; }
}