namespace DeliveryBoy.Core.Strategy
{
    public class EnvioService
    {
        private IEnvioStrategy _actual;

        public EnvioService(IEnvioStrategy inicial)
        {
            _actual = inicial;
        }

        public void SetStrategy(IEnvioStrategy s) => _actual = s;

        public decimal Calcular(decimal subtotal) => _actual.Calcular(subtotal);

        public string NombreActual => _actual.Nombre;
    }
}
