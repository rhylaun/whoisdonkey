using Donkey.Common.AI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Donkey.Server
{
	public class AIFactory
	{
		private readonly Dictionary<string, Type> _aiModules = new Dictionary<string, Type>();

		public void Init()
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (var assembly in assemblies)
			{
				var types = assembly.GetTypes();
				var aiInterface = typeof(IAIModule);
				foreach (var t in types)
				{
					if (!aiInterface.IsAssignableFrom(t))
						continue;
					if (t.IsInterface || t.IsAbstract)
						continue;

					var attrs = t.GetCustomAttributes(typeof(AIModuleAttribute), true);
					if (attrs == null || attrs.Length == 0)
						continue;
					var aiName = ((AIModuleAttribute)attrs[0]).Name;
					_aiModules.Add(aiName, t);
				}
			}
		}

		public IAIModule CreateInstance(string name)
		{
			if (!_aiModules.ContainsKey(name))
				throw new ArgumentException("No AI with name " + name + " were found");

			var aiType = _aiModules[name];
			var instance = Activator.CreateInstance(aiType);

			return (IAIModule)instance;
		}

		public List<string> GetNames()
		{
			return _aiModules.Keys.ToList();
		}
	}
}