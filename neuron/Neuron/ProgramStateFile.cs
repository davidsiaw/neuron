using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using BlueBlocksLib.FileAccess;

namespace Neuron
{
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
        public MatrixFormat(double[,] matrix)
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
}
