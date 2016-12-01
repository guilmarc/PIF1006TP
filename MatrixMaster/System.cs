using System;
using System.Collections.Generic;

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

			var result = new Matrix(Length, 1);

			for ( int i = 1; i <= Length; i++ )
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
            var result = new Matrix(Length, 1);
            result = inverse1.Multiply(this.Matrix2);

            return result;
		}

	    public List<Matrix> SolveByJacobi(double epsilon = 0.05)
	    {

	        const int MAX_ITERATIONS = 100;


            if (!Matrix1.isStrictlyDominantDiagonally)
	        {
	            Console.WriteLine("Cette matrice ne respecte pas la condition de convergence");
	        }

            List<Matrix> matrixs = new List<Matrix>() { Matrix2.Copy() };


            //Ajout d'une nouvelle matrice de valeurs dans la liste

	        for (var k = 0; k < MAX_ITERATIONS; k++)
	        {
                matrixs.Add(Matrix2.Copy());
                for ( var i = 1; i <= Length; i++ ) {
                    matrixs[k + 1][i, 1] = (1 / Matrix1[i, i]) * (Matrix2[i, 1] - PartialScalarProduct(Matrix1, matrixs[k], i));
                }

                Console.WriteLine(matrixs[k]);

                if( ConverganceObtained(matrixs, epsilon) )
                {
                    return matrixs;
                }
            }

	        return matrixs;
	    }

	    private bool ConverganceObtained(List<Matrix> matrixs, double epsilon)
	    {
	        var k = matrixs.Count - 1;

            for (var i = 1; i <= Length; i++)
            {
                if ((matrixs[k][i, 1] - matrixs[k - 1][i, 1]) >= epsilon)
                {
                    return false;
                }
            }

	        return true;
	    }


	    private double PartialScalarProduct(Matrix a, Matrix x, int i)
	    {
	        double result = 0;

	        for (var j = 1; j <= Length; j++)
	        {
	            if (i != j)
	            {
	                result += a[i, j]*x[j, 1];
	            }        
	        }
            Console.WriteLine("i="+i + "=" + result);
	        return result;
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
