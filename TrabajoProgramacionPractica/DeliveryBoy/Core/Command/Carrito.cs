using System.Collections.Generic;
using System.Linq;

namespace DeliveryBoy.Core.Command
{
    public class Carrito
    {
        private readonly Dictionary<string, Item> _items = new();

        public void Agregar(Item i)
        {
            if (_items.TryGetValue(i.Sku, out var exist))
            {
                exist.Cantidad += i.Cantidad;
            }
            else
            {
                _items[i.Sku] = new Item { Sku = i.Sku, Nombre = i.Nombre, Precio = i.Precio, Cantidad = i.Cantidad };
            }
        }
        public Item? Quitar(string sku)
        {
            if (_items.TryGetValue(sku, out var item))
            {
                _items.Remove(sku);
                return new Item { Sku = item.Sku, Nombre = item.Nombre, Precio = item.Precio, Cantidad = item.Cantidad };
            }
            return null;
        }

        public bool SetCantidad(string sku, int nueva)
        {
            if (_items.TryGetValue(sku, out var item))
            {
                item.Cantidad = nueva;
                if (item.Cantidad <= 0)
                    _items.Remove(sku);
                return true;
            }
            return false;
        }

        public decimal Subtotal()
        {
            decimal sum = 0m;
            foreach (var it in _items.Values)
            {
                sum += it.Precio * it.Cantidad;
            }
            return sum;
        }

        public IReadOnlyCollection<Item> Snapshot()
    => _items.Values.Select(i => new Item
    {
        Sku = i.Sku,
        Nombre = i.Nombre,
        Precio = i.Precio,
        Cantidad = i.Cantidad
    }).ToList().AsReadOnly();

    }
}
