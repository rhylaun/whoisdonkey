using Donkey.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Server.Storage
{
	internal static class DbRequestHelper
	{
		public static string GenerateConnectionString(string fpath)
		{
			return string.Format("Data Source={0};Verison=3;FailIfMissing=True;", fpath);
		}

		public static string GenerateCreateAccountsTableRequest()
		{
			var req = "CREATE TABLE IF NOT EXISTS accounts (name TEXT UNIQUE PRIMARY KEY, password TEXT);";
			return req;
		}

		public static string GenerateCreateHistoryTableRequest()
		{
			var req = "CREATE TABLE IF NOT EXISTS history (moveindex INTEGER UNIQUE PRIMARY KEY, move TEXT, name TEXT, date TEXT, cards Text);";
			return req;
		}

		public static string GenerateReadAccountsRequest()
		{
			var req = "SELECT name, password FROM accounts;";
			return req;
		}

		public static string GenerateWriteAccountsRequest(List<AuthData> accounts)
		{
			var req = "INSERT OR IGNORE INTO accounts (name, password) VALUES ";
			for (int i = 0; i < accounts.Count - 1; i++)
				req += string.Format("('{0}','{1}'), ", accounts[i].Login, accounts[i].Password);
			var last = accounts.Count - 1;
			req+= string.Format("('{0}','{1}');", accounts[last].Login, accounts[last].Password);
			return req;
		}

		public static string GenerateWriteGameMoveRequest()
		{
			var req = "INSERT OR IGNORE INTO history (moveindex, move, name, date, cards) VALUES ($moveindex, $move, $name, $date, $cards);"; 
			return req;
		}

		public static string GenerateReadGameHistory()
		{
			var req = "SELECT * FROM history ORDER BY moveindex ASC;";
			return req;
		}

		public static string ConvertCardList(List<Card> list)
		{
			var result = string.Empty;
			foreach (var o in list)
				result += o.ToString() + ";";
			return result;
		}

		public static List<Card> ConvertCardString(string cards)
		{
			var list = new List<Card>();
			var tokens = cards.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var t in tokens)
				list.Add((Card)Enum.Parse(typeof(Card), t));
			return list;
		}


	}
}
