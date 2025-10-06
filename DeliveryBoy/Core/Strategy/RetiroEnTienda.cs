namespace DeliveryBoy.Core.Strategy
{
    public class RetiroEnTienda : IEnvioStrategy
    {
        public string Nombre => "Retiro";
        public decimal Calcular(decimal subtotal) => 0m;
    }
}
