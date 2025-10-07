using System;

namespace DeliveryBoy.Core.Order.Observers
{
    public class AuditoriaObserver
    {
        public void Suscribir(DeliveryBoy.Core.Order.PedidoService s) => s.EstadoCambiado += Handle;
        public void Desuscribir(DeliveryBoy.Core.Order.PedidoService s) => s.EstadoCambiado -= Handle;
        private void Handle(object? sender, DeliveryBoy.Core.Order.PedidoChangedEventArgs e)
            => Console.WriteLine($"[Auditoria] {e.Cuando:O} - Pedido {e.PedidoId} cambió a {e.NuevoEstado}");
    }
}
