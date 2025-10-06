using System.Collections.Generic;

namespace DeliveryBoy.Core.Command
{
	public interface ICarritoPort
	{
		decimal Subtotal();
		void Run(ICommand cmd);
		void Undo();
		void Redo();
		IReadOnlyCollection<Item> Snapshot();
	}
}
