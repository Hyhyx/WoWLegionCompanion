﻿using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Utilities
{
	internal static class ConvertUtils
	{
		private static Func<object, object> CreateCastConverter(ConvertUtils.TypeConvertKey t)
		{
			MethodInfo method = t.TargetType.GetMethod("op_Implicit", new Type[]
			{
				t.InitialType
			});
			if (method == null)
			{
				method = t.TargetType.GetMethod("op_Explicit", new Type[]
				{
					t.InitialType
				});
			}
			if (method == null)
			{
				return null;
			}
			MethodCall<object, object> call = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
			return (object o) => call(null, new object[]
			{
				o
			});
		}

		public static bool CanConvertType(Type initialType, Type targetType, bool allowTypeNameToString)
		{
			ValidationUtils.ArgumentNotNull(initialType, "initialType");
			ValidationUtils.ArgumentNotNull(targetType, "targetType");
			if (ReflectionUtils.IsNullableType(targetType))
			{
				targetType = Nullable.GetUnderlyingType(targetType);
			}
			if (targetType == initialType)
			{
				return true;
			}
			if (typeof(IConvertible).IsAssignableFrom(initialType) && typeof(IConvertible).IsAssignableFrom(targetType))
			{
				return true;
			}
			if (initialType == typeof(DateTime) && targetType == typeof(DateTimeOffset))
			{
				return true;
			}
			if (initialType == typeof(Guid) && (targetType == typeof(Guid) || targetType == typeof(string)))
			{
				return true;
			}
			if (initialType == typeof(Type) && targetType == typeof(string))
			{
				return true;
			}
			TypeConverter converter = ConvertUtils.GetConverter(initialType);
			if (converter != null && !ConvertUtils.IsComponentConverter(converter) && converter.CanConvertTo(targetType) && (allowTypeNameToString || converter.GetType() != typeof(TypeConverter)))
			{
				return true;
			}
			TypeConverter converter2 = ConvertUtils.GetConverter(targetType);
			return (converter2 != null && !ConvertUtils.IsComponentConverter(converter2) && converter2.CanConvertFrom(initialType)) || (initialType == typeof(DBNull) && ReflectionUtils.IsNullable(targetType));
		}

		private static bool IsComponentConverter(TypeConverter converter)
		{
			return converter is ComponentConverter;
		}

		public static T Convert<T>(object initialValue)
		{
			return ConvertUtils.Convert<T>(initialValue, CultureInfo.CurrentCulture);
		}

		public static T Convert<T>(object initialValue, CultureInfo culture)
		{
			return (T)((object)ConvertUtils.Convert(initialValue, culture, typeof(T)));
		}

		public static object Convert(object initialValue, CultureInfo culture, Type targetType)
		{
			if (initialValue == null)
			{
				throw new ArgumentNullException("initialValue");
			}
			if (ReflectionUtils.IsNullableType(targetType))
			{
				targetType = Nullable.GetUnderlyingType(targetType);
			}
			Type type = initialValue.GetType();
			if (targetType == type)
			{
				return initialValue;
			}
			if (initialValue is string && typeof(Type).IsAssignableFrom(targetType))
			{
				return Type.GetType((string)initialValue, true);
			}
			if (targetType.IsInterface || targetType.IsGenericTypeDefinition || targetType.IsAbstract)
			{
				throw new ArgumentException("Target type {0} is not a value type or a non-abstract class.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					targetType
				}), "targetType");
			}
			if (initialValue is IConvertible && typeof(IConvertible).IsAssignableFrom(targetType))
			{
				if (targetType.IsEnum)
				{
					if (initialValue is string)
					{
						return Enum.Parse(targetType, initialValue.ToString(), true);
					}
					if (ConvertUtils.IsInteger(initialValue))
					{
						return Enum.ToObject(targetType, initialValue);
					}
				}
				return System.Convert.ChangeType(initialValue, targetType, culture);
			}
			if (initialValue is DateTime && targetType == typeof(DateTimeOffset))
			{
				return new DateTimeOffset((DateTime)initialValue);
			}
			if (initialValue is string)
			{
				if (targetType == typeof(Guid))
				{
					return new Guid((string)initialValue);
				}
				if (targetType == typeof(Uri))
				{
					return new Uri((string)initialValue);
				}
				if (targetType == typeof(TimeSpan))
				{
					return TimeSpan.Parse((string)initialValue);
				}
			}
			TypeConverter converter = ConvertUtils.GetConverter(type);
			if (converter != null && converter.CanConvertTo(targetType))
			{
				return converter.ConvertTo(null, culture, initialValue, targetType);
			}
			TypeConverter converter2 = ConvertUtils.GetConverter(targetType);
			if (converter2 != null && converter2.CanConvertFrom(type))
			{
				return converter2.ConvertFrom(null, culture, initialValue);
			}
			if (initialValue != DBNull.Value)
			{
				throw new Exception("Can not convert from {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					type,
					targetType
				}));
			}
			if (ReflectionUtils.IsNullable(targetType))
			{
				return ConvertUtils.EnsureTypeAssignable(null, type, targetType);
			}
			throw new Exception("Can not convert null {0} into non-nullable {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				type,
				targetType
			}));
		}

		public static bool TryConvert<T>(object initialValue, out T convertedValue)
		{
			return ConvertUtils.TryConvert<T>(initialValue, CultureInfo.CurrentCulture, out convertedValue);
		}

		public static bool TryConvert<T>(object initialValue, CultureInfo culture, out T convertedValue)
		{
			return MiscellaneousUtils.TryAction<T>(delegate
			{
				object obj;
				ConvertUtils.TryConvert(initialValue, CultureInfo.CurrentCulture, typeof(T), out obj);
				return (T)((object)obj);
			}, out convertedValue);
		}

		public static bool TryConvert(object initialValue, CultureInfo culture, Type targetType, out object convertedValue)
		{
			return MiscellaneousUtils.TryAction<object>(() => ConvertUtils.Convert(initialValue, culture, targetType), out convertedValue);
		}

		public static T ConvertOrCast<T>(object initialValue)
		{
			return ConvertUtils.ConvertOrCast<T>(initialValue, CultureInfo.CurrentCulture);
		}

		public static T ConvertOrCast<T>(object initialValue, CultureInfo culture)
		{
			return (T)((object)ConvertUtils.ConvertOrCast(initialValue, culture, typeof(T)));
		}

		public static object ConvertOrCast(object initialValue, CultureInfo culture, Type targetType)
		{
			if (targetType == typeof(object))
			{
				return initialValue;
			}
			if (initialValue == null && ReflectionUtils.IsNullable(targetType))
			{
				return null;
			}
			object result;
			if (ConvertUtils.TryConvert(initialValue, culture, targetType, out result))
			{
				return result;
			}
			return ConvertUtils.EnsureTypeAssignable(initialValue, ReflectionUtils.GetObjectType(initialValue), targetType);
		}

		public static bool TryConvertOrCast<T>(object initialValue, out T convertedValue)
		{
			return ConvertUtils.TryConvertOrCast<T>(initialValue, CultureInfo.CurrentCulture, out convertedValue);
		}

		public static bool TryConvertOrCast<T>(object initialValue, CultureInfo culture, out T convertedValue)
		{
			return MiscellaneousUtils.TryAction<T>(delegate
			{
				object obj;
				ConvertUtils.TryConvertOrCast(initialValue, CultureInfo.CurrentCulture, typeof(T), out obj);
				return (T)((object)obj);
			}, out convertedValue);
		}

		public static bool TryConvertOrCast(object initialValue, CultureInfo culture, Type targetType, out object convertedValue)
		{
			return MiscellaneousUtils.TryAction<object>(() => ConvertUtils.ConvertOrCast(initialValue, culture, targetType), out convertedValue);
		}

		private static object EnsureTypeAssignable(object value, Type initialType, Type targetType)
		{
			Type type = (value == null) ? null : value.GetType();
			if (value != null)
			{
				if (targetType.IsAssignableFrom(type))
				{
					return value;
				}
				Func<object, object> func = ConvertUtils.CastConverters.Get(new ConvertUtils.TypeConvertKey(type, targetType));
				if (func != null)
				{
					return func(value);
				}
			}
			else if (ReflectionUtils.IsNullable(targetType))
			{
				return null;
			}
			throw new Exception("Could not cast or convert from {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				(initialType == null) ? "{null}" : initialType.ToString(),
				targetType
			}));
		}

		internal static TypeConverter GetConverter(Type t)
		{
			return JsonTypeReflector.GetTypeConverter(t);
		}

		public static bool IsInteger(object value)
		{
			switch (System.Convert.GetTypeCode(value))
			{
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
				return true;
			default:
				return false;
			}
		}

		static ConvertUtils()
		{
			// Note: this type is marked as 'beforefieldinit'.
			if (ConvertUtils.<>f__mg$cache0 == null)
			{
				ConvertUtils.<>f__mg$cache0 = new Func<ConvertUtils.TypeConvertKey, Func<object, object>>(ConvertUtils.CreateCastConverter);
			}
			ConvertUtils.CastConverters = new ThreadSafeStore<ConvertUtils.TypeConvertKey, Func<object, object>>(ConvertUtils.<>f__mg$cache0);
		}

		private static readonly ThreadSafeStore<ConvertUtils.TypeConvertKey, Func<object, object>> CastConverters;

		[CompilerGenerated]
		private static Func<ConvertUtils.TypeConvertKey, Func<object, object>> <>f__mg$cache0;

		internal struct TypeConvertKey : IEquatable<ConvertUtils.TypeConvertKey>
		{
			public TypeConvertKey(Type initialType, Type targetType)
			{
				this._initialType = initialType;
				this._targetType = targetType;
			}

			public Type InitialType
			{
				get
				{
					return this._initialType;
				}
			}

			public Type TargetType
			{
				get
				{
					return this._targetType;
				}
			}

			public override int GetHashCode()
			{
				return this._initialType.GetHashCode() ^ this._targetType.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				return obj is ConvertUtils.TypeConvertKey && this.Equals((ConvertUtils.TypeConvertKey)obj);
			}

			public bool Equals(ConvertUtils.TypeConvertKey other)
			{
				return this._initialType == other._initialType && this._targetType == other._targetType;
			}

			private readonly Type _initialType;

			private readonly Type _targetType;
		}
	}
}
