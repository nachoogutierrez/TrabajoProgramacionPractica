namespace DeliveryBoy.Core.Payment
{
	public class PagoConCupon : IPago
	{
		private readonly IPago _inner;
		private readonly decimal _porcentaje;

		public PagoConCupon(IPago inner, decimal porcentaje)
		{
			_inner = inner;
			_porcentaje = porcentaje;
		}

		public string Nombre => _inner.Nombre + $" - Cupon {_porcentaje:P}";

		public bool Procesar(decimal monto)
		{
			var total = monto * (1 - _porcentaje);
			if (total < 0m) total = 0m;
			return _inner.Procesar(total);
		}
	}
}
