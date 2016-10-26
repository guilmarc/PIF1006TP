using System;
using System.Linq;

namespace MatrixMaster
{

	public enum MatrixTriangularType
	{
		Any = 0,
		Lower,
		Upper
	}

	public enum MatrixTriangularMode
	{
		NonStrict = 0,
		Strict
	}

	public class Matrix 
	{
		

		double[,] _matrix;      // tableau à 2 dimensions de nombre décimaux, qui stocke la matrice.

		//Constructeurs

		public Matrix() {}
		public Matrix(double[,] matrix)
		{
			this._matrix = matrix;
		}

		/* Pour l’accès et l’édition de la matrice, vous pouvez également ajouter une façon de pouvoir
		accéder ou modifier un élément quelconque de la matrice d’une façon de votre choix, par
		exemple :
		- En utilisant une propriété pour accéder au tableau contenant les éléments, comme :
			- double[,] Elements
		- En utilisant des méthodes permettant d’obtenir ou de modifier un élément particulier
		du tableau, comme :
			- double GetElement(int ligne, int colonne) ta tête
			- void SetElement(int ligne, int colonne, double valeur) */
		public double this[int row, int column]
		{
			get
			{
				return this._matrix[row, column];
			}

			set
			{
				this._matrix[row, column] = value;
			}
		}

		//TODO: Quel est le nom anglais de trace de la matrice... Layout ?
		public double Trace	//calcule et retourne la trace de la matrice, si la matrice est carrée ;
		{
			get
			{
				if (this.isSquare)
				{
					double result = 0;

					for (int i = 0; i < _matrix.GetLength(0); i++)
					{
						for (int j = 0; j < _matrix.GetLength(1); j++)
						{
							if (i == j) result += _matrix[i, j];
						}
					}

					return result;
				}
				else 
				{
					return 0;
				}
			}
		}

		public double Determinant //calcule et retourne le déterminant de la matrice, si la matrice est carrée ;
		{
			get
			{
				return 0;
			}
		}

		public Matrix Transposed //calcule et retourne la transposée de la matrice, si la matrice est carrée ;
		{
			get
			{
				var result = new Matrix(new double[_matrix.GetLength(1), _matrix.GetLength(0)]);

				for (int i = 0; i < _matrix.GetLength(0); i++ )
				{
					for (int j = 0; j < _matrix.GetLength(1); j++)
					{
						result[j, i] = _matrix[i, j];
					}
				}

				return result;
			}
		}


		public Matrix CoMatrix //calcule et retourne la comatrice de la matrice, si la matrice est carrée
		{
			get
			{
				return null;
			}
		}

		public Matrix Inverse //calcule et retourne la matrice inverse, si la matrice est carrée
		{
			get
			{
				//Avant de tout calculer, assurez-vous que la matrice est régulière;

				//Si la matrice est, en plus, triangulaire, calculez directement la matrice inverse en utilisant le schème (voir énoncé)
				return null;
			}
		}

		public bool isSquare //retourne vrai ou faux selon si la matrice est carrée ou non
		{
			get
			{
				return _matrix.GetLength(0) == _matrix.GetLength(1);
			}
		}

		public bool isRegular //retourne vrai si la matrice est régulière(déterminant non nul) ou faux si la matrice est singulière(déterminant nul)
		{
			get
			{
				//TODO: Ajouter cette difficulté au rapport de projet.
				return this.Determinant == 0 ? true : false ;
			}
		}

		public int GetLength(int dimension)
		{
			return _matrix.GetLength(dimension);
		}

		//public static Matrix operator +(Matrix m1, Matrix m2)
		//{
		//	var result = new Matrix(new double[_matrix.GetLength(1), _matrix.GetLength(0)]);

		//	for (int i = 0; i < _matrix.GetLength(0); i++)
		//	{
		//		for (int j = 0; j < _matrix.GetLength(1); j++)
		//		{
		//			_matrix[i, j] += matrix[i, j];
		//		}
		//	}

