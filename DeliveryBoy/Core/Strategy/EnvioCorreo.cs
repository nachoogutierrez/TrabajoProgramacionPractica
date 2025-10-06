using DeliveryBoy.Core.Singleton;

namespace DeliveryBoy.Core.Strategy
{
    public class EnvioCorreo : IEnvioStrategy
    {
        public string Nombre => "Correo";

        public decimal Calcular(decimal subtotal)
        {
            return subtotal >= ConfigManager.Instance.EnvioGratisDesde ? 0m : 3500m;
        }
    }
}
