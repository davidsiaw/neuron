// -----------------------------------------------------------------------
// <copyright file="Training.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Neuron {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using DotNumerics.LinearAlgebra;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class Training {

		public static void BackpropLayer(Matrix dy, Matrix h, Matrix w, IPerceptronFunc outputfunc, out Matrix dh, out Matrix dw) {
			Matrix z = (BaseMatrix)w * (BaseMatrix)h;

			Matrix y = outputfunc.Func(z);

			var dz = outputfunc.GradientAt(y);
			dz.ElemntsMult(dy);

			dh = (BaseMatrix)w.Transpose() * (BaseMatrix)dz;

			dw = (BaseMatrix)dz * (BaseMatrix)h.Transpose();
		}

		public static Matrix AddBiasTerm(Matrix x) {
			Matrix n = new Matrix(x.RowCount + 1, 1);
			n[0, 0] = 1;
			for (int i = 0; i < x.RowCount; i++) {
				n[i + 1, 0] = x[i, 0];
			}
			return n;
		}

        public static Matrix RemoveBiasTerm(Matrix x)
        {
            Matrix n = new Matrix(x.RowCount - 1, 1);
            n[0, 0] = 1;
            for (int i = 1; i < x.RowCount; i++)
            {
                n[i - 1, 0] = x[i, 0];
            }
            return n;
        }

		public static Matrix ForwardPropLayer(IPerceptronFunc outputfunc, Matrix x, Matrix w) {
			var z = (BaseMatrix)w * (BaseMatrix)x;
			return outputfunc.Func(z);
		}
	}

	public interface IPerceptronFunc {
		Matrix Func(Matrix z);
		Matrix GradientAt(Matrix y);
	}

	class LogisticPerceptronFunc : IPerceptronFunc {
		public Matrix Func(Matrix z) {
			Matrix y = new Matrix(z.RowCount, z.ColumnCount);
			for (int m = 0; m < z.ColumnCount; m++) {
				for (int n = 0; n < z.RowCount; n++) {
					y[n, m] = 1 / (1 + Math.Exp(-z[n, m]));
				}
			}
			return y;
		}

		public Matrix GradientAt(Matrix y) {
			var dz = y.Clone();
			dz.ElemntsMult(1 - y);
			return dz;
		}
	}

	class LinearPerceptronFunc : IPerceptronFunc {
		public Matrix Func(Matrix z) {
			return z.Clone();
		}

		public Matrix GradientAt(Matrix y) {
			return Matrix.Ones(y.RowCount, y.ColumnCount);
		}
	}
}
