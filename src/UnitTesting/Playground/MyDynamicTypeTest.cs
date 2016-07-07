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









			ab.Save(aName.Name + ".dll");
		}
	}
}
