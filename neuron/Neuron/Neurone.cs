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
using BlueBlocksLib.Collections;
using BlueBlocksLib.BaseClasses;
using BlueBlocksLib.SetUtils;
using BlueBlocksLib.FileAccess;
using System.Runtime.InteropServices;

namespace Neuron
{
    public partial class Neurone : Form
    {

        GraphMap<FeatureVector, WeightMatrix> gm = new GraphMap<FeatureVector, WeightMatrix>();
        Dictionary<string, GraphMap<FeatureVector, WeightMatrix>.ILinkable> vectors = new Dictionary<string, GraphMap<FeatureVector, WeightMatrix>.ILinkable>();
		
        public Neurone()
        {
            InitializeComponent();
            gm.Dock = DockStyle.Fill;
            splitContainer1.Panel2.Controls.Add(gm);
        }

        private void SetActions(GraphMap<FeatureVector, WeightMatrix>.Box box)
        {
            box.Data.sourcenames.Add(box.Data.name);
            box.Data.state = new Matrix(box.Data.size, 1);

            if (box.Data.layer != LayerType.OUTPUT)
            {
                box.AddAction("Link", self =>
                {
                    gm.SelectNode(node =>
                    {
                        if (node.Data.layer != LayerType.INPUT)
                        {
                            LinkNodes(self, node);
                            return;
                        }
                    });
                });
            }

            //box.AddAction("Edit", self =>
            //{
            //    EditFeatureVector editfv = new EditFeatureVector(self.Data);
            //    editfv.ShowDialog();
            //    self.Data.size = editfv.NumOfUnits;
            //    self.Data.name = editfv.FVName;
            //    self.Name = editfv.FVName;
            //});
        }

        private static WeightMatrix LinkNodes(GraphMap<FeatureVector, WeightMatrix>.ILinkable self, GraphMap<FeatureVector, WeightMatrix>.ILinkable node)
        {
            if (self.Data.sourcenames.Contains(node.Data.name))
            {
                MessageBox.Show("Cycles not supported yet");
                return null;
            }

            WeightMatrix wm = new WeightMatrix(self.Data.size + 1, node.Data.size);
            wm.linkAccess = self.LinkTo(node, wm);
            wm.linkAccess.AddAction("See Matrix", x =>
            {
                var fm = new ViewMatrix(x.weights);
                fm.Show();
            });
            wm.linkAccess.Name = wm.weights.ColumnCount + "x" + wm.weights.RowCount;

            foreach (var sourcename in self.Data.sourcenames)
            {
                node.Data.sourcenames.Add(sourcename);
            }

            // add new
            UpdateLinks(self);
            return wm;
        }

        private static void UpdateLinks(GraphMap<FeatureVector, WeightMatrix>.ILinkable self)
        {
		}


        static Dictionary<FeatureVectorType, Tuple<Color, IPerceptronFunc, LayerType>> featVecType = new Dictionary<FeatureVectorType, Tuple<Color, IPerceptronFunc, LayerType>>(); 
        static Neurone()
        {
            featVecType[FeatureVectorType.INPUTLOG] = new Tuple<Color, IPerceptronFunc, LayerType>(Color.LightGreen, new LogisticPerceptronFunc(), LayerType.INPUT);
            featVecType[FeatureVectorType.HIDDENLIN] = new Tuple<Color, IPerceptronFunc, LayerType>(Color.Pink, new LinearPerceptronFunc(), LayerType.HIDDEN);
            featVecType[FeatureVectorType.HIDDENLOG] = new Tuple<Color, IPerceptronFunc, LayerType>(Color.HotPink, new LogisticPerceptronFunc(), LayerType.HIDDEN);
            featVecType[FeatureVectorType.OUTPUTLIN] = new Tuple<Color, IPerceptronFunc, LayerType>(Color.LightBlue, new LinearPerceptronFunc(), LayerType.OUTPUT);
            featVecType[FeatureVectorType.OUTPUTLOG] = new Tuple<Color, IPerceptronFunc, LayerType>(Color.DeepSkyBlue, new LogisticPerceptronFunc(), LayerType.OUTPUT);
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            EditFeatureVector efv = new EditFeatureVector(new FeatureVector());
            efv.ShowDialog();

            CreateLayer(efv.FVName, efv.NumOfUnits, FeatureVectorType.INPUTLOG);
        }

        private GraphMap<FeatureVector, WeightMatrix>.Box CreateLayer(string name, int numOfUnits, FeatureVectorType fvt)
        {
            if (vectors.ContainsKey(name))
            {
                MessageBox.Show("There already is a feature vector named " + name);
                return null;
            }

            var info = featVecType[fvt];
            FlowLayoutPanel flp = CreateStatePanel(numOfUnits);
            var box = gm.AddBox(info.Item1, new FeatureVector() { name = name, layer = info.Item3, type = info.Item2, size = numOfUnits, fvt = fvt }, x => x.name + " " + x.size + " units", flp);
            SetupStatePanel(flp, box);

            vectors[name] = box;

            SetActions(box);
            if (info.Item3 == LayerType.INPUT)
            {
                inputs.Add(box);
            }
            else if (info.Item3 == LayerType.HIDDEN)
            {
                hiddens.Add(box);
            }
            else if (info.Item3 == LayerType.OUTPUT)
            {
                outputs.Add(box);
            }

            return box;
        }

        private void btn_remove_Click(object sender, EventArgs e)
        {
            gm.SelectNode(node =>
            {
                gm.Delete(node);
                if (outputs.Contains(node))
                {
                    outputs.Remove((GraphMap<FeatureVector, WeightMatrix>.Box)node);
                }
                if (hiddens.Contains(node))
                {
                    hiddens.Remove((GraphMap<FeatureVector, WeightMatrix>.Box)node);
                }
                if (inputs.Contains(node))
                {
                    inputs.Remove((GraphMap<FeatureVector, WeightMatrix>.Box)node);
                }
            });

        }

        private void btn_addLogHiddenLayer_Click(object sender, EventArgs e)
        {
            EditFeatureVector efv = new EditFeatureVector(new FeatureVector());
            efv.ShowDialog();

            CreateLayer(efv.FVName, efv.NumOfUnits, FeatureVectorType.HIDDENLOG);
        }

        private void btn_addLinHiddenLayer_Click(object sender, EventArgs e)
        {
            EditFeatureVector efv = new EditFeatureVector(new FeatureVector());
            efv.ShowDialog();
            CreateLayer(efv.FVName, efv.NumOfUnits, FeatureVectorType.HIDDENLIN);
        }

        private void btn_addLogOutputLayer_Click(object sender, EventArgs e)
        {
            EditFeatureVector efv = new EditFeatureVector(new FeatureVector());
            efv.ShowDialog();

            CreateLayer(efv.FVName, efv.NumOfUnits, FeatureVectorType.OUTPUTLOG);
        }

        private void SetupStatePanel(FlowLayoutPanel flp, GraphMap<FeatureVector, WeightMatrix>.Box box)
        {
            foreach (CheckBox cb in flp.Controls)
            {
                cb.CheckedChanged += new EventHandler((o, ev) =>
                {
                    var chk = (o as CheckBox);
                    int num = (int)chk.Tag;
                    box.Data.state[num, 0] = chk.Checked ? 1 : 0;
                    btn_forwardProp_Click(o, ev);
                    chk.BackColor = chk.Checked ? Color.White : Color.Black;
                });
                flp.Width = 10;
                flp.Parent.Resize += new EventHandler((o, ev) =>
                {
                    ResizeBox(flp, box);
                });

                ResizeBox(flp, box);
            }
        }

        private static void ResizeBox(FlowLayoutPanel flp, GraphMap<FeatureVector, WeightMatrix>.Box box)
        {
            flp.Width = flp.Parent.Width - 6;
            int numPerRow = flp.Width / 16;
            int rows = box.Data.size / numPerRow;
            flp.Height = (rows + 1) * 16;
        }

        private void btn_addLinOutputLayer_Click(object sender, EventArgs e)
        {
            EditFeatureVector efv = new EditFeatureVector(new FeatureVector());
            efv.ShowDialog();
            CreateLayer(efv.FVName, efv.NumOfUnits, FeatureVectorType.OUTPUTLIN);
        }

        private static FlowLayoutPanel CreateStatePanel(int numOfUnits)
        {
            FlowLayoutPanel flp = new FlowLayoutPanel();
            flp.FlowDirection = FlowDirection.LeftToRight;
            flp.BackColor = Color.White;
            for (int i = 0; i < numOfUnits; i++)
            {
                var cb = new CheckBox();
                cb.Appearance = Appearance.Button;
                cb.BackColor = Color.Black;
                cb.Size = new System.Drawing.Size(10, 10);
                cb.Tag = i;
                flp.Controls.Add(cb);
            }
            return flp;
        }


        Set<GraphMap<FeatureVector, WeightMatrix>.Box> outputs = new Set<GraphMap<FeatureVector, WeightMatrix>.Box>();
        Set<GraphMap<FeatureVector, WeightMatrix>.Box> hiddens = new Set<GraphMap<FeatureVector, WeightMatrix>.Box>();
        Set<GraphMap<FeatureVector, WeightMatrix>.Box> inputs = new Set<GraphMap<FeatureVector, WeightMatrix>.Box>();

		private void btn_train_Click(object sender, EventArgs e) {

            //BinaryMap bm = new BinaryMap();
            //bm.ShowDialog();


			// move forward thru the graph and build a backwards graph for backprop

		}

        private void btn_forwardProp_Click(object sender, EventArgs e)
        {
            if (!chk_autoForwardProp.Checked)
            {
                return;
            }

            OneToManyMap<GraphMap<FeatureVector, WeightMatrix>.ILinkable, Pair<GraphMap<FeatureVector, WeightMatrix>.ILinkable, GraphMap<FeatureVector, WeightMatrix>.Link<WeightMatrix>>> backwards =
                new OneToManyMap<GraphMap<FeatureVector, WeightMatrix>.ILinkable, Pair<GraphMap<FeatureVector, WeightMatrix>.ILinkable, GraphMap<FeatureVector, WeightMatrix>.Link<WeightMatrix>>>();

            // Just to keep track of the frontier
            Queue<GraphMap<FeatureVector, WeightMatrix>.ILinkable> queue = new Queue<GraphMap<FeatureVector, WeightMatrix>.ILinkable>();

            // Vectors that receive outputs
            Queue<GraphMap<FeatureVector, WeightMatrix>.ILinkable> outputVectors = new Queue<GraphMap<FeatureVector, WeightMatrix>.ILinkable>();

            foreach (var input in inputs)
            {
                queue.Enqueue(input);
            }

            while (queue.Count != 0)
            {
                var node = queue.Dequeue();
                foreach (var edge in node.Edges)
                {
                    queue.Enqueue(edge.Key);
                    outputVectors.Enqueue(edge.Key);
                    backwards.Add(edge.Key, new Pair<GraphMap<FeatureVector, WeightMatrix>.ILinkable, GraphMap<FeatureVector, WeightMatrix>.Link<WeightMatrix>>()
                    {
                        a = node,
                        b = edge.Value
                    });
                }
            }

            while (outputVectors.Count != 0)
            {
                var node = outputVectors.Dequeue();
                var inputVectors = backwards[node];
                node.Data.state = new Matrix(node.Data.state.RowCount, node.Data.state.ColumnCount);
                foreach (var inputVector in inputVectors)
                {
                    var x = Training.AddBiasTerm(inputVector.a.Data.state);
                    node.Data.state += (inputVector.b.Data.weights * x);
                }
                node.Data.state = node.Data.type.Func(node.Data.state);

                if (node.Secondary != null)
                {
                    for (int i = 0; i < node.Data.size; i++)
                    {
                        CheckBox cb = (CheckBox)node.Secondary.Controls[i];
                        cb.Checked = node.Data.state[i, 0] >= 0.5 ? true : false;
                    }
                }
            }
        }

        private void btn_initialize_Click(object sender, EventArgs e)
        {
            Queue<GraphMap<FeatureVector, WeightMatrix>.ILinkable> queue = new Queue<GraphMap<FeatureVector, WeightMatrix>.ILinkable>();

            foreach (var input in inputs)
            {
                queue.Enqueue(input);
            }

            while (queue.Count != 0)
            {
                var node = queue.Dequeue();
                foreach (var edge in node.Edges)
                {
                    queue.Enqueue(edge.Key);
                    edge.Value.Data.weights = (Matrix.Random(edge.Value.Data.weights.RowCount, edge.Value.Data.weights.ColumnCount) - 0.5) * (1.0 / (double)(node.Edges.Count * node.Data.size));
                }
            }

            btn_forwardProp_Click(sender, e);
        }

        private void btn_addConfiguration_Click(object sender, EventArgs e)
        {
            string configString = string.Join(";", 
                inputs.Select(x => x.Data.Configuration)
                .Concat(outputs.Select(x => x.Data.Configuration))
                );
            AddToConfiguration(configString);
        }

        private void AddToConfiguration(string configString)
        {

            data_configurations.Rows.Add("Move To Train", "Move To CrsVld", configString);
        }

        private void AddToTraining(string configString)
        {
            data_training.Rows.Add("Move To Data", "Move To CrsVld", configString);
        }

        private void AddToCrossValidation(string configString)
        {
            data_crossValidationData.Rows.Add("Move To Train", "Move To Data", configString);
        }

        private void data_configurations_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((string)data_configurations[e.ColumnIndex, e.RowIndex].Value == "Move To Train")
            {
                AddToTraining(data_configurations[2, e.RowIndex].Value.ToString());
                data_configurations.Rows.RemoveAt(e.RowIndex);
            }

            if ((string)data_configurations[e.ColumnIndex, e.RowIndex].Value == "Move To CrsVld")
            {
                AddToCrossValidation(data_configurations[2, e.RowIndex].Value.ToString());
                data_configurations.Rows.RemoveAt(e.RowIndex);
            }

        }

        private void data_training_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((string)data_training[e.ColumnIndex, e.RowIndex].Value == "Move To Data")
            {
                AddToConfiguration(data_training[2, e.RowIndex].Value.ToString());
                data_training.Rows.RemoveAt(e.RowIndex);
            }

            if ((string)data_training[e.ColumnIndex, e.RowIndex].Value == "Move To CrsVld")
            {
                AddToCrossValidation(data_training[2, e.RowIndex].Value.ToString());
                data_training.Rows.RemoveAt(e.RowIndex);
            }

        }

        private void data_crossValidationData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((string)data_crossValidationData[e.ColumnIndex, e.RowIndex].Value == "Move To Train")
            {
                AddToTraining(data_crossValidationData[2, e.RowIndex].Value.ToString());
                data_crossValidationData.Rows.RemoveAt(e.RowIndex);
            }

            if ((string)data_crossValidationData[e.ColumnIndex, e.RowIndex].Value == "Move To Data")
            {
                AddToConfiguration(data_crossValidationData[2, e.RowIndex].Value.ToString());
                data_crossValidationData.Rows.RemoveAt(e.RowIndex);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        struct ProgramStateFile
        {
            public int version;
            public int numNodes;
            public int numConfigurations;
            public int numTrainings;
            public int numCrossValidations;

            [ArraySize("numNodes")]
            public Node[] nodes;

            [ArraySize("numConfigurations")]
            public StringData[] configurations;

            [ArraySize("numTrainings")]
            public StringData[] trainings;

            [ArraySize("numCrossValidations")]
            public StringData[] crossValidations;

        }

        [StructLayout(LayoutKind.Sequential)]
        struct Link
        {
            public MatrixFormat matrix;
            public StringData nodename;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MatrixFormat
        {
            public MatrixFormat(double [,] matrix)
            {
                rows = new MatrixRow[matrix.GetLength(1)];
                numRows = rows.Length;
                numColumns = matrix.GetLength(0);
                for (int y = 0; y < numRows; y++)
                {
                    rows[y].cols = new double[numColumns];
                    rows[y].numColumns = numColumns;
                    for (int x = 0; x < numColumns; x++)
                    {
                        rows[y].cols[x] = matrix[x, y];
                    }
                }
            }

            public double[,] Matrix
            {
                get
                {
                    double[,] res = new double[numColumns, numRows];

                    for (int y = 0; y < res.GetLength(1); y++)
                    {
                        for (int x = 0; x < res.GetLength(0); x++)
                        {
                            res[x, y] = rows[y].cols[x];
                        }
                    }

                    return res;
                }
            }

            public int numColumns, numRows;

            [ArraySize("numRows")]
            public MatrixRow[] rows;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MatrixRow
        {
            public int numColumns;

            [ArraySize("numColumns")]
            public double[] cols;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Node
        {
            public int x, y;
            public int size;
            public StringData name;
            public FeatureVectorType type;

            public int numlinks;

            [ArraySize("numlinks")]
            public Link[] links;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct StringData
        {
            public int stringlen;

            [ArraySize("stringlen")]
            public byte[] bytes;

            public string Contents
            {
                get
                {
                    return Encoding.UTF8.GetString(bytes);
                }
                set
                {
                    bytes = Encoding.UTF8.GetBytes(value);
                    stringlen = bytes.Length;
                }
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();
            sfd.Filter = "Neuron State Files (*.neuron)|*.neuron";
            if (sfd.FileName != null)
            {
                using (FormattedWriter fw = new FormattedWriter(sfd.FileName))
                {
                    ProgramStateFile psf = new ProgramStateFile();
                    psf.version = 0;
                    psf.nodes = inputs.Concat(hiddens).Concat(outputs).Select(x => new Node()
                    {
                        x = x.MainControl.Location.X,
                        y = x.MainControl.Location.Y,
                        size = x.Data.size,
                        name = new StringData() { Contents = x.Data.name },
                        type = x.Data.fvt,
                        numlinks = x.Edges.Count,
                        links = x.Edges.Select(edge => new Link()
                        {
                            matrix = new MatrixFormat(edge.Value.Data.weights.CopyToArray()),
                            nodename = new StringData() { Contents = edge.Key.Data.name }
                        }).ToArray()
                    }).ToArray();
                    psf.numNodes = psf.nodes.Length;

                    psf.configurations = new StringData[data_configurations.Rows.Count];
                    for (int i = 0; i < psf.configurations.Length; i++)
                    {
                        psf.configurations[i].Contents = data_configurations.Rows[i].Cells[2].Value.ToString();
                    }
                    psf.numConfigurations = psf.configurations.Length;

                    psf.trainings = new StringData[data_training.Rows.Count];
                    for (int i = 0; i < psf.trainings.Length; i++)
                    {
                        psf.trainings[i].Contents = data_training.Rows[i].Cells[2].Value.ToString();
                    }
                    psf.numTrainings = psf.trainings.Length;

                    psf.crossValidations = new StringData[data_crossValidationData.Rows.Count];
                    for (int i = 0; i < psf.crossValidations.Length; i++)
                    {
                        psf.crossValidations[i].Contents = data_crossValidationData.Rows[i].Cells[2].Value.ToString();
                    }
                    psf.numCrossValidations = psf.crossValidations.Length;

                    fw.Write(psf);
                }
            }
        }

        private void btn_load_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            ofd.Filter = "Neuron State Files (*.neuron)|*.neuron";
            if (ofd.FileName != null)
            {
                using (FormattedReader fr = new FormattedReader(ofd.FileName))
                {
                    var psf = fr.Read<ProgramStateFile>();

                    if (psf.version > 0)
                    {
                        MessageBox.Show("Version too high");
                        return;
                    }

                    // Create nodes
                    foreach (var node in psf.nodes)
                    {
                        var box = CreateLayer(node.name.Contents, node.size, node.type);
                        box.MainControl.Location = new Point(node.x, node.y);
                    }

                    // Link up
                    foreach (var node in psf.nodes)
                    {
                        foreach (var link in node.links)
                        {
                            var wm = LinkNodes(vectors[node.name.Contents], vectors[link.nodename.Contents]);
                            wm.weights = new Matrix(link.matrix.Matrix);
                        }
                    }

                    // Pull in configurations
                    foreach (var config in psf.configurations)
                    {
                        AddToConfiguration(config.Contents);
                    }

                    foreach (var config in psf.trainings)
                    {
                        AddToTraining(config.Contents);
                    }

                    foreach (var config in psf.crossValidations)
                    {
                        AddToCrossValidation(config.Contents);
                    }
                }
            }
        }

    }
}
