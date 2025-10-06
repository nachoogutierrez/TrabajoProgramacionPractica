using System;

namespace DeliveryBoy.Core.Order
{
	public class PedidoChangedEventArgs : EventArgs
	{
		public int PedidoId { get; }
		public EstadoPedido NuevoEstado { get; }
		public DateTime Cuando { get; }
		public PedidoChangedEventArgs(int id, EstadoPedido estado, DateTime cuando) => (PedidoId, NuevoEstado, Cuando) = (id, estado, cuando);
	}

	public class PedidoService
	{
		public event EventHandler<PedidoChangedEventArgs>? EstadoCambiado;

		public void CambiarEstado(int pedidoId, EstadoPedido nuevo)
			=> EstadoCambiado?.Invoke(this, new PedidoChangedEventArgs(pedidoId, nuevo, DateTime.Now));
	}
}
