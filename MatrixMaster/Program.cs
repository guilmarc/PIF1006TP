using System;

namespace MatrixMaster
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var matrix = new Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 } });
			var matrix2 = new Matrix(new double[,] { { 9 }, { 8 }, { 7 } });

			var matrix3 = new Matrix(new double[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } }); 

			//Console.WriteLine(matrix.isSquare);
			//Console.WriteLine(matrix.Trace);

			Console.WriteLine(matrix.Multiply(matrix2));

			//Console.WriteLine(matrix2);
		}
	}
}
