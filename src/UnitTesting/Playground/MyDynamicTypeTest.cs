using NUnit.Framework;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Dynamic.Decorator.UnitTesting.Playground
{
    [TestFixture]
	public class MyDynamicTypeTest
	{
		[Test]
        public void Build_A_Dynamic_Type()
		{

			/*
				public class MyDynamicType
				{
					private int m_number;

					public MyDynamicType() : this(42) {}
					public MyDynamicType(int initNumber)
					{
						m_number = initNumber;
					}

					public int Number
					{
						get { return m_number; }
						set { m_number = value; }
					}

					public int MyMethod(int multiplier)
					{
						return m_number * multiplier;
					}
				}
			 */

			AssemblyName aName = new AssemblyName("DynamicAssemblyExample");

			AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.RunAndSave);

			ModuleBuilder mb = ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");

			EmitCustomType1(mb, "EmittedCustomType1");

			EmitCustomType2(mb, "EmittedCustomType2");

			EmitCustomType3(mb, "EmittedCustomType3");

			EmitCustomType4(mb, "EmittedCustomType4");

			EmitCustomType5(mb, "EmittedCustomType5");

			ab.Save(aName.Name + ".dll");
		}


		public Type EmitCustomType1(ModuleBuilder moduleBuilder, string typeName)
		{
			TypeBuilder tb = moduleBuilder.DefineType(typeName, TypeAttributes.Public);

			return tb.CreateType();
		}

		public Type EmitCustomType2(ModuleBuilder moduleBuilder, string typeName)
		{
			TypeBuilder tb = moduleBuilder.DefineType(typeName, TypeAttributes.Public);

			// Define a default constructor that supplies a default value
			// for the private field. For parameter types, pass the empty
			// array of types or pass null.
			ConstructorBuilder ctor = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);

			ILGenerator ctorIL = ctor.GetILGenerator();
			ctorIL.Emit(OpCodes.Ldarg_0);
			ctorIL.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
			ctorIL.Emit(OpCodes.Ret);

			return tb.CreateType();
		}

		public Type EmitCustomType3(ModuleBuilder moduleBuilder, string typeName)
		{
			TypeBuilder tb = moduleBuilder.DefineType(typeName, TypeAttributes.Public);

			ConstructorBuilder ctor = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);

			ILGenerator ctorIL = ctor.GetILGenerator();
			ctorIL.Emit(OpCodes.Ldarg_0);
			ctorIL.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
			ctorIL.Emit(OpCodes.Ret);

			tb.DefineField("m_number", typeof(int), FieldAttributes.Private);

			return tb.CreateType();
		}

		public Type EmitCustomType4(ModuleBuilder moduleBuilder, string typeName)
		{
			TypeBuilder tb = moduleBuilder.DefineType(typeName, TypeAttributes.Public);

			FieldBuilder fbNumber = tb.DefineField("m_number", typeof(int), FieldAttributes.Private);

			Type[] ctorParameterTypes = new Type[] { typeof(int) };

			ConstructorBuilder ctor = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, ctorParameterTypes);

			ILGenerator ctorIL = ctor.GetILGenerator();
			ctorIL.Emit(OpCodes.Ldarg_0);
			ctorIL.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
			ctorIL.Emit(OpCodes.Ldarg_0);
			ctorIL.Emit(OpCodes.Ldarg_1);
			ctorIL.Emit(OpCodes.Stfld, fbNumber);
			ctorIL.Emit(OpCodes.Ret);

			return tb.CreateType();
		}

		public Type EmitCustomType5(ModuleBuilder moduleBuilder, string typeName)
		{
			TypeBuilder tb = moduleBuilder.DefineType(typeName, TypeAttributes.Public);

			FieldBuilder fbNumber = tb.DefineField("m_number", typeof(int), FieldAttributes.Private);

			Type[] ctorParameterTypes = new Type[] { typeof(int) };

			ConstructorBuilder ctor1 = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, ctorParameterTypes);

			ILGenerator ctorIL1 = ctor1.GetILGenerator();
			ctorIL1.Emit(OpCodes.Ldarg_0);
			ctorIL1.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
			ctorIL1.Emit(OpCodes.Ldarg_0);
			ctorIL1.Emit(OpCodes.Ldarg_1);
			ctorIL1.Emit(OpCodes.Stfld, fbNumber);
			ctorIL1.Emit(OpCodes.Ret);


			ConstructorBuilder ctor2 = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);

			ILGenerator ctorIL2 = ctor2.GetILGenerator();
			ctorIL2.Emit(OpCodes.Ldarg_0);
			ctorIL2.Emit(OpCodes.Ldc_I4_S, 23);
			ctorIL2.Emit(OpCodes.Call, ctor1);
			ctorIL1.Emit(OpCodes.Ret);

			return tb.CreateType();
		}
	}

	public class CustomType1
	{

	}

	public class CustomType2
	{
		public CustomType2()
		{

		}
	}

	public class CustomType3
	{
		private int m_number;

		public CustomType3()
		{

		}
	}

	public class CustomType4
	{
		private int m_number;

		public CustomType4(int value)
		{
			m_number = value;
		}
	}

	public class CustomType5
	{
		private int m_number;

		public CustomType5()
			: this(23)
		{ }

		public CustomType5(int value)
		{
			m_number = value;
		}
	}
}
