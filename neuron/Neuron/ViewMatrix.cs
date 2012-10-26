using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotNumerics.LinearAlgebra;

namespace Neuron
{
    public partial class ViewMatrix : Form
    {
        public ViewMatrix(Matrix m)
        {
            InitializeComponent();
            MatrixView mv = new MatrixView();
            mv.Dock = DockStyle.Fill;
            this.Controls.Add(mv);
            mv.Matrix(m);
        }

        private void ViewMatrix_Load(object sender, EventArgs e)
        {

        }
    }
}
