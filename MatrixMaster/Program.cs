using System;

namespace MatrixMaster
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var matrix = new Matrix(new double[,] { { 5, 7, 9 }, { 0, 1, 4 }, { 5, 8, 3 } });
			var matrix2 = new Matrix(new double[,] { { 5, 7, 9 }, { 0, 1, 4 } });

			Console.WriteLine(matrix.isSquare);
			Console.WriteLine(matrix.Trace);

			Console.WriteLine(matrix.Transposed);
		}
	}
}
