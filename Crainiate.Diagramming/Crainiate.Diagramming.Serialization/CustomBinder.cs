// (c) Copyright Crainiate Software 2010




using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.Serialization;

//Performs the mapping to types declared in this assembly. It accumulates all types defined in the assembly
//this class is defined in. Optionally, an assembly can be passed as an argument.
namespace Crainiate.Diagramming.Serialization
{
    internal class CustomBinder: SerializationBinder
    {
        private static Dictionary<string, Type> _typeList; //The list that holds the types and type names contained in the assembly.

        //Constructor
        public CustomBinder()
        {
            _typeList = LoadDomainTypes();
        }

        //Binds the passed typename to the type contained in the dictionary.
        public override Type BindToType(string assemblyName, string typeName)
        {
            //Added to just return an object type instead
            if (typeName == null) return typeof(object);
            Dictionary<string, Type> typeList = _typeList;

            if (typeList.ContainsKey(typeName))
            {
                return typeList[typeName];
            }
            else
            {
                Type type = Type.GetType(typeName);
                if (type != null)
                {
                    return type;
                }
                else
                {
                    return typeof(object);
                }
            }
        }

        //Loads the types from the passed assembly in the list. The key of the list is the full name of the type.
        private static Dictionary<string, Type> LoadDomainTypes()
        {
            Dictionary<string, Type> typeList = new Dictionary<string, Type>();

            //Try load from assembly path or resource			
            //Make an array for the list of assemblies.
            Assembly[] assemblyArray = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblyArray)
            {
                if (!assembly.FullName.StartsWith("mscorlib") && !assembly.FullName.StartsWith("System"))
                {
                     foreach (Type type in assembly.GetTypes())
                     {
                        typeList.Add(type.FullName, type);
                     }
                }
            }
            return typeList;
        }
    }
}
