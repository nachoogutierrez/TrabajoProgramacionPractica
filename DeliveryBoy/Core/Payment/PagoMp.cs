using System;

namespace DeliveryBoy.Core.Payment
{
    public class PagoMp : IPago
    {
        public string Nombre => "MercadoPago";

        public bool Procesar(decimal monto)
        {
            Console.WriteLine($"[PagoMp] Procesando {monto:C2} (simulado)");
            return true;
        }
    }
}
