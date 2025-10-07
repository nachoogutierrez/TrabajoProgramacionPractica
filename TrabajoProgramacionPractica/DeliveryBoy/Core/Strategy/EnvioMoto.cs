namespace DeliveryBoy.Core.Strategy
{
    public class EnvioMoto : IEnvioStrategy
    {
        public string Nombre => "Moto";
        public decimal Calcular(decimal subtotal) => 1200m;
    }
}
