using System;

namespace DeliveryBoy.Core.Order.Observers
{
	public class LogisticaObserver
	{
		public void Suscribir(DeliveryBoy.Core.Order.PedidoService s) => s.EstadoCambiado += Handle;
		public void Desuscribir(DeliveryBoy.Core.Order.PedidoService s) => s.EstadoCambiado -= Handle;
		private void Handle(object? sender, DeliveryBoy.Core.Order.PedidoChangedEventArgs e)
			=> Console.WriteLine($"[Logistica] Tablero: Pedido {e.PedidoId} -> {e.NuevoEstado}");
	}
}