		//	return result;
		//}


		//l’addition d’une matrice, qui retourne une matrice;
		public Matrix Add(Matrix matrix)
		{

			if (!this.isSameDimention(matrix))
			{
				throw new ArgumentOutOfRangeException("Inconsistent matrix dimensions");
			}

			var result = new Matrix(new double[_matrix.GetLength(1), _matrix.GetLength(0)]);

			for (int i = 0; i < _matrix.GetLength(0); i++)
			{
				for (int j = 0; j < _matrix.GetLength(1); j++)
				{
					result[i, j] = matrix[i, j] + _matrix[i, j];
				}
			}

			return result;
		}

		// Le produit de la matrice par un scalaire,qui retourne une matrice
		public Matrix Scalar(double number)
		{
			var result = new Matrix(new double[_matrix.GetLength(1), _matrix.GetLength(0)]);

			for (int i = 0; i < _matrix.GetLength(0); i++)
			{
				for (int j = 0; j < _matrix.GetLength(1); j++)
				{
					result[i, j] = _matrix[i, j] * number;
				}
			}

			return result;
		}

		public bool isSameDimention(Matrix matrix)
		{
			return (_matrix.GetLength(0) == matrix.GetLength(0)) && (_matrix.GetLength(1) == matrix.GetLength(1)) ;
		}

		//Le produit matriciel (avec une autre Matrice), qui retourne une matrice.
		/* Vous devez prévoir une version de cette méthode qui prend en
		paramètre un certain nombre de matrice à multiplier et qui permet de
		donner également le nombre d’opérations de produits effectués pour
		le calcul du produit matriciel. */
		public Matrix Multiply(Matrix matrix)
		{
			int operations;
			//Vérifie si les matrices sot compatibles à la multiplication
			if (_matrix.GetLength(1) != matrix.GetLength(0))
			{
				throw new ArgumentOutOfRangeException("Invalid matrix length");
			}
			else {
				operations = _matrix.GetLength(1);
			}

			//On crée la matrice résultante de la hauteur de l'origine et la largeur du multiplicandre
			var result = new Matrix(new double[_matrix.GetLength(0), matrix.GetLength(1)]);

			for (int i = 0; i < result.GetLength(0); i++)
			{
				for (int j = 0; j < result.GetLength(1); j++)
				{
					for (int k = 0; k < operations; k++)
					{
						result[i, j] += _matrix[i, k] * matrix[k, j];
					}
				}
			}

			return result;
		}

		private static double ScalarProduct(double[] vector1, double[] vector2)
		{
			if (vector1.Length != vector2.Length)
				throw new ArgumentOutOfRangeException("Invalid vector dimensions");

			double result = 0;

			for (int i = 0; i < vector1.Length; i++)
			{
				result += vector1[i] * vector2[i];
			}

			return result;
		}

		public Matrix Multiply(out int operations, params Matrix[] matrix )
		{
			operations = 0;
			return null;
		}

		/* Bool EstTriangulaire(…): retourne vrai ou faux selon si la matrice est triangulaire
		ou non ; comme arguments de la méthode, un premier paramètre doit dire si
		on souhaite vérifier si la méthode est triangulaire inférieure, supérieure ou peu
		importe, et un second paramètre doit indiquer soit on souhaite vérifier si elle
		est triangulaire stricte ou non. */
		public bool isTriangular(MatrixTriangularType type = MatrixTriangularType.Any, MatrixTriangularMode mode = MatrixTriangularMode.NonStrict)
		{
			return true;
		}

		public override string ToString()
		{
			string result = "";
			for (int i = 0; i < _matrix.GetLength(0); i++) {

				result += "|";
				for (int j = 0; j < _matrix.GetLength(1); j++) {

					result += _matrix[i, j];

					if (j < _matrix.GetLength(1) - 1) result += "\t";
				}
				result += "|\n";
			}
			return result;

		}

	}
}
