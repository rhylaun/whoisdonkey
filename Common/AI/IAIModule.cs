namespace Donkey.Common.AI
{
	public interface IAIModule
	{
		GameMove ProcessMove(GameMove[] history, PlayerCardSet hand);
	}
}
