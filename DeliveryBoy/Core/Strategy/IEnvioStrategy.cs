namespace DeliveryBoy.Core.Strategy
{
	public interface IEnvioStrategy
	{
		decimal Calcular(decimal subtotal);
		string Nombre { get; }
	}
}
