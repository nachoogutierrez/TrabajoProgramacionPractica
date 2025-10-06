using System;

namespace DeliveryBoy.Core.Payment
{
	public class PagoTarjeta : IPago
	{
		public string Nombre => "Tarjeta";

		public bool Procesar(decimal monto)
		{
			Console.WriteLine($"[PagoTarjeta] Procesando {monto:C2} ... simulando autorización");
			return true;
		}
	}
}
