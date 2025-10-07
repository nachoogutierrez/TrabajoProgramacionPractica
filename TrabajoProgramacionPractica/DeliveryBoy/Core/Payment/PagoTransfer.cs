using System;

namespace DeliveryBoy.Core.Payment
{
    public class PagoTransfer : IPago
    {
        public string Nombre => "Transferencia";

        public bool Procesar(decimal monto)
        {
            Console.WriteLine($"[PagoTransfer] Procesando {monto:C2} ... simulando acreditación");
            return true;
        }
    }
}
