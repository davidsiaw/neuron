using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotNumerics.LinearAlgebra;

namespace Neuron {
	public class BinaryMapControl : PictureBox {
		public BinaryMapControl() {
			this.MouseClick += new MouseEventHandler(BinaryMapControl_MouseClick);
		}

		List<Point> Class1 = new List<Point>();
		List<Point> Class2 = new List<Point>();

		List<Point> SupposedClass1 = new List<Point>();
		List<Point> SupposedClass2 = new List<Point>();

		public void ShowOverlay(List<Point> supposedClass1, List<Point> supposedClass2) {
			SupposedClass1 = supposedClass1;
			SupposedClass2 = supposedClass2;
			Refresh();
		}

		public float w1, w2, w0;

		public List<Tuple<Vector, Vector>> Data {
			get {
				List<Tuple<Vector, Vector>> data = new List<Tuple<Vector, Vector>>();
				foreach (var p in Class1) {
					data.Add(new Tuple<Vector, Vector>(new Vector(new double[] { p.X, p.Y }), new Vector(new double[] { 1 })));
				}
				foreach (var p in Class2) {
					data.Add(new Tuple<Vector, Vector>(new Vector(new double[] { p.X, p.Y }), new Vector(new double[] { 0 })));
				}
				return data;
			}
		}

		void BinaryMapControl_MouseClick(object sender, MouseEventArgs e) {
			if (e.Button == System.Windows.Forms.MouseButtons.Left) {
				Class1.Add(e.Location);
			} else {
				Class2.Add(e.Location);
			}
			Refresh();
		}

		protected override void OnPaint(PaintEventArgs pe) {
			pe.Graphics.FillRectangle(Brushes.Black, pe.ClipRectangle);

			if (w2 != 0) {
				float m = -w1 / w2;
				float c = (0.5f - w0) / w2;


				pe.Graphics.DrawLine(Pens.White, -pe.ClipRectangle.Width, -pe.ClipRectangle.Width * m + c, pe.ClipRectangle.Width, pe.ClipRectangle.Width * m + c);
			}

			foreach (var p in Class1) {
				pe.Graphics.DrawEllipse(Pens.Yellow, new Rectangle(p.X - 2, p.Y - 2, 5, 5));
			}
			foreach (var p in Class2) {
				pe.Graphics.DrawRectangle(Pens.Yellow, new Rectangle(p.X - 2, p.Y - 2, 5, 5));
			}

			foreach (var p in SupposedClass1) {
				pe.Graphics.DrawEllipse(Pens.Green, new Rectangle(p.X - 4, p.Y - 4, 9, 9));
			}
			foreach (var p in SupposedClass2) {
				pe.Graphics.DrawRectangle(Pens.Red, new Rectangle(p.X - 4, p.Y - 4, 9, 9));
			}
		}


		protected override void OnPaintBackground(PaintEventArgs pevent) {
		}
	}
}
