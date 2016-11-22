using System;

namespace MatrixMaster
{
	public class System
	{

		Matrix Matrix1 { get; set; }
		Matrix Matrix2 { get; set; }

		public System(Matrix matrix1, Matrix matrix2)
		{
			if (!matrix1.isSquare)
			{
				throw new FormatException("La matrice 1 doit être carrée !");
			}

			if (matrix2.GetLength(1) != 1)
			{
				throw new FormatException("La matrice 2 doit être de format [n, 1].");
			}

			if (matrix1.GetLength(0) != matrix2.GetLength(0))
			{
				throw new FormatException("La matrice 2 doit avoir le même nombre de rangées que la matrice 1.");
			}

			this.Matrix1 = matrix1;
			this.Matrix2 = matrix2;
		}

		public int Length
		{
			get { return Matrix1.GetLength(0); }
		}

		private Matrix GetCramerMatrixAt(int index)
		{
			var result = this.Matrix1.Clone();
			for (int i = 1; i <= Length; i++)
			{
				result[i, index] = this.Matrix2[i, 1];
			}


			return result;
		}

		//Matrice TrouverXParCramer() : retourne une matrice X contenant les valeurs des inconnues en appliquant la règle de Cramer;
		//Avant de procéder, on vérifie d’abord que le déterminant est non nul;
		//s’il est nul, alors il faut afficher un message explicatif à la console et retourner « null »;
		public Matrix SolveByCramer()
		{
			if (this.Matrix1.Determinant == 0)
			{
				Console.WriteLine("Impossible de résoudre cette équation car le détermiannt est null.");
				return null;
			}

			var result = new Matrix(new double[Length, 1]);

			for (int i = 1; i <= Length; i++ )
			{
				Console.WriteLine(GetCramerMatrixAt(i).Determinant);
				result[i, 1] = GetCramerMatrixAt(i).Determinant / Matrix1.Determinant;
			}

			return result;
		}

		//Matrice TrouverXParInversionMatricielle() : retourne une matrice X contenant les valeurs des inconnues en appliquant la méthode d’inversion matricielle;
		//Avant de procéder, on vérifie d’abord que le déterminant est non nul; s’il est nul, alors il faut afficher un message explicatif à la console et
		//retourner « null ».
		public Matrix SolveByInversion()
		{
            if(this.Matrix1.Determinant == 0)
            {
                Console.WriteLine("Le determinant de la premiere matrice doit être différente de zéro.");
                return null;
            }
          
            Matrix inverse1 = this.Matrix1.Inverse;
            var result = new Matrix(new double[Length, 1]);
            result = inverse1.Multiply(this.Matrix2);

            return result;
		}

		//Matrice TrouverXParJacobi(double epsilon) : retourne une matrice X contenant les valeurs des inconnues en appliquant la méthode itérative de Jacobi;
		//Avant de procéder, on vérifie d’abord que la condition de convergence est respectée(la dominance diagonale stricte); si on ne peut s’assurer
		//de la convergence, alors on retourne « null » après avoir affiché un message explicatif à la console;
		//Le paramètre « epsilon » représente le taux d’écart minimal acceptable entre les valeurs des inconnues de deux itérations successives afin de
		//réussir le test de terminaison, c’est-à-dire de juger la solution comme ayant convergée.
		public Matrix SolveByJacobi(double epsilion)
		{
			if (!this.Matrix1.isStrictlyDominantDiagonally)
			{
				Console.WriteLine("Impossible de calculer selon la méthode de Jacobi. Cette matrice n'est pas strictement dominante diagonalement !");
				return null;
			}

			var diagonal = this.Matrix2.GetDiagonal();
			var length = this.Matrix2.GetLength(0);



			// Créer une matice d [n, 1] à partir de la diagonale:


			//Créer une matrice de [n, n + 1]  -->  (n+1) = matrice2[i,0] / d[i,0].  Elle ne varie plus

			//-matrice2[i, j] / d[i, 0];


		}



		//Enfin, ajoutez aussi une méthode permettant d’afficher le système (sous forme d’équations ax1 + ax2 + … = b1) à la console
		//ou dans un contrôle utilisateur(p.ex.en redéfinissant la méthode ToString() ou une méthode similaire).
		public override string ToString()
		{
			var result = "";
			var length = this.Length;

			for (var i = 1; i <= length; i++)
			{
				for (var j = 1; j <= length; j++)
				{
					result += Math.Round(Matrix1[i, j],2).ToString() + "X" + j.ToString();
					if (j < length) result += " + ";
				}

				result += " = " + Math.Round(Matrix2[i, 1], 2).ToString() + "\n";
			}

			return result;
		}


	}
}
