using DeliveryBoy.Core.Singleton;

namespace DeliveryBoy.Core.Payment
{
	public class PagoConImpuesto : IPago
	{
		private readonly IPago _inner;
		public PagoConImpuesto(IPago inner) => _inner = inner;
		public string Nombre => _inner.Nombre + " + IVA";

		public bool Procesar(decimal monto)
		{
			var total = monto * (1 + ConfigManager.Instance.IVA);
			return _inner.Procesar(total);
		}
	}
}
