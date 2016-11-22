using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace MatrixMaster
{
    public enum MatrixTriangularType
    {
        Diagonal = 0,
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
		/// <summary>
		/// Tableau à 2 dimensions de nombre décimaux, qui stocke la matrice.
		/// </summary>
		double[,] _matrix;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="matrix">new double [,]</param>
        public Matrix(double[,] matrix)
        {
			this._matrix = matrix;
        }

		/// <summary>
		/// Indexeur de la classe basé sur des indices mathématique afin d'offrir à l'utilisateur une utilisation régulière des matrices
		/// </summary>
		/// <param name="row">Rangée (à partir de 1)</param>
		/// <param name="column">Column (à partir de 1)</param>
        public double this[int row, int column]
        {
            get { return this._matrix[row - 1, column - 1]; }

            set { this._matrix[row - 1, column - 1] = value; }
        }

        /// <summary>
        /// Calcul la trace de la matrice
        /// </summary>
        public double Trace //calcule et retourne la trace de la matrice, si la matrice est carrée ;
        {
            get
            {
                if (this.isSquare)
                { 
                    double result = 0;

                    for (var i = 1; i <= _matrix.GetLength(0); i++)
                    {
                        result += this[i, i];
                    }

                    return result;
                }
                else
                {
                    return 0;
                }
            }
        }

		public Matrix GetDiagonal()
		{
			var result = new Matrix(new Double[this.GetLength(0), 1]);
			for (var i = 1; i <= this.GetLength(0); i++)
				result[i, 1] = this[i, i];
			return result;
		}

		/// <summary>
		/// Calcul le complément de la matrice à partir un pivot
		/// </summary>
		/// <param name="row">La rangée (indice mathématique)</param>
		/// <param name="column">La colonne (indice mathématique)</param>
        private double Complement(int row, int column)
        {
            var sign = ((row + column)%2) == 0 ? 1 : -1;

            var minor = this.Minor(row, column);

            return sign*minor.Determinant;
        }

		/// <summary>
		/// Calcul de la mineure de la matrice à partir d'un point pivot
		/// </summary>
		/// <param name="row">La rangée (indice mathématique)</param>
		/// <param name="column">La colonne (indice mathématique)</param>
        private Matrix Minor(int row, int column)
        {
            if (!isSquare)
            {
                throw new InvalidOperationException("La matrice doit être carrée pour calculer la mineure!");
            }

            var length = _matrix.GetLength(0); //Longueur de la matrice originale

            if (length < 2)
            {
                throw new InvalidOperationException("La matrice doit être au minimum 2x2!");
            }

            var result = new Matrix(new double[length - 1, length - 1]);

            var newRow = 1;
            var newColumn = 1;

            for (var i = 1; i <= length; i++)
            {
                if (i != row)
                {
                    newColumn = 1;

                    for (var j = 1; j <= length; j++)
                    {
                        if (j != column)
                        {
                            result[newRow, newColumn] = this[i, j];
                            newColumn++;
                        }
                    }

                    newRow++;
                }
            }

            return result;
        }

        /// <summary>
        /// Calcule et retourne le déterminant de la matrice, si la matrice est carrée
        /// </summary>
        public double Determinant
        {
            get
            {
                if (!isSquare)
                {
                    throw new InvalidOperationException("La matrice doit être carrée pour calculer le déterminant!");
                }

                int i = 1;
                    //Nous allons calculer le déterminant en utilisant la première ligne (i = 0) et 1 pour le calcul des signes
                double result = 0;
                var length = _matrix.GetLength(1);

                if (length == 2)
                {
                    result = this[1, 1]*this[2, 2] - this[1, 2]*this[2, 1]; //ad - bc
                }
                else
                {
                    for (var j = 1; j <= _matrix.GetLength(1); j++)
                    {
                        var coefficient = this[i, j];

                        result += coefficient*this.Complement(i, j);
                    }
                }

                return result;
            }
        }

		/// <summary>
		/// Calcule et retourne la transposée de la matrice, si la matrice est carrée
		/// </summary>
        public Matrix Transposed
        {
            get
            {
                var result = new Matrix(new double[_matrix.GetLength(1), _matrix.GetLength(0)]);

                for (var i = 1; i <= _matrix.GetLength(0); i++)
                {
                    for (var j = 1; j <= _matrix.GetLength(1); j++)
                    {
                        result[j, i] = this[i, j];
                    }
                }

                return result;
            }
        }

		/// <summary>
		/// Calcule et retourne la comatrice de la matrice, si la matrice est carrée
		/// </summary>
        public Matrix CoMatrix
        {
            get
            {
                //Il faut vérifier que le déterminant ne soit pas null
				if (this.Determinant == 0)
                {
                    throw new InvalidOperationException("Imposible de calculer la comatrice quand le déterminant est null!");
                }

                var length = _matrix.GetLength(0);

                var result = new Matrix(new double[length, length]);

                for (var i = 1; i <= length; i++)
                {
                    for (var j = 1; j <= length; j++)
                    {
                        result[i, j] = this.Complement(i, j);
                    }
                }

                //Pour tester si ca marche, multiplier le résultat de CoMatrix par la matrice originale
                //le résultat devrait donner la matrice identité.
                return result;
            }
        }

		/// <summary>
		/// Calcule et retourne la matrice inverse, si la matrice est carrée
		/// </summary>
        public Matrix Inverse
        {
            get
            {

                //Avant de tout calculer, assurez-vous que la matrice est régulière;
                if (!this.isRegular)
                {
                    throw new InvalidOperationException("Impossible de calculer l'inverse sur une matrice irrégulière!");
                }

                var length = this.GetLength(0);

                var result = new Matrix(new double[length, length]);

                if (this.isTriangular())
                {
                    
                    for (var i = 1; i <= length; i++)
                    {
                        if (this[i, i] != 0) //TODO: Serait innutile car déjà vérifié
                        {
                            result[i, i] = 1/this[i, i];
                        }
                    }
                }
                else
                {
                    result = this.CoMatrix.Transposed.Scalar(1 / this.Determinant);
                }

                return result;
            }
        }

		/// <summary>
		/// Retourne vrai ou faux selon si la matrice est carrée ou non
		/// </summary>
        public bool isSquare //retourne vrai ou faux selon si la matrice est carrée ou non
        {
            get { return _matrix.GetLength(0) == _matrix.GetLength(1); }
        }

		/// <summary>
		/// Retourne vrai si la matrice est régulière(déterminant non nul) ou faux si la matrice est singulière(déterminant nul)
		/// </summary>
        public bool isRegular
            //
        {
            get
            {
                //TODO: Ajouter cette difficulté au rapport de projet.
				return this.isSquare && this.Determinant != 0;
            }
        }

		/// <summary>
		/// Retourne la longueur d'une dimention de la matrice
		/// </summary>
        public int GetLength(int dimension)
        {
            return _matrix.GetLength(dimension);
        }

        /// <summary>
        /// Additionne la matrice en cours avec une matrice en paramètre
        /// </summary>
        public Matrix Add(Matrix matrix)
        {
            if (!this.isSameDimention(matrix))
            {
                throw new ArgumentOutOfRangeException("Inconsistent matrix dimensions");
            }

            var result = new Matrix(new double[_matrix.GetLength(1), _matrix.GetLength(0)]);

            for (var i = 1; i <= _matrix.GetLength(0); i++)
            {
                for (var j = 1; j <= _matrix.GetLength(1); j++)
                {
                    result[i, j] = matrix[i, j] + this[i, j];
                }
            }

            return result;
        }

		/// <summary>
		/// Le produit de la matrice par un scalaire,qui retourne une matrice
		/// </summary>
        public Matrix Scalar(double number)
        {
            var result = new Matrix(new double[_matrix.GetLength(1), _matrix.GetLength(0)]);

            for (var i = 1; i <= _matrix.GetLength(0); i++)
            {
                for (var j = 1; j <= _matrix.GetLength(1); j++)
                {
                    result[i, j] = this[i, j]*number;
                }
            }

            return result;
        }

		/// <summary>
		/// Évalue si deux matrices dont de même dimention
		/// </summary>
        public bool isSameDimention(Matrix matrix)
        {
            return (_matrix.GetLength(0) == matrix.GetLength(0)) && (_matrix.GetLength(1) == matrix.GetLength(1));
        }

		/// <summary>
		/// Evalue si la matrice est unidimentionnelle
		/// </summary>
		public bool isUnidimentionnal
		{
			get
			{
				return _matrix.GetLength(0) == 1 || _matrix.GetLength(1) == 1;
			}
		}

		//Le produit matriciel (avec une autre Matrice), qui retourne une matrice.
		/* Vous devez prévoir une version de cette méthode qui prend en
        paramètre un certain nombre de matrice à multiplier et qui permet de
        donner également le nombre d’opérations de produits effectués pour
        le calcul du produit matriciel. */
		public Matrix Multiply(Matrix matrix)
		{
			int operations;
			return Multiply(out operations, matrix);
		}

		/// <summary>
		/// Multiplication de deux matrices
		/// </summary>
        private Matrix Multiply(out int operations, Matrix matrix)
        {
			operations = 0;
            int length;
            //Vérifie si les matrices sot compatibles à la multiplication
            if (_matrix.GetLength(1) != matrix.GetLength(0))
            {
                throw new ArgumentOutOfRangeException("Taille de matrices incompatibles pour la multiplication.");
            }
            else
            {
                length = _matrix.GetLength(1);
            }

            //On crée la matrice résultante de la hauteur de l'origine et la largeur du multiplicandre
            var result = new Matrix(new double[_matrix.GetLength(0), matrix.GetLength(1)]);

            for (var i = 1; i <= result.GetLength(0); i++)
            {
                for (var j = 1; j <= result.GetLength(1); j++)
                {
                    for (var k = 1; k <= length; k++)
                    {
                        result[i, j] += this[i, k] * matrix[k, j];
						operations++;
                    }
                }
            }

            return result;
        }

		/// <summary>
		/// Multiplication de plusieurs matrices en obtenant le nombre d'opérations effectués
		/// </summary>
        public Matrix Multiply(out int operations, params Matrix[] matrixs)
        {
            operations = 0;
			int total = 0;

			Matrix result = this;

			for (var i = 0; i < matrixs.Length; i++)
			{
				result = result.Multiply(out operations, matrixs[i]);
				total += operations;
			}

			operations = total;
			return result;
        }


        /* Bool EstTriangulaire(…): retourne vrai ou faux selon si la matrice est triangulaire
        ou non ; comme arguments de la méthode, un premier paramètre doit dire si
        on souhaite vérifier si la méthode est triangulaire inférieure, supérieure ou peu
        importe, et un second paramètre doit indiquer soit on souhaite vérifier si elle
        est triangulaire stricte ou non. */
        public bool isTriangular(MatrixTriangularType type = MatrixTriangularType.Diagonal, MatrixTriangularMode mode = MatrixTriangularMode.NonStrict)
        {
			if (!isSquare)
			{
				throw new InvalidOperationException("Matrix must be square to calcul triangular");
			}

			var length = this.GetLength(0);

			bool strict = true;
			bool upper = true;
			bool lower = true;
			bool diagonal = true;

			for (var i = 1; i <= length; i++)
			{
				for (var j = 1; j <= length; j++)
				{
					if (this[i, j] != 0)
					{
						if (i == j) strict = false;
						if (i < j) upper = false;
						if (i > j) lower = false;
					}
					else {
						if (i == j) diagonal = false;
					}
				}
			}

			switch (type)
			{
				case MatrixTriangularType.Upper:	return upper && (mode == MatrixTriangularMode.Strict ? strict : true);

				case MatrixTriangularType.Lower:	return lower && (mode == MatrixTriangularMode.Strict ? strict : true);
					
				case MatrixTriangularType.Diagonal:	return diagonal && lower && upper && (mode == MatrixTriangularMode.Strict ? strict : true);

				default:							return false;
			}

        }


		/// <summary>
		/// Retour vrai si la matrice est strictement dominante diagonalement
		/// </summary>
		public bool isStrictlyDominantDiagonally
		{
			get
			{
				for (var i = 1; i <= this.GetLength(0); i++)
				{
					double sum = 0;
					for (var j = 1; j <= this.GetLength(1); j++ )
					{
						if (i != j) sum += this[i, j];
					}
					if (sum >= this[i, i]) return false;
				}
				return true;
			}
		}


		/// <summary>
		/// Clone la matrice en cours
		/// </summary>
		public Matrix Clone()
		{
			return new Matrix( (double[,])_matrix.Clone() );
		}

		/// <summary>
		/// Affiche la matrice sous un format standard
		/// </summary>
        public override string ToString()
        {
            string result = "";
            for (var i = 1; i <= _matrix.GetLength(0); i++)
            {
                result += "|";
                for (var j = 1; j <= _matrix.GetLength(1); j++)
                {
					result += this[i, j];

                    if (j < _matrix.GetLength(1)) result += "\t";
                }
                result += "|\n";
            }
            return result;
        }
    }
}
