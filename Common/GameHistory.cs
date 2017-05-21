using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Common
{
	public class GameHistory
	{
		private readonly List<GameMove> _moveList;

		public GameHistory()
		{
			_moveList = new List<GameMove>();
		}

		public int Count
		{
			get
			{
				return _moveList.Count;
			}
		}

		public GameMove Last()
		{
			return _moveList.Last();
		}

		public bool Add(GameMove move)
		{
			if (_moveList.Count == move.Index)
			{
				_moveList.Add(move);
				return true;
			}

			return false;
		}

		public void Clear()
		{
			_moveList.Clear();
		}

		public GameMove[] ToArray(int fromIndex = 0)
		{
			if (fromIndex > _moveList.Count)
				throw new IndexOutOfRangeException();

			if (fromIndex < 0)
				fromIndex = 0;

			var tmpList = new List<GameMove>();
			for (int i = 0; i < _moveList.Count; i++)
			{
				if (_moveList[i].Index < fromIndex) continue;
				tmpList.Add(_moveList[i].GetCopy());
			}
			return tmpList.ToArray();
		}
	}
}
