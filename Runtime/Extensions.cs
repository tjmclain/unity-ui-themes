using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
	public static bool TryGetValue<T>(this T[] array, Predicate<T> match, out T value)
	{
		int index = Array.FindIndex(array, match);
		if (index >= 0)
		{
			value = array[index];
			return true;
		}
		else
		{
			value = default;
			return false;
		}
	}
}