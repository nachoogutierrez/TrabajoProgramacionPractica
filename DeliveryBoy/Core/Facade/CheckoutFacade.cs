using System;
using System.Linq;
using System.Threading;
using DeliveryBoy.Core.Command;
using DeliveryBoy.Core.Order;
using DeliveryBoy.Core.Payment;
using DeliveryBoy.Core.Strategy;

namespace DeliveryBoy.Core.Facade
{
	public class CheckoutFacade
	{
		private readonly ICarritoPort _carrito;
		private IEnvioStrategy _envioActual;
		private readonly PedidoService _pedidos;

		public CheckoutFacade(ICarritoPort carrito, IEnvioStrategy envioInicial, PedidoService pedidos)
		{
			_carrito = carrito;
			_envioActual = envioInicial;
			_pedidos = pedidos;
		}

		public void AgregarItem(string sku, string nombre, decimal precio, int cantidad)
		{
			var item = new Item { Sku = sku, Nombre = nombre, Precio = precio, Cantidad = cantidad };
			_carrito.Run(new AgregarItemCommand(new CarritoAdapter(_carrito), item));
		}

		public void CambiarCantidad(string sku, int cantidad)
		{
			_carrito.Run(new SetCantidadCommand(new CarritoAdapter(_carrito), sku, cantidad));
		}

		public void QuitarItem(string sku)
		{
			_carrito.Run(new QuitarItemCommand(new CarritoAdapter(_carrito), sku));
		}

		public void ElegirEnvio(IEnvioStrategy estrategia) => _envioActual = estrategia;

		public decimal CalcularTotal()
		{
			var subtotal = _carrito.Subtotal();
			var envio = _envioActual.Calcular(subtotal);
			return subtotal + envio;
		}

		public bool Pagar(string tipoPago, bool aplicarIVA, decimal? cupon = null)
		{
			IPago pago;
			if (tipoPago?.ToLowerInvariant() == "mp-adapter")
			{
				pago = new PagoAdapterMp(new MpSdkFalsa());
			}
			else
			{
				pago = PagoFactory.Create(tipoPago);
			}

			if (aplicarIVA) pago = new PagoConImpuesto(pago);
			if (cupon.HasValue) pago = new PagoConCupon(pago, cupon.Value);

			var monto = CalcularTotal();
			return pago.Procesar(monto);
		}

		public Pedido ConfirmarPedido(string direccion, string tipoPago)
		{
			var items = _carrito.Snapshot().Select(i => (i.Sku, i.Nombre, i.Precio, i.Cantidad));
			var monto = CalcularTotal();

			var builder = new PedidoBuilder();
			var pedido = builder.ConItems(items).ConDireccion(direccion).ConMetodoPago(tipoPago).ConMonto(monto).Build();

			// Simular flujo con notificaciones
			_pedidos.CambiarEstado(pedido.Id, EstadoPedido.Recibido);
			Thread.Sleep(300); // simulación ligera
			_pedidos.CambiarEstado(pedido.Id, EstadoPedido.Preparando);
			Thread.Sleep(300);
			_pedidos.CambiarEstado(pedido.Id, EstadoPedido.Enviado);
			Thread.Sleep(300);
			_pedidos.CambiarEstado(pedido.Id, EstadoPedido.Entregado);

			return pedido;
		}
		private class CarritoAdapter : Carrito
		{
			public CarritoAdapter(ICarritoPort port)
			{
				foreach (var it in port.Snapshot())
					base.Agregar(new DeliveryBoy.Core.Command.Item { Sku = it.Sku, Nombre = it.Nombre, Precio = it.Precio, Cantidad = it.Cantidad });
			}
		}
	}
}

