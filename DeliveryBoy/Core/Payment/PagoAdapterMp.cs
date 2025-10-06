namespace DeliveryBoy.Core.Payment
{
    public class PagoAdapterMp : IPago
    {
        private readonly MpSdkFalsa _sdk;

        public PagoAdapterMp(MpSdkFalsa sdk)
        {
            _sdk = sdk;
        }

        public string Nombre => "MpAdapter";

        public bool Procesar(decimal monto) => _sdk.Cobrar(monto);
    }
}
