using System.Collections.Generic;

namespace DeliveryBoy.Core.Command
{
	public interface ICarritoPort
	{
		List<Item> GetItemsSnapshot();
        decimal Subtotal();
		void Run(ICommand cmd);
		void Undo();
		void Redo();
		IReadOnlyCollection<Item> Snapshot();
	}
}
