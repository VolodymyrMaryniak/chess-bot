using System;
using System.Collections.Generic;

namespace Chess.Engine.Extensions
{
	internal static class DictionaryExtensions
	{
		public static Dictionary<TKey, TValue> CloneDictionaryCloningValues<TKey, TValue>(this Dictionary<TKey, TValue> original)
			where TKey : struct
			where TValue : ICloneable
		{
			var clone = new Dictionary<TKey, TValue>(original.Count, original.Comparer);
			foreach (var keyValuePair in original)
			{
				clone.Add(keyValuePair.Key, (TValue)keyValuePair.Value.Clone());
			}

			return clone;
		}

		public static Dictionary<TKey, TValue> CloneDictionary<TKey, TValue>(this Dictionary<TKey, TValue> original)
			where TKey : struct
			where TValue : struct
		{
			var clone = new Dictionary<TKey, TValue>(original.Count, original.Comparer);
			foreach (var keyValuePair in original)
			{
				clone.Add(keyValuePair.Key, keyValuePair.Value);
			}

			return clone;
		}
	}
}
