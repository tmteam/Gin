﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TheGin
{
    /// <summary>
    /// Command sketch library implementation with auto-assembly-scan ability
    /// </summary>
    public class CommandScanner : ICommandLibrary
    {
        Dictionary<string, CommandSketch> commands 
            = new Dictionary<string, CommandSketch>();

        public CommandSketch GetOrNull(string name)
        {
            name = name.ToLower();
            if (this.commands.ContainsKey(name))
            {
                return this.commands[name];
            }
            return null;
        }

        public void Registrate<T>() where T: ICommand, new()
        {
            this.Registrate(typeof(T));
        }
       
        public void Registrate(ICommand exemplar)
        {
            var sketch = new CommandSketch(
                attribute:      ReflectionTools.GetCommandAttributeOrThrow(exemplar.GetType()),
                commandType:    exemplar.GetType(),
                locator:        ()=>exemplar
            );
            this.Registrate(sketch);
        }

        public void Registrate(Type type) {
            ReflectionTools.ThrowIfItIsNotValidCommand(type);
            var cmdAttribute = ReflectionTools.GetCommandAttributeOrThrow(type);
            this.Registrate(type, cmdAttribute);
        }

        void Registrate(Type type, CommandAttribute attribute)
        {
            ReflectionTools.ThrowIfItIsNotValidCommand(type);
            RegistrateUnsafe(type, attribute);
        }
        void RegistrateUnsafe(Type type, CommandAttribute attribute)
        {
            var sketch = new CommandSketch(attribute, type, () => (ICommand)Activator.CreateInstance(type));
            this.Registrate(sketch);
        }
        void Registrate(CommandSketch sketch)
        {
            string key = ParseTools.GetCommandName(sketch.CommandType).ToLower();
            this.commands.Add(key, sketch);
        }
        /// <summary>
        /// Looks up specified assembly for suitable command types and registrates all of them
        /// </summary>
        /// <param name="assembly"></param>
        public void ScanAssembly(Assembly assembly)
        {
            foreach (var type  in assembly.ExportedTypes.Where(t=>typeof(ICommand).IsAssignableFrom(t)))
            {
                var customAttribute = type.GetCustomAttribute<CommandAttribute>();
                if (customAttribute != null) {
                    if (ReflectionTools.IsItValidCommand(type))
                        this.RegistrateUnsafe(type, customAttribute);
                }
            }
        }


        public IEnumerable<CommandSketch> Sketches {
            get { return this.commands.Values; }
        }


        public void Deregistrate(string commandName) {
            commandName = commandName.ToLower();
            commands.Remove(commandName);
        }
        
    }
}
