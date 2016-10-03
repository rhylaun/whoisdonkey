using System;

namespace Donkey.Common
{
	[Serializable]
	public class AuthData
	{
		private readonly string _login;
		private readonly string _password;

		public string Login { get { return _login; } }
		public string Password { get { return _password; } }

		public AuthData(string login, string password)
		{
			_login = login;
			_password = password;
			if (string.IsNullOrEmpty(_password)) _password = string.Empty;
		}

		public override bool Equals(object obj)
		{
			if (obj is AuthData)
			{
				var authData = obj as AuthData;
				return this.Login == authData.Login && this.Password == authData.Password;
			}

			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return Login.GetHashCode() + Password.GetHashCode();
		}
	}
}
