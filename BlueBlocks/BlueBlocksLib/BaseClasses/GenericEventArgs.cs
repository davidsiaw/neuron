using System;
using System.Collections.Generic;
using System.Text;

namespace BlueBlocksLib.BaseClasses {
	public class GenericEventArgs<T> : EventArgs {

		public T Args { get; set; }

		public GenericEventArgs(T arg) {
			this.Args = arg;
		}
	}
}
