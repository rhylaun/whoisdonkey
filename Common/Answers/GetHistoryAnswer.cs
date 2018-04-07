using System;
using System.Text;

namespace Donkey.Common.Answers
{
	[Serializable]
	public class GetHistoryAnswer : ServerAnswer
	{
		public GameMove[] History;

		public GetHistoryAnswer(bool success, GameMove[] history)
			: base(success)
		{
			History = history;
		}

		public string ToConsoleOutput()
		{
			var builder = new StringBuilder();

			for (int i = 0; i < History.Length; i++)
			{
				switch (History[i].MoveType)
				{
					case MoveType.Pass:
						builder.AppendLine(String.Format("[{0}] pass his turn", History[i].Player.Login, "Pass"));
						break;
					case MoveType.Clear:
						builder.AppendLine("Board was cleared!");
						break;
					case MoveType.Drop:
						builder.Append(String.Format("[{0}] move with ", History[i].Player.Login));
						foreach (var card in History[i].Cards)
							builder.AppendFormat("[{0}]", card);
						builder.AppendLine();
						break;
				}
			}

			return builder.ToString();
		}
	}
}
