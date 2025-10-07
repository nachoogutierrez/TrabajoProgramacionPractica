using System;

namespace DeliveryBoy.Core.Payment
{
    public static class PagoFactory
    {
        public static IPago Create(string tipo)
        {
            tipo = (tipo ?? "").ToLowerInvariant();
            return tipo switch
            {
                "tarjeta" => new PagoTarjeta(),
                "transf" or "transferencia" => new PagoTransfer(),
                "mp" => new PagoMp(),
                _ => throw new ArgumentException($"Tipo de pago desconocido: {tipo}")
            };
        }
    }
}
