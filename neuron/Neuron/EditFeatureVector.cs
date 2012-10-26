using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Neuron
{
    public partial class EditFeatureVector : Form
    {
        static int count = 0;
        public EditFeatureVector(FeatureVector fv)
        {
            InitializeComponent();
            textBox1.Text = string.IsNullOrEmpty(fv.name) ? "Incognito " + count : fv.name;
            numericUpDown1.Value = fv.size;
            count++;
        }

        private void EditFeatureVector_Load(object sender, EventArgs e)
        {

        }

        public int NumOfUnits
        {
            get
            {
                return (int)numericUpDown1.Value;
            }
        }

        public string FVName
        {
            get
            {
                return textBox1.Text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                this.Close();
            }
        }
    }
}
