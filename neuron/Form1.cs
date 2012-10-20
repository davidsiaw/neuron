using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace neuron {
	public partial class Form1 : Form {
		Playground<string> pg;

		public Form1() {
			InitializeComponent();
			pg = new Playground<string>();
			pg.Dock = DockStyle.Fill;
			splitContainer1.Panel2.Controls.Add(pg);
		}

		int a = 0;

		private void btn_addbox_Click(object sender, EventArgs e) {

			var b1 = pg.AddBox("m" + a, Color.LightGreen, "m" + a);
			b1.AddAction("link", x => {
				pg.SelectNode(other => {
					x.LinkTo(other);
				});
			});
			a++;
		}

		private void btn_sel_Click(object sender, EventArgs e) {
			pg.SelectNode(x => {
				Debug.WriteLine(((neuron.Playground<string>.Box)x).data);
			});
		}
	}
}
