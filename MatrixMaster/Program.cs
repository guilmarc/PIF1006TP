using System;

namespace MatrixMaster
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var matrix = new Matrix(new double[,] { { 5, 7, 8 }, { 0, 0, 3 }, { 0, 0, 5 } } ) ;
			Console.Write(matrix);
		}
	}
}
