using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Reflection.Emit;

namespace Dynamic.Decorator.UnitTesting.Playground
{
	[TestClass]
	public class MyDynamicTypeTest
	{
		[TestMethod]
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

			EmitDynamicCustomType1(mb, "DynamicCustomType1");

			EmitDynamicCustomType2(mb, "DynamicCustomType2");

			EmitDynamicCustomType3(mb, "DynamicCustomType3");

			EmitDynamicCustomType4(mb, "DynamicCustomType4");

			CreateTypeWithConstructorThatCallsConstructorWithParameterAndAssignToPrivateField(mb, "CreateTypeWithConstructorThatCallsConstructorWithParameterAndAssignToPrivateField");

			ab.Save(aName.Name + ".dll");
		}


		public Type EmitDynamicCustomType1(ModuleBuilder moduleBuilder, string typeName)
		{
			TypeBuilder tb = moduleBuilder.DefineType(typeName, TypeAttributes.Public);

			Type t = tb.CreateType();

			return t;
		}

		public Type EmitDynamicCustomType2(ModuleBuilder moduleBuilder, string typeName)
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

			Type t = tb.CreateType();

			return t;
		}

		public Type EmitDynamicCustomType3(ModuleBuilder moduleBuilder, string typeName)
		{
			TypeBuilder tb = moduleBuilder.DefineType(typeName, TypeAttributes.Public);

			ConstructorBuilder ctor = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);

			ILGenerator ctorIL = ctor.GetILGenerator();
			ctorIL.Emit(OpCodes.Ldarg_0);
			ctorIL.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
			ctorIL.Emit(OpCodes.Ret);

			tb.DefineField("m_number", typeof(int), FieldAttributes.Private);

			Type t = tb.CreateType();

			return t;
		}

		public Type EmitDynamicCustomType4(ModuleBuilder moduleBuilder, string typeName)
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

			Type t = tb.CreateType();

			return t;
		}

		public Type CreateTypeWithConstructorThatCallsConstructorWithParameterAndAssignToPrivateField(ModuleBuilder moduleBuilder, string typeName)
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

			Type t = tb.CreateType();

			return t;
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

	public class MyDynamicType
	{
		private int m_number;

		public MyDynamicType()
			: this(23)
		{ }

		public MyDynamicType(int initNumber)
		{
			m_number = initNumber;
		}
	}
}
