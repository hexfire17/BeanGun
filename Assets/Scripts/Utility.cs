using UnityEngine;
using System.Collections;

public static class Utility
{
	public static T[] FisherShuffle<T> (T[] arr, int seed)
	{
		System.Random random = new System.Random (seed);
		for (int i = 0; i < arr.Length-1; i++)
		{
			int randIndex = random.Next(i, arr.Length);
			T item = arr[i];
			arr[i] = arr[randIndex];
			arr[randIndex] = item;
		}
		
		return arr;
	}
}
