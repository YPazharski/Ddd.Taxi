using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Ddd.Infrastructure
{
	/// <summary>
	/// Базовый класс для всех Value типов.
	/// </summary>
	public class ValueType<T>
	{
		static Type ObjType { get; }
		static PropertyInfo[] ObjProps { get; }
		static ValueType()
		{
            ObjType = typeof(T);
			ObjProps = ObjType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
			if (obj.GetType() != ObjType) return false;
			return Equals((T)obj);
        }

		public bool Equals(T other)
		{
            if (other == null) return false;
            foreach (var prop in ObjProps)
			{
				var thisPropValue = prop.GetValue(this);
				var otherPropValue = prop.GetValue(other);
				if (thisPropValue == null || otherPropValue == null)
					return thisPropValue == null && otherPropValue == null ?
						true
						: false;
				if (!thisPropValue.Equals(otherPropValue)) 
					return false;
			}
			return true;
		}
    }
}