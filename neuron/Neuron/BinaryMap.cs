using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotNumerics.LinearAlgebra;
using System.Threading;
using System.Diagnostics;

namespace Neuron {
	public partial class BinaryMap : Form {
		public BinaryMap() {
			InitializeComponent();
		}

		private void BinaryMap_Load(object sender, EventArgs e) {

		}

		private void button1_Click(object sender, EventArgs e) {

			var fun = new LogisticPerceptronFunc();

			Matrix w = Matrix.Random(1, 3) * 0.5;

			Thread t = new Thread(() => {

				double learningrate = 1;
				Matrix lastdw = Matrix.Ones(1, 3);

				for (int i = 0; i < 10000; i++) {
					var data = binaryMapControl1.Data;

					binaryMapControl1.w0 = (float)w[0, 0] * 1000;
					binaryMapControl1.w1 = (float)w[0, 1];
					binaryMapControl1.w2 = (float)w[0, 2];

					List<Point> supposedClass1 = new List<Point>();
					List<Point> supposedClass2 = new List<Point>();

					foreach (var datum in data) {
						var x = Training.AddBiasTerm(datum.Item1 * 0.001);
						// forward prop
						var y = fun.Func(w * x);

						if (y[0, 0] > 0.5) {
							supposedClass1.Add(new Point((int)datum.Item1[0], (int)datum.Item1[1]));
						} else {
							supposedClass2.Add(new Point((int)datum.Item1[0], (int)datum.Item1[1]));
						}
					}					

					Matrix dwtotal = new Matrix(w.RowCount, w.ColumnCount);
					foreach (var datum in data) {
						var x = Training.AddBiasTerm(datum.Item1 * 0.001);
						// forward prop
						var y = fun.Func(w * x);

						var dy = datum.Item2 - y;

						Matrix dh;
						Matrix dw;

						Training.BackpropLayer(dy, x, w, fun, out dh, out dw);

						dwtotal -= dw;
					}

					Thread.Sleep(10);

					w = (w + dwtotal * (1 / (double)data.Count) * learningrate);

					double change = dwtotal.ElementsRMS();
					learningrate = (change == 0 ? 0 : 1 / change);

					Invoke(new Action(() => {
						binaryMapControl1.ShowOverlay(supposedClass1, supposedClass2);
						matrixView1.Matrix(w);
						matrixView2.Matrix(dwtotal);
					}));

					lastdw = dwtotal;
				}
			});

			t.Start();
		}
	}
}
