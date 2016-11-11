using System;
using System.Collections.Generic;
using System.Linq;

namespace MatrixMaster
{
public static class ExtensionMethods
	{
		public static T[,] To2DArray<T>(this IEnumerable<IEnumerable<T>> source)
		{
			var jaggedArray = source.Select(r => r.ToArray()).ToArray();
			int rows = jaggedArray.GetLength(0);
			int columns = jaggedArray.Max(r => r.Length);
			var array = new T[rows, columns];
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < jaggedArray[i].Length; j++)
				{
					array[i, j] = jaggedArray[i][j];
				}
			}
			return array;
		}
	}
}
