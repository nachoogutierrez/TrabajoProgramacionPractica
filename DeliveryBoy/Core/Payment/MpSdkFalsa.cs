using System;

namespace DeliveryBoy.Core.Payment
{
    // Simula el SDK externo (interfaz distinta)
    public class MpSdkFalsa
    {
        public bool Cobrar(decimal monto)
        {
            Console.WriteLine($"[MpSdkFalsa] Cobrar {monto:C2} (simulado SDK)");
            return true;
        }
    }
}
