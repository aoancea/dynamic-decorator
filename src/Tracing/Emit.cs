using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Dynamic.Decorator.Tracing
{
    public static class Emit
    {
        /*
        public class Foo_Time_Decorator : IFoo
        {
            private readonly IFoo decorated;
            private readonly ILogger logger;

            public Foo_Time_Decorator(IFoo decorated, ILogger logger)
            {
                this.decorated = decorated;
                this.logger = logger;
            }

            public string Bar(string text)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                string result = decorated.Bar(text);

                sw.Stop();

                logger.Log(sw);

                return result;
            }
        }
        */


        public static Type EmitTimeLogger<TDecorated>()
        {
            AssemblyName aName = new AssemblyName("EmitTimeLogger");

            AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.RunAndSave, AppDomain.CurrentDomain.BaseDirectory);

            ModuleBuilder mb = ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");

            Type loggerType = typeof(ILogger);

            Type type = EmitCustomType(mb, typeof(TDecorated), "Foo_Time_Decorator", loggerType);

            ab.Save(aName.Name + ".dll");

            return type;
        }



        private static Type EmitCustomType(ModuleBuilder moduleBuilder, Type decoratedType, string typeName, Type loggerType)
        {
            TypeBuilder tb = moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.BeforeFieldInit);
            tb.AddInterfaceImplementation(decoratedType);

            FieldBuilder fbDecorated = tb.DefineField(TypeFieldName(decoratedType), decoratedType, FieldAttributes.Public); // change to Private
            FieldBuilder fbLogger = tb.DefineField(TypeFieldName(loggerType), loggerType, FieldAttributes.Public); // change to Private

            Type[] ctorParameterTypes = new Type[] { decoratedType, loggerType };

            ConstructorBuilder ctor = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, ctorParameterTypes);

            ILGenerator ctorIL = ctor.GetILGenerator();
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Ldarg_1);
            ctorIL.Emit(OpCodes.Stfld, fbDecorated);
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Ldarg_2);
            ctorIL.Emit(OpCodes.Stfld, fbLogger);
            ctorIL.Emit(OpCodes.Ret);


            string implementedMethodName = "Bar";

            MethodBuilder mbMyMethod = tb.DefineMethod(implementedMethodName, MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final, typeof(string), new Type[] { typeof(string) });

            ILGenerator myMethodGetIL = mbMyMethod.GetILGenerator();
            myMethodGetIL.Emit(OpCodes.Ldarg_1);
            myMethodGetIL.Emit(OpCodes.Ret);

            tb.DefineMethodOverride(mbMyMethod, decoratedType.GetMethod(implementedMethodName));


            //PropertyBuilder pbNumber = tb.DefineProperty("Number", PropertyAttributes.HasDefault, typeof(int), null);

            //MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            //MethodBuilder mbNumberGetAccessor = tb.DefineMethod("get_Number", getSetAttr, typeof(int), null);

            //ILGenerator numberGetIL = mbNumberGetAccessor.GetILGenerator();
            //numberGetIL.Emit(OpCodes.Ldarg_0);
            //numberGetIL.Emit(OpCodes.Ldfld, fbNumber);
            //numberGetIL.Emit(OpCodes.Ret);

            //MethodBuilder mbNumberSetAccessor = tb.DefineMethod("set_Number", getSetAttr, typeof(void), new Type[] { typeof(int) });

            //ILGenerator numberSetIL = mbNumberSetAccessor.GetILGenerator();
            //numberSetIL.Emit(OpCodes.Ldarg_0);
            //numberSetIL.Emit(OpCodes.Ldarg_1);
            //numberSetIL.Emit(OpCodes.Stfld, fbNumber);
            //numberSetIL.Emit(OpCodes.Ret);

            //pbNumber.SetGetMethod(mbNumberGetAccessor);
            //pbNumber.SetSetMethod(mbNumberSetAccessor);

            return tb.CreateType();
        }

        private static string TypeFieldName(Type type)
        {
            if (type.IsInterface)
            {
                string typeName = type.Name;

                return string.Concat(typeName.ToLowerInvariant()[1], typeName.Substring(2));
            }

            throw new NotSupportedException("Dynamic.Decorator currently supports interface implementation only. Class extensions are not supported!");
        }
    }
}