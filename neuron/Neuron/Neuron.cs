using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BlueBlocksLib.Controls;
using DotNumerics.LinearAlgebra;

namespace Neuron
{
    public partial class Neuron : Form
    {
        GraphMap<FeatureVector> gm = new GraphMap<FeatureVector>();

        public Neuron()
        {
            InitializeComponent();
            gm.Dock = DockStyle.Fill;
            splitContainer1.Panel2.Controls.Add(gm);
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            EditFeatureVector efv = new EditFeatureVector(new FeatureVector());
            efv.ShowDialog();

            var box = gm.AddBox(Color.LightGreen, new FeatureVector() { name = efv.FVName, layer = Layer.INPUT, type = FeatureVectorType.LOGISTIC, size = efv.NumOfUnits }, x => x.name + " " + x.size + " units");
            SetActions(box);
        }

        private void SetActions(GraphMap<FeatureVector>.Box box)
        {
            if (box.Data.layer != Layer.OUTPUT)
            {
                box.AddAction("Link", self =>
                {
                    gm.SelectNode(node =>
                    {
                        if (node.Data.layer != Layer.INPUT)
                        {
                            self.LinkTo(node);
                        }
                    });
                });
            }

            box.AddAction("Edit", self =>
            {
                EditFeatureVector editfv = new EditFeatureVector(self.Data);
                editfv.ShowDialog();
                self.Data.size = editfv.NumOfUnits;
                self.Data.name = editfv.FVName;
                self.Name = editfv.FVName;
            });
        }

        private void btn_remove_Click(object sender, EventArgs e)
        {
            gm.SelectNode(node =>
            {
                gm.Delete(node);
            });

        }

        private void btn_addLogHiddenLayer_Click(object sender, EventArgs e)
        {
            EditFeatureVector efv = new EditFeatureVector(new FeatureVector());
            efv.ShowDialog();

            var box = gm.AddBox(Color.Pink, new FeatureVector() { name = efv.FVName, layer = Layer.HIDDEN, type = FeatureVectorType.LOGISTIC, size = efv.NumOfUnits }, x => x.name + " (Logistic " + x.size + " units )");
            SetActions(box);
        }

        private void btn_addLinHiddenLayer_Click(object sender, EventArgs e)
        {
            EditFeatureVector efv = new EditFeatureVector(new FeatureVector());
            efv.ShowDialog();

            var box = gm.AddBox(Color.Pink, new FeatureVector() { name = efv.FVName, layer = Layer.HIDDEN, type = FeatureVectorType.LINEAR, size = efv.NumOfUnits }, x => x.name + " (Linear " + x.size + " units )");
            SetActions(box);
        }

        private void btn_addLogOutputLayer_Click(object sender, EventArgs e)
        {
            EditFeatureVector efv = new EditFeatureVector(new FeatureVector());
            efv.ShowDialog();

            var box = gm.AddBox(Color.LightBlue, new FeatureVector() { name = efv.FVName, layer = Layer.OUTPUT, type = FeatureVectorType.LOGISTIC, size = efv.NumOfUnits }, x => x.name + " (Logistic " + x.size + " units )");
            SetActions(box);
        }

        private void btn_addLinOutputLayer_Click(object sender, EventArgs e)
        {
            EditFeatureVector efv = new EditFeatureVector(new FeatureVector());
            efv.ShowDialog();

            var box = gm.AddBox(Color.LightBlue, new FeatureVector() { name = efv.FVName, layer = Layer.OUTPUT, type = FeatureVectorType.LOGISTIC, size = efv.NumOfUnits }, x => x.name + " (Linear " + x.size + " units )");
            SetActions(box);
        }
    }
}
