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
        

        //RÉSOLUTION PAR JACOBI

        //Les paramètres d'appel de cette fonction sont :

        //Matrix1 : un tableau carré(la matrice du système);
        //Matrix2 : un tableau unidimensionnel(le second membre du système);
        //x1 : un tableau unidimensionnel(le vecteur de départ);
        //Length : l'ordre de la matrice ;
        //iter : le nombre d'itérations demandé ;
        //eps : la précision demandée ;
        //t : un tableau rectangulaire.

        //Elle renvoie dans le tableau t les itérés successifs de la méthode de Jacobi; elle remplit ce tableau en commençant par les indices les plus élevés(t contient les derniers itérés si iter dépasse ITERMAX-1).

        //La constante entière NMAX est égale à la dimension maximale de la matrice(+1);
        //la constante entière ITERMAX est égale au nombre maximum d'itérations demandé (+1).

        void sl_jacobi(int iter, double eps)
        {
            int i, j, k;
            double alfa, s;
            int ITERMAX = iter + 1;
            int NMAX = Length + 1;
           
            var t = new Matrix(new double[NMAX, ITERMAX]);
            //1ère variable d'inconue
            var x1 = new Matrix(new double[NMAX, 1]);
            //2ème variable d'inconnue
            var x2 = new Matrix(new double[NMAX, 1]);
            for (i = 1; i <= Length; i++)
            {
                for (j = 1; j <= iter; j++)
                {
                    t[i, j] = 0.0;
                }
            }
            alfa = 1.0;
            k = 1;
            //vérifie si le nombre d"itérations n'est pas encore atteint et si la valeur alpha n'est pas inférier à celle de eps
            while (k <= iter && alfa > eps)
            {
                //ceci permet de calculer la valeur de la matrice xi
                for (i = 1; i <= Length; i++)
                {
                    s = this.Matrix2[i, 1];// prend es valeurs de la matrice unidimensionnelle
                    //ceci permet de faire le calcul sans la division
                    for (j = 1; j <= Length; j++)
                    {
                        if (i != j)
                        {
                            s -= this.Matrix1[i, j] * x1[j, 1];
                        }
                    }
                    //ceci est la division qui complete la formule
                    x2[i, 1] = s / this.Matrix1[i, i];
                }
                alfa = 0.0;
                for (i = 1; i <= Length; i++)
                {
                    //ici on incremente le alfa
                    alfa += Math.Pow(x2[i, 1] - x1[i, 1], 2);
                    x1[i, 1] = x2[i, 1];
                }
                for (i = 1; i <= Length; i++)
                {
                    for (j = 1; j <= ITERMAX - 2; j++)
                    {
                        t[i, j] = t[i, j + 1];
                    }
                    t[i, ITERMAX - 1] = x1[i, 1];
                }
                k++;
            }
        }


        //Elle renvoie la norme vectorielle euclidienne de la matrice mat de dimension Length
        double al_norme_vect(Matrix mat)
        {
            double t;
            int i;
            t = 0;
            for (i = 1; i <= Length; i++)
                t += mat[Length, 1] * mat[Length, 1];
            t = Math.Sqrt(t);
            return (t);
        }

        //méthode elle même
        public Matrix SolveByJacobi()
        {
            double eps;
            //double t[NMAX][ITERMAX];
            int i, j, iter;
            int NMAX = Length + 1;
            
            var x = new Matrix(new double[NMAX, 1]);
            Console.WriteLine("Méthode de Jacobi\n");
            Console.WriteLine("                           A                                     b\n");
            //prend au clavier la valeur seuil
            Console.WriteLine("Entrer la valeur seuil de précision");
            eps = double.Parse(Console.ReadLine());
            //prend au clavier le nombre d'iterations
            Console.WriteLine("Entrer le nombre d'itérations maximun");
            iter = int.Parse(Console.ReadLine());

            int ITERMAX = iter + 1;

            var t = new Matrix(new double[NMAX, ITERMAX]);

            for (i = 1; i <= Length; i++)
            {
                for (j = 1; j <= Length; j++)
                {
                    Console.WriteLine(this.Matrix1[i, j]);
                }
                Console.WriteLine(this.Matrix1[i, 1]);
                x[i, 1] = 0;
            }
            /*Console.WriteLine("Solution exacte :\n");
            for (i = 1; i <= Length; i++)
            {
                Console.WriteLine(sol[i]);
            }*/
            Console.WriteLine("Itérations :\n");
            sl_jacobi(iter, eps);
            for (j = 1; j <= iter; j++)
            {
                Console.WriteLine("x= (", j);
                for (i = 1; i <= Length; i++)
                {
                    //x[i, 1] = t[i, j] - sol[i];
                    Console.WriteLine(t[i, j]);
                }
                Console.WriteLine(")    err= \n", al_norme_vect(x));
            }
            Console.WriteLine("Dernier itéré :\n");
            for (i = 1; i <= Length; i++)
            {
                Console.WriteLine("\n", t[i, ITERMAX - 1]);
            }

            return (t);
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
