using System;
using System.Collections.Generic;
using System.Linq;

namespace MatrixMaster
{
	class MainClass
	{
		static Matrix matrix;

		public static void Main(string[] args)
		{

			//Afin de faciliter la correction, j'ai hardcodé la matrice présente dans les notes de cours par défaut
			//Il suffit d'inverser les commentaires pour forcer l'entrée d'une matrice
			//Il est également possible de modifier la matrice en cours avec l'option [1]
			
			//matrix = new Matrix(new double[,] { { 2, 1, 3 }, { 1, -2, 1 }, { 1, 1, -2 } });
            matrix = new Matrix(new double[,] { { 4, -1, 0 }, { -1, 4, -1 }, { 0, -1, 4 } });

            string input = "";

			do
			{
				ShowActualMatrixParameters();
				Header("Menu principal");
				Choice("1", "Modifier la matrice en cours");
				Choice("2", "Opérations sur la matrice");
				Choice("3", "Résoudre un système d'équations");
				Choice("Q", "Quitter");

				input = Console.ReadKey().KeyChar.ToString().ToUpper();

				switch (input)
				{
					case "1": MenuSetActualMatrix(); break;
					case "2": MenuMatrixOperations(); break;
					case "3": MenuMatrixEquations(); break;
				}


			} while (input != "Q");

		}

		public static void ShowActualMatrixParameters()
		{
			Header("Paramètres de la matrice en cours");
			Console.WriteLine("Matrice originale:\n" + matrix);
			Console.WriteLine("CoMatrice:\n" + GetCoMatrix());
			Console.WriteLine("Inverse:\n" + GetInverse());
			Console.WriteLine("Transposée:\n" + matrix.Transposed);
			//Pause();
			Console.WriteLine("Est carrée : " + matrix.isSquare);
			Console.WriteLine("Est régulière : " + matrix.isRegular);
			Console.WriteLine("Est triangulaire diagonale : " + GetTriangularDiagonal());
			Console.WriteLine("Est triangulaire suppérieure : " + GetTriangularUpper());
			Console.WriteLine("Est triangulaire inférieure : " + GetTriangularLower());
			Console.WriteLine("Est triangulaire suppérieure stricte : " + GetTriangularUpperStrict());
			Console.WriteLine("Est triangulaire inférieure stricte : " + GetTriangularLowerStrict());
			Console.WriteLine("Trace : " + matrix.Trace);
			Console.WriteLine("Déterminant : " + GetDeterminant());
		}

		public static string GetCoMatrix()
		{
			try
			{
				return matrix.CoMatrix.ToString();
			}
			catch(Exception e)
			{
				return e.Message;
			}
		}

		public static string GetInverse()
		{
			try
			{
				return matrix.Inverse.ToString();
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		public static string GetDeterminant()
		{
			try
			{
				return matrix.Determinant.ToString();
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		public static string GetTriangularDiagonal()
		{
			try
			{
				return matrix.isTriangular().ToString();
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		public static string GetTriangularUpper()
		{
			try
			{
				return matrix.isTriangular(MatrixTriangularType.Upper).ToString();
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		public static string GetTriangularLower()
		{
			try
			{
				return matrix.isTriangular(MatrixTriangularType.Lower).ToString();
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		public static string GetTriangularUpperStrict()
		{
			try
			{
				return matrix.isTriangular(MatrixTriangularType.Upper, MatrixTriangularMode.Strict).ToString();
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		public static string GetTriangularLowerStrict()
		{
			try
			{
				return matrix.isTriangular(MatrixTriangularType.Lower, MatrixTriangularMode.Strict).ToString();
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}



		public static void MenuSetActualMatrix()
		{
			Header("Entrez une matrice et appuyez de nouveau sur ENTER pour terminer");
			do
			{
				matrix = GetMatrixFromUser();
			} while (matrix == null);
			Footer();
		}

		public static void MenuMatrixOperations()
		{
			string input = "";

			do
			{
				Header("Opérations sur la matrice en cours");
				Choice("1", "Additionner une deuxième matrice");
				Choice("2", "Multiplier par une valeur scalaire");
				Choice("3", "Multiplier par une deuxième matrice");
				Choice("4", "Multiplier par plusieurs matrices");
				Choice("R", "Retour");

				input = Console.ReadKey().KeyChar.ToString().ToUpper();

				switch (input)
				{
					case "1": OperationAddMatrix(); 		break;
					case "2": OperationScalar(); 			break;
					case "3": OperationMultiplyMatrix();	break;
					case "4": OperationMultiplyMatrixs(); 	break;
				}


			} while (input != "R");
		}

		public static void OperationAddMatrix()
		{
			Header("Entrez la deuxième matrice");
			Console.WriteLine(matrix);
			Console.WriteLine("+");
			var matrix2 = GetMatrixFromUser();
			Console.WriteLine("=");
			Console.WriteLine(matrix.Add(matrix2));
			Footer();
			Pause();
		}

		public static void OperationScalar()
		{
			Header("Entrez la valeur scalaire");

			var input = Console.ReadLine().Trim();
			double value = 0;
			if (Double.TryParse(input, out value))
			{
				Console.WriteLine(matrix.Scalar(value));
			}
			else 
			{
				Console.WriteLine("Valeur entrée invalide (" + input + ")");
			}
			Footer();
			Pause();
		}

		public static void OperationMultiplyMatrix()
		{
			Header("Entrez la matrice à multiplier");
			Console.WriteLine(matrix);
			Console.WriteLine("*");
			var matrix2 = GetMatrixFromUser();
			Console.WriteLine("=");
			try
			{
				Console.WriteLine(matrix.Multiply(matrix2));
			}
			catch (ArgumentOutOfRangeException e)
			{
				Console.WriteLine(e.Message);
			}
			Footer();
			Pause();
		}

		public static void OperationMultiplyMatrixs()
		{
			int operations = 0;
			Header("Entrez les matrices à multiplier");
			Console.WriteLine(matrix);
			Console.WriteLine("*");
			var matrixs = GetMatrixsFromUser().ToArray();
			Console.WriteLine("=");

			try
			{
				Console.WriteLine(matrix.Multiply(out operations, matrixs));
				Console.WriteLine("Cela a nécessité " + operations + " opérations");
			}
			catch (ArgumentOutOfRangeException e)
			{
				Console.WriteLine(e.Message);
			}

			Footer();
			Pause();
		}


		public static void MenuMatrixEquations()
		{
			Header("Entrez la matrice contenant les valeurs d'équations [n, 1]");
			var values = GetMatrixFromUser();
			try
			{

				var system = new System(matrix, values);

				Console.WriteLine("Solution par Cramer: \n" + system.SolveByCramer());
				Console.WriteLine("Solution par Inversion: \n" + system.SolveByInversion());
				Console.WriteLine("Solution par Jacobi: \n" + system.SolveByJacobi(0.05).Last());
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			Footer();
			Pause();
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
			//Header("Ajoutez des matrices de façon naturelles");
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


		public static void Header(string title)
		{
			Console.WriteLine("\n*****************************************************************************");
			Console.WriteLine("* " + title);
			Console.WriteLine("*****************************************************************************");
		}

		public static void Choice(string letter, string title)
		{
			Console.WriteLine("[" + letter.ToUpper() + "] = " + title);
		}

		public static void Footer()
		{
			Console.WriteLine("*****************************************************************************\n\n");
		}

		public static void Pause()
		{
			Console.WriteLine("Appuyez sur une ENTER pour continuer...");
			Console.ReadLine();
		}
	}
}
