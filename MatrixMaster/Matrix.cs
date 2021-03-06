﻿using System;
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
		/// tableau à 2 dimensions de nombre décimaux, qui stocke la matrice.
		/// </summary>
		double[,] _matrix;

        //Constructeurs
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

        //TODO: Quel est le nom anglais de trace de la matrice... Layout ?
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

        public double Complement(int row, int column) //ici on utilise les indices mathématique
        {
            var sign = ((row + column)%2) == 0 ? 1 : -1;

            var minor = this.Minor(row, column);

            return sign*minor.Determinant;
        }


        public Matrix Minor(int row, int column) //ici on utilise les indices mathématique
        {
            if (!isSquare)
            {
                throw new InvalidOperationException("Matrix must be square to calcul minor");
            }

            var length = _matrix.GetLength(0); //Longueur de la matrice originale

            if (length < 2)
            {
                throw new InvalidOperationException("Matrix must be 2x2 or higher to calcul minor");
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

        //Cette méthode doit calculer la mineure (fonctions à créer) et l'utiliser
        public double Determinant //calcule et retourne le déterminant de la matrice, si la matrice est carrée ;
        {
            get
            {
                if (!isSquare)
                {
                    throw new InvalidOperationException("Matrix must be square to calcul determinant");
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

        public Matrix Transposed //calcule et retourne la transposée de la matrice, si la matrice est carrée ;
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


        public Matrix CoMatrix //calcule et retourne la comatrice de la matrice, si la matrice est carrée
        {
            get
            {
                //Il faut vérifier que le déterminant ne soit pas null
                if (this.Determinant == 0)
                {
                    throw new InvalidOperationException("Unable to calcul comatrix when determinant is null");
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

        public Matrix Inverse //calcule et retourne la matrice inverse, si la matrice est carrée
        {
            get
            {

                //Avant de tout calculer, assurez-vous que la matrice est régulière;
                if (!this.isRegular)
                {
                    throw new InvalidOperationException("Inverse will only work on regular matrix");
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

                //Si la matrice est, en plus, triangulaire, calculez directement la matrice inverse en utilisant le schème (voir énoncé)
                return result;
            }
        }

        public bool isSquare //retourne vrai ou faux selon si la matrice est carrée ou non
        {
            get { return _matrix.GetLength(0) == _matrix.GetLength(1); }
        }

        public bool isRegular
            //retourne vrai si la matrice est régulière(déterminant non nul) ou faux si la matrice est singulière(déterminant nul)
        {
            get
            {
                //TODO: Ajouter cette difficulté au rapport de projet.
                return this.isSquare && this.Determinant != 0;
            }
        }

        public int GetLength(int dimension)
        {
            return _matrix.GetLength(dimension);
        }

        //l’addition d’une matrice, qui retourne une matrice;
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

        // Le produit de la matrice par un scalaire,qui retourne une matrice
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

        public bool isSameDimention(Matrix matrix)
        {
            return (_matrix.GetLength(0) == matrix.GetLength(0)) && (_matrix.GetLength(1) == matrix.GetLength(1));
        }

		public bool isUnidimentionnal()
		{
			return true;
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

        private Matrix Multiply(out int operations, Matrix matrix)
        {
			operations = 0;
            int length;
            //Vérifie si les matrices sot compatibles à la multiplication
            if (_matrix.GetLength(1) != matrix.GetLength(0))
            {
                throw new ArgumentOutOfRangeException("Invalid matrix length");
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

		//Mutiplication de plusieurs matrices
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




	//Information complémentaire

	//Identité
	// 1 0 0
	// 0 1 0
	// 0 0 1

	//Diagonal Strict
	// 0 0 0
	// 0 0 0
	// 0 0 0

	//Diagonal (Non-Strict)
	// 3 0 0
	// 0 2 0
	// 0 0 3 

	//Upper (Non-Strict)
	// 3 5 6
	// 0 5 4
	// 0 0 7

	//Lower (Non-Strict)
	// 3 0 0
	// 0 5 0
	// 1 0 7

	//Upper (Strict)
	// 0 5 6
	// 0 0 4
	// 0 0 0

	//Lower (Strict)
	// 0 0 0
	// 0 0 0
	// 1 0 0
}
