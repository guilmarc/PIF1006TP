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
				throw new FormatException("La matrice 2 doit être de format [n, 1]");
			}

			if (matrix1.GetLength(0) != matrix2.GetLength(0))
			{
				throw new FormatException("La matrice 2 doit avoir le même nombre de rangées que la matrice 1");
			}

			this.Matrix1 = matrix1;
			this.Matrix2 = matrix2;
		}

		//Matrice TrouverXParCramer() : retourne une matrice X contenant les valeurs des inconnues en appliquant la règle de Cramer;
		//Avant de procéder, on vérifie d’abord que le déterminant est non nul;
		//s’il est nul, alors il faut afficher un message explicatif à la console et retourner « null »;
		public Matrix SolveByCramer()
		{
			return null;
		}

		//Matrice TrouverXParInversionMatricielle() : retourne une matrice X contenant les valeurs des inconnues en appliquant la méthode d’inversion matricielle;
		//Avant de procéder, on vérifie d’abord que le déterminant est non nul; s’il est nul, alors il faut afficher un message explicatif à la console et
		//retourner « null ».
		public Matrix SolveByInversion()
		{
			return null;
		}

		//Matrice TrouverXParJacobi(double epsilon) : retourne une matrice X contenant les valeurs des inconnues en appliquant la méthode itérative de Jacobi;
		//Avant de procéder, on vérifie d’abord que la condition de convergence est respectée(la dominance diagonale stricte); si on ne peut s’assurer
		//de la convergence, alors on retourne « null » après avoir affiché un message explicatif à la console;
		//Le paramètre « epsilon » représente le taux d’écart minimal acceptable entre les valeurs des inconnues de deux itérations successives afin de
		//réussir le test de terminaison, c’est-à-dire de juger la solution comme ayant convergée.
		public Matrix SolveByJacobi()
		{
			return null;
		}



		//Enfin, ajoutez aussi une méthode permettant d’afficher le système (sous forme d’équations ax1 + ax2 + … = b1) à la console
		//ou dans un contrôle utilisateur(p.ex.en redéfinissant la méthode ToString() ou une méthode similaire).
		public override string ToString()
		{
			var result = "";
			var length = Matrix1.GetLength(0);

			for (var i = 1; i <= length; i++)
			{
				result += "|";
				for (var j = 1; j <= length; i++) 
				{
					
				}
				result += "| |";
			}

			return result;
		}

	}
}
