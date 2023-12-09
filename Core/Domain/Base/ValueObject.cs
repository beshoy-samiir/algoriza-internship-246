using System.Runtime.CompilerServices;

namespace Core.Domain.Base
{
    [Serializable]
    public abstract class ValueObject : IComparable, IComparable<ValueObject>
    {
        private int? _cachedHashCode;

        protected abstract IEnumerable<IComparable> GetEqualityComponents();

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (GetUnproxiedType(this) != GetUnproxiedType(obj))
            {
                return false;
            }

            ValueObject valueObject = (ValueObject)obj;
            return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            if (!_cachedHashCode.HasValue)
            {
                _cachedHashCode = GetEqualityComponents().Aggregate(1, (int current, IComparable obj) => current * 23 + (obj?.GetHashCode() ?? 0));
            }

            return _cachedHashCode.Value;
        }

        public virtual int CompareTo(ValueObject other)
        {
            if ((object)other == null)
            {
                return 1;
            }

            if ((object)this == other)
            {
                return 0;
            }

            Type unproxiedType = GetUnproxiedType(this);
            Type unproxiedType2 = GetUnproxiedType(other);
            if (unproxiedType != unproxiedType2)
            {
                DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
                defaultInterpolatedStringHandler.AppendFormatted(unproxiedType);
                string strA = defaultInterpolatedStringHandler.ToStringAndClear();
                defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
                defaultInterpolatedStringHandler.AppendFormatted(unproxiedType2);
                return string.Compare(strA, defaultInterpolatedStringHandler.ToStringAndClear(), StringComparison.Ordinal);
            }

            return GetEqualityComponents().Zip(other.GetEqualityComponents(), (IComparable left, IComparable right) => left?.CompareTo(right) ?? ((right != null) ? (-1) : 0)).FirstOrDefault((int cmp) => cmp != 0);
        }

        public virtual int CompareTo(object other)
        {
            return CompareTo(other as ValueObject);
        }

        public static bool operator ==(ValueObject a, ValueObject b)
        {
            if ((object)a == null && (object)b == null)
            {
                return true;
            }

            if ((object)a == null || (object)b == null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject a, ValueObject b)
        {
            return !(a == b);
        }

        internal static Type GetUnproxiedType(object obj)
        {
            Type type = obj.GetType();
            string text = type.ToString();
            if (text.Contains("Castle.Proxies.") || text.EndsWith("Proxy"))
            {
                return type.BaseType;
            }

            return type;
        }
    }
}
