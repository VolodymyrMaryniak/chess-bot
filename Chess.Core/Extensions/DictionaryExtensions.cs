using System;
using System.Collections.Generic;

namespace Chess.Core.Extensions
{
	internal static class DictionaryExtensions
	{
		public static Dictionary<TKey, TValue> CloneDictionaryCloningValues<TKey, TValue>(this Dictionary<TKey, TValue> original)
			where TKey : struct
			where TValue : ICloneable
		{
			var clone = new Dictionary<TKey, TValue>(original.Count, original.Comparer);
			foreach (var (key, value) in original)
			{
				clone.Add(key, (TValue) value.Clone());
			}

			return clone;
		}

		public static Dictionary<TKey, TValue> CloneDictionary<TKey, TValue>(this Dictionary<TKey, TValue> original)
			where TKey : struct
			where TValue : struct
		{
			var clone = new Dictionary<TKey, TValue>(original.Count, original.Comparer);
			foreach (var (key, value) in original)
			{
				clone.Add(key, value);
			}

			return clone;
		}
	}
}
