using System;
using System.Collections.Generic;
using System.Text;

namespace BlueBlocksLib.SetUtils {
	// This class is for facilitating memoization over large sets
	class Enumerator<TState, T> : IEnumerator<T> {

		TState startState;
		TState currentState;
		Func<TState, TState> nextStep;
		Func<T, TState> extractOutput;
		Predicate<T> isLast;

		public Enumerator(
			TState startState,
			Func<TState, TState> nextStep,
			Func<T, TState> extractOutput,
			Predicate<T> isLast) {

			this.startState = startState;
			this.currentState = startState;
			this.nextStep = nextStep;
			this.extractOutput = extractOutput;
			this.isLast = isLast;
		}

		#region IEnumerator<T> Members

		public T Current {
			get {
				return extractOutput(currentState);
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose() { }

		#endregion

		#region IEnumerator Members

		object System.Collections.IEnumerator.Current {
			get { return this.Current; }
		}

		public bool MoveNext() {
			if (isLast(Current)) {
				return false;
			}

			currentState = nextStep(currentState);
			return true;
		}

		public void Reset() {
			currentState = startState;
		}

		#endregion
	}
}
