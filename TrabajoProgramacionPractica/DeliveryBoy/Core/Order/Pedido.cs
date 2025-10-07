using System.Collections.Generic;
using DeliveryBoy.Core.Command;

namespace DeliveryBoy.Core.Order
{
	public enum EstadoPedido { Recibido, Preparando, Enviado, Entregado }

	public class Pedido
	{
		public int Id { get; set; }
		public List<Item> Items { get; set; } = new();
		public string Direccion { get; set; } = "";
		public string MetodoPago { get; set; } = "";
		public EstadoPedido Estado { get; set; }
		public decimal Monto { get; set; }
	}
}
