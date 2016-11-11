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

			if (1 = 1)
			{
				throw new FormatException("La matrice 2 doit être unidimentionnelle");
			}

		}
	}
}
