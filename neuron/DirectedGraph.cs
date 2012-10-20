// -----------------------------------------------------------------------
// <copyright file="DirectedGraph.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace neuron {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class DirectedGraph {
		IEnumerable<INode> vertices;

		public DirectedGraph(HashSet<INode> vertices) {
			this.vertices = vertices;
		}
	}

	public interface INode {
		HashSet<INode> Edges { get; }
	}
}
