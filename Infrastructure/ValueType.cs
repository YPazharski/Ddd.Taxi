using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
			if (ObjProps.Length == 0)
				return ObjType.Name;
            var result = new StringBuilder(ObjType.Name);
			result.Append("(");
			foreach (var prop in ObjProps.OrderBy(p => p.Name))
			{
				result.Append(prop.Name);
				result.Append(": ");
				var propValue = prop.GetValue(this);
				if (propValue != null)
					result.Append(propValue.ToString());
				result.Append("; ");
			}
			result.Remove(result.Length - 2, 2);
			result.Append(")");
			return result.ToString();
        }
    }
}