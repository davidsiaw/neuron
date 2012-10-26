using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlueBlocksLib.FileAccess;
using DotNumerics.LinearAlgebra;
using BlueBlocksLib.Controls;

namespace Neuron
{

    public enum FeatureVectorType
    {
        INPUTLOG,
        HIDDENLIN,
        HIDDENLOG,
        OUTPUTLIN,
        OUTPUTLOG,
    }

    public enum LayerType
    {
        INPUT,
        HIDDEN,
        OUTPUT
    }

	public class FeatureVector {
		public FeatureVector() {
		}

        public FeatureVectorType fvt;
		public string name;
		public IPerceptronFunc type;
		public int size = 1;
		public LayerType layer;
        public Matrix state;
        public HashSet<string> sourcenames = new HashSet<string>();

        public string Configuration
        {
            get
            {
                return name + "=" + string.Join(",", state.GetColumnArray(0));
            }
        }
	}

	public class WeightMatrix {

        public WeightMatrix(int inputs, int outputs)
        {
            weights = new Matrix(rows: outputs, columns: inputs);
        }

		public string name;
        public Matrix weights;
        public GraphMap<FeatureVector, WeightMatrix>.Link<WeightMatrix> linkAccess;
	}
}
