using System;
using System.Collections.Generic;
using System.Linq;

namespace MatrixMaster
{
	class MainClass
	{

		static List<Matrix> Matrixs = new List<Matrix>();

		public static void ShowMatrixs()
		{
			Header("Liste des matrices en mémoire");
			var counter = 1;
			foreach (var matrix in Matrixs)
			{
				Console.WriteLine("Matrice #" + counter++.ToString() + "\n" + matrix);
			}
			Footer();
		}

		public static void Main(string[] args)
		{




			Matrixs.AddRange(GetMatrixsFromUser());

			ShowMatrixs();

			//Console.WriteLine(GetMatrixFromUser());



			//var Matrix1 = new Matrix(new double[,] { { 2, 1, 3 }, { 1, -2, 1 }, { 1, 1, -2 } });
			//var Matrix2 = new Matrix(new double[,] { { 6 }, { 2 }, { 1 } });
			//System system = new System(Matrix1, Matrix2);
			////Console.WriteLine(system);
			//Console.WriteLine(system.SolveByInversion());
			//Console.WriteLine(system.SolveByCramer());
            Console.ReadLine();
			//string input = "";

			//do
			//{

			//	Header("Choississez un test à effectuer");
			//	Choice("1", "Additionner deux matrices");
			//	Choice("2", "Multiplier deux matrices");
			//	Choice("3", "Multiplier plusieurs matrices et obtenir le nombre d'opérations");
			//	Choice("4", "Calculer le déterminant d'une matrice");
			//	Choice("5", "Calculer la co-matrixe d'une matrice");
			//	Choice("6", "Inverser une matrice");
			//	Choice("7", "Calculer la trace d'une matrice");
			//	Choice("8", "Obtenir la transposée d'une matrice");
			//	Choice("9", "Vérifier si une matrice est triangulaire");
			//	Choice("Q", "Quitter");
			//	Footer();

			//	input = Console.ReadKey().KeyChar.ToString().ToUpper();

			//	switch (input)
			//	{
			//		case "1":		TestAdd();				Pause();	break;
			//		case "2":		TestMultiply();			Pause(); 	break;
			//		case "3": 		TestMultiplyArray(); 	Pause(); 	break;
			//		case "4": 		TestDeterminant(); 		Pause(); 	break;
			//		case "5": 		TestCoMatrix(); 		Pause(); 	break;
			//		case "6": 		TestInvert(); 			Pause(); 	break;
			//		case "7": 		TestTrace(); 			Pause(); 	break;	
			//		case "8": 		TestTransposed(); 		Pause(); 	break;
			//		case "9": 		TestTriangular(); 		Pause(); 	break;
			//	}

			//} while (input != "Q");
		}

		public static List<Double> GetValuesFromLine(string input)
		{
			List<Double> values = new List<Double>();
			foreach (var element in input.Split(' ').Where(x => !x.Equals("")).ToArray())
			{
				double value = 0;
				if (Double.TryParse(element, out value))
				{
					values.Add(value);
				}
				else
				{
					Console.WriteLine("L'entrée " + element + " n'est pas valide.  Elle a été remplacée par 0");
				}

			}
			return values;
		}

		public static Matrix GetMatrixFromUser()
		{
			//Header("Entrez la matrice en séparant les valeurs par une espace.  Appuyez sur ENTER pour une nouvelle rangée");

			//Matrix result = null;

			//var result = new List<double, double>();

			var result = new List<List<double>>();
			int columns;

			while(true)
			{

				var values = GetValuesFromLine(Console.ReadLine().Trim());
				columns = values.Count;

				if (columns == 0)
				{
					return null;
				}

				result.Add(values);

				while (true)
				{
					values = GetValuesFromLine(Console.ReadLine());

					if (values.Count == 0)
					{
						return new Matrix(result.To2DArray());
					}

					if (values.Count == columns)
					{
						result.Add(values);
					}
					else
					{
						Console.WriteLine("Nombre de valeurs invalide.  Vous devez entrer " + columns + " valeurs !");
					}
				}


			}
		}

		public static List<Matrix> GetMatrixsFromUser()
		{
			var result = new List<Matrix>();
			Header("Ajoutez des matrices de façon naturelles");
			AddMatrixCallback(ref result);
			Footer();
			return result;
		}

		private static void AddMatrixCallback(ref List<Matrix> matrixs)
		{
			Console.WriteLine("Ajout de la matrice #" + (matrixs.Count + 1).ToString() + " (Appuyez sur ENTER pour terminer)");
			var newMatrix = GetMatrixFromUser();
			if (newMatrix != null)
			{
				matrixs.Add(newMatrix);
				AddMatrixCallback(ref matrixs);
			}
		}


		public static void TestAdd()
		{
			Header("Addition de deux matrices");
			//Console.WriteLine(matrix1);
			var matrix1 = GetMatrixFromUser();
			Console.WriteLine("+");
			var matrix2 = GetMatrixFromUser();
			Console.WriteLine("=");
			Console.WriteLine(matrix1.Add(matrix2));
			Footer();
		}

