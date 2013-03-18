using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using GraphLib;

namespace Neuron
{
    public partial class TrainingForm : Form
    {
        Action<int, TrainingType, double, Action<double>> trainingFunc;

        public TrainingForm(Action<int, TrainingType, double, Action<double>> trainingFunc)
        {
            InitializeComponent();

            this.trainingFunc = trainingFunc;
            numericUpDown1_ValueChanged(this, new EventArgs());

            this.FormClosing += new FormClosingEventHandler(TrainingForm_FormClosing);
        }

        void TrainingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        List<cPoint> error = new List<cPoint>();
        private void TrainingForm_Load(object sender, EventArgs e)
        {
            plotterDisplayEx1.PanelLayout = GraphLib.PlotterGraphPaneEx.LayoutMode.STACKED;
            plotterDisplayEx1.DataSources.Add(new GraphLib.DataSource());
            plotterDisplayEx1.DataSources[0].AutoScaleX = true;
            plotterDisplayEx1.DataSources[0].AutoScaleY = true;
            plotterDisplayEx1.DataSources[0].GraphColor = Color.Red;
            plotterDisplayEx1.DataSources[0].SetGridDistanceY(10);
            plotterDisplayEx1.DashedGridColor = Color.Gray;
            plotterDisplayEx1.SolidGridColor = Color.Gray;
            plotterDisplayEx1.SetGridDistanceX(1000);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            btn_batchtrain.Text = "Batch for " + num_iters.Value + " iterations";
            btn_onlinetrain.Text = "Online for " + num_iters.Value + " iterations";
        }

        private void btn_train_Click(object sender, EventArgs e)
        {
            TrainWithType(TrainingType.Batch);
        }

        private void TrainWithType(TrainingType type)
        {

            Thread t = new Thread(() =>
            {
                double learningRate = 1;
                Invoke(new Action(() =>
                {
                    learningRate = (double)num_learningRate.Value;
                }));
                trainingFunc((int)num_iters.Value, type, learningRate, x =>
                {
                    error.Add(new cPoint() { x = error.Count, y = (float)x * 100 });
                    if (error.Count % 100 == 0)
                    {
                        UpdateGraph();
                    }
                });

                Invoke(new Action(() =>
                {
                    btn_batchtrain.Enabled = true;
                }));
            });
            btn_batchtrain.Enabled = false;
            t.IsBackground = true;
            t.Start();
        }

        private void UpdateGraph()
        {

            plotterDisplayEx1.DataSources[0].Length = error.Count;
            plotterDisplayEx1.DataSources[0].Samples = error.ToArray();
            plotterDisplayEx1.SetDisplayRangeX(0, error.Count);

            Invoke(new Action(() =>
            {
                plotterDisplayEx1.Refresh();
            }));
        }

        private void btn_onlinetrain_Click(object sender, EventArgs e)
        {
            TrainWithType(TrainingType.Online);
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            error = new List<cPoint>();
            UpdateGraph();
        }
    }
}
