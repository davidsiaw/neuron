using System;
using System.Collections.Generic;
using System.Text;

namespace BlueBlocksLib.BaseClasses {

	public class Enumerable {

		private Enumerable() {
		}

		public static IEnumerable<int> Range(int start, int count) {
			return new Enumerable<int, int>(
				start,
				x => x + 1,
				x => x,
				x => x == start + count);
		}

		public static int Sum(IEnumerable<int> collection) {
			int a = 0;
			foreach (int num in collection) {
				a += num;
			}
			return a;
		}
	}

	class Enumerable<TState, T> : IEnumerable<T> {

		TState startState;
		Func<TState, TState> nextStep;
		Func<T, TState> extractOutput;
		Predicate<T> isLast;

		public Enumerable(
			TState startState,
			Func<TState, TState> nextStep,
            Func<T, TState> extractOutput,
			Predicate<T> isLast) {

			this.startState = startState;
			this.nextStep = nextStep;
			this.extractOutput = extractOutput;
			this.isLast = isLast;

		}

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator() {
			return new Enumerator<TState, T>(
				startState,
				nextStep,
				extractOutput,
				isLast);
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return this.GetEnumerator();
		}

		#endregion
	}


}
