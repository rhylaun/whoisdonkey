using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Donkey.Common.Commands;
using Donkey.Common;

namespace Donkey.Server.CommandProcessors
{
    public class CommandProcessorFactory
    {
        private static readonly Dictionary<CommandType, Type> _commandToProccesor = new Dictionary<CommandType, Type>();

        public static void Init()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var t in types)
                {
                    var attrs = t.GetCustomAttributes(typeof(CommandProcessorInfoAttribute), true);
                    if (attrs == null || attrs.Length == 0)
                        continue;
                    var comType = ((CommandProcessorInfoAttribute)attrs[0]).CommandType;
                    _commandToProccesor[comType] = t;
                }
            }
        }

        public static BaseCommandProcessor GetProcessor(ClientCommand command)
        {
            if (command == null)
                throw new GameServerException("Wrong argument: command == null");

            if(!_commandToProccesor.ContainsKey(command.CommandType))
                throw new GameServerException("Подходящий обработчик не найден.");

            var processorType = _commandToProccesor[command.CommandType];
            var obj = processorType.GetConstructor(new[] { typeof(ClientCommand) }).Invoke(new[] { command });

            return (BaseCommandProcessor)obj;
        }
    }
}