		public static void TestInvert()
		{
			Header("Inverser une matrice");
			var matrix1 = GetMatrixFromUser();
			Console.WriteLine("Résultat:");
			Console.WriteLine(matrix1.Inverse);
			Footer();
		}

		public static void TestTrace()
		{
			var matrix1 = new Matrix(new double[,] { { 1, 5, 1, 1 }, { 4, 4, 4, 4 }, { 9, 10, 11, 12 }, { 6, 5, 0, 0 } });

			var result = matrix1.Trace;

			Header("Calculer la trace de cette matrice");
			Console.WriteLine(matrix1);
			Console.WriteLine("Résultat = " + result);
			Footer();
		}

		public static void TestTransposed()
		{
			var matrix1 = new Matrix(new double[,] { { 1, 5, 1, 1 }, { 4, 4, 4, 4 }, { 9, 10, 11, 12 }, { 6, 5, 0, 0 } });

			var result = matrix1.Transposed;

			Header("Obtenir la transposée de cette matrice");
			Console.WriteLine(matrix1);
			Console.WriteLine("Résultat:");
			Console.WriteLine(result);
			Footer();
		}

		public static void TestTriangular()
		{
			var matrix1 = new Matrix(new double[,] { { 0, 5, 1, 1 }, { 0, 0, 4, 4 }, { 0, 0, 0, 12 }, { 0, 0, 0, 0 } });

			var diagonal = matrix1.isTriangular( MatrixTriangularType.Diagonal, MatrixTriangularMode.NonStrict);
			var lowerStrict = matrix1.isTriangular(MatrixTriangularType.Lower, MatrixTriangularMode.Strict);
			var upper = matrix1.isTriangular(MatrixTriangularType.Upper, MatrixTriangularMode.NonStrict);

			Header("Vérifier si la matrice suivante est triangulaire");
			Console.WriteLine(matrix1);
			Console.WriteLine("Cette matrice est :");
			Console.WriteLine("Triangulaire Diagonale = " + diagonal);
			Console.WriteLine("Triangulaire inférieure stricte = " + lowerStrict);
			Console.WriteLine("Triangulaire suppérieure = " + upper);
			Footer();
		}

		public static void TestCoMatrix()
		{
			var matrix = new Matrix(new double[,] { { 1, 5, 1, 1 }, { 4, 4, 4, 4 }, { 9, 10, 11, 12 }, { 6, 5, 0, 0 } });
			var result = matrix.CoMatrix;

			Header("Calcul de la co-matrice de cette matrice");
			Console.WriteLine(matrix);
			Console.WriteLine("Résultat:");
			Console.WriteLine(result);
			Footer();
		}

		public static void TestDeterminant()
		{
			var matrix = new Matrix(new double[,] { { 1, 5, 1, 1 }, { 4, 4, 4, 4 }, { 9, 10, 11, 12 }, { 6, 5, 0, 0 } });
			var result = matrix.Determinant;

			Header("Calcul du déterminant de cette matrice");
			Console.WriteLine(matrix);
			Console.WriteLine("Résultat = " + result);
			Footer();
		}

		public static void TestMultiply()
		{
			var matrix1 = new Matrix(new double[,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 } });

			var matrix2 = new Matrix(new double[,] { { 1, 2 }, { 3, 4 }, { 5, 6 }, { 7, 8 } });

			var result = matrix1.Multiply(matrix2);

			Header("Test de multiplication simple");
			Console.WriteLine(matrix1);
			Console.WriteLine("Multipliée par");
			Console.WriteLine(matrix2);
			Console.WriteLine("=");
			Console.WriteLine(result);
			Footer();

		}

		public static void TestMultiplyArray()
		{
			int operations;

			var matrix1 = new Matrix(new double[,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 } });

			var matrix2 = new Matrix(new double[,] { { 1, 2 }, { 3, 4 }, { 5, 6 }, { 7, 8 } });

			var matrix3 = new Matrix(new double[,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 } });

			var matrix4 = new Matrix(new double[,] { { 1, 2 }, { 3, 4 }, { 5, 6 }, { 7, 8 } });

			var result = matrix1.Multiply(out operations, matrix2, matrix3, matrix4);

			Header("Test de multiplication en chaîne");
			Console.WriteLine(matrix1);
			Console.WriteLine("Multipliée par");
			Console.WriteLine(matrix2);
			Console.WriteLine("Multipliée par");
			Console.WriteLine(matrix3);
			Console.WriteLine("Multipliée par");
			Console.WriteLine(matrix4);
			Console.WriteLine("=");
			Console.WriteLine(result);
			Console.WriteLine("Ce calcul a nécessité " + operations + " opérations");
			Footer();
		}

		public static void Header(string title)
		{
			Console.WriteLine("\n*********************************************************");
			Console.WriteLine("* " + title);
			Console.WriteLine("*********************************************************");
		}

		public static void Choice(string letter, string title)
		{
			Console.WriteLine("[" + letter.ToUpper() + "] = " + title);
		}

		public static void Footer()
		{
			Console.WriteLine("*********************************************************\n\n");
		}

		public static void Pause()
		{
			Console.WriteLine("Appuyez sur une ENTER pour continuer...");
			Console.ReadLine();
		}
	}
}
