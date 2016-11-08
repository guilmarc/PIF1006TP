using System;

namespace MatrixMaster
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var matrix = new Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 } });

			var matrix2 = new Matrix(new double[,] { { 9 }, { 8 }, { 7 } });

			var matrix3 = new Matrix(new double[,] { { 6, 5, 5 }, { 4, 5, 6 }, { 7, 8, 9 } });

            var matrix4 = new Matrix(new double[,] { { 1, 5, 1, 1 }, { 4, 4, 4, 4 }, { 9, 10, 11, 12 }, {6, 5, 0, 0} });

            //Console.WriteLine(matrix.isSquare);
            //Console.WriteLine(matrix.Trace);

            Console.WriteLine( matrix3.Inverse);

			//Console.WriteLine(matrix2);
		    Console.ReadLine();
		}
	}
}
