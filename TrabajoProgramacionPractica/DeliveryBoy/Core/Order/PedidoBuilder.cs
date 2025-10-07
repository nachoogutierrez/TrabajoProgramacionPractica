using System;
using System.Collections.Generic;
using DeliveryBoy.Core.Command;

namespace DeliveryBoy.Core.Order
{
    public interface IPedidoBuilder
    {
        IPedidoBuilder ConItems(IEnumerable<(string sku, string nombre, decimal precio, int cantidad)> items);
        IPedidoBuilder ConDireccion(string direccion);
        IPedidoBuilder ConMetodoPago(string tipoPago);
        IPedidoBuilder ConMonto(decimal monto);
        Pedido Build();
    }

    public class PedidoBuilder : IPedidoBuilder
    {
        private readonly Pedido _p = new();

        public IPedidoBuilder ConItems(IEnumerable<(string sku, string nombre, decimal precio, int cantidad)> items)
        {
            _p.Items = new List<Item>();
            foreach (var it in items)
                _p.Items.Add(new Item { Sku = it.sku, Nombre = it.nombre, Precio = it.precio, Cantidad = it.cantidad });
            return this;
        }

        public IPedidoBuilder ConDireccion(string direccion)
        {
            _p.Direccion = direccion;
            return this;
        }

        public IPedidoBuilder ConMetodoPago(string tipoPago)
        {
            _p.MetodoPago = tipoPago;
            return this;
        }

        public IPedidoBuilder ConMonto(decimal monto)
        {
            _p.Monto = monto;
            return this;
        }

        public Pedido Build()
        {
            if (_p.Items == null || _p.Items.Count == 0) throw new InvalidOperationException("Pedido debe tener items.");
            if (string.IsNullOrWhiteSpace(_p.Direccion)) throw new InvalidOperationException("Pedido necesita dirección.");
            _p.Id = new Random().Next(1000, 9999);
            _p.Estado = EstadoPedido.Recibido;
            return _p;
        }
    }
}
