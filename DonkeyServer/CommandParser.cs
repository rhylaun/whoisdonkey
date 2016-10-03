using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Donkey.Common.Commands;
using Donkey.Common;

namespace Donkey.Server
{
    public static class CommandParser
    {
        private static readonly Dictionary<CommandType, Type> _commandTypeToType = new Dictionary<CommandType, Type>();
        private static readonly Dictionary<Type, CommandType> _typeToCommandType = new Dictionary<Type, CommandType>();
        private static readonly Dictionary<string, Type> _textToType = new Dictionary<string, Type>();

        public static void Init()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var t in types)
                {
                    var attrs = t.GetCustomAttributes(typeof(RegisterClientCommandAttribute), true);
                    if (attrs == null || attrs.Length == 0)
                        continue;
                    var comType = ((RegisterClientCommandAttribute)attrs[0]).CommandType;
                    _commandTypeToType[comType] = t;
                    _typeToCommandType[t] = comType;
                }

                foreach (var t in types)
                {
                    var attrs = t.GetCustomAttributes(typeof(ConsoleCommandInfoAttribute), true);
                    if (attrs == null || attrs.Length == 0)
                        continue;
                    var comType = ((ConsoleCommandInfoAttribute)attrs[0]).ConsoleLine;
                    _textToType[comType] = t;
                }
            }
        }

        public static ClientCommand Parse(string commandText)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new GameServerException("No command found");

            var tokens = commandText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0)
                throw new GameServerException("No command found");
            Type type;
            try
            {
                type = _textToType[tokens[0]];
            }
            catch (KeyNotFoundException)
            {
                throw new GameServerException("Unknown command");
            }
            var commandType = _typeToCommandType[type];
            return CommandCreator.Create(commandType, tokens.Skip(1).ToArray());
        }
    }
}
