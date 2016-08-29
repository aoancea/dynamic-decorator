using System;

namespace Dynamic.Decorator.Core
{
    public class TypeDefinition
    {
        public string Name { get; set; }

        public ConstructorDefinition Constructor { get; set; }

        public MethodDefinition[] Methods { get; set; }
    }

    public class ConstructorDefinition
    {
        public MethodParameterDefinition[] Parameters { get; set; }
    }

    public class MethodDefinition
    {
        public string Name { get; set; }

        public Type ReturnType { get; set; }

        public MethodParameterDefinition[] Parameters { get; set; }
    }

    public class MethodParameterDefinition
    {
        public string Name { get; set; }

        public Type Type { get; set; }
    }
}