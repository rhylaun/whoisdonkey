using Donkey.Client;

namespace DonkeyAI
{
	public static class AIManager
	{
		public static AIWorker StartAI(IGameClient client)
		{
			var worker = new AIWorker(client, new DropMaxPointsStrategy());
			worker.Start();
			return worker;
		}

	}
}
