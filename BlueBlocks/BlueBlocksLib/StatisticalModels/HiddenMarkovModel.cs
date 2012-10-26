using System;
using System.Collections.Generic;
using System.Text;
using BlueBlocksLib.BaseClasses;
using BlueBlocksLib.SetUtils;

namespace BlueBlocksLib.StatisticalModels {
	public class HiddenMarkovModel<TInput, TOutput> {
		FixedCollator<TInput> inputs;
		FixedCollator<TOutput> outputs;
		HiddenMarkovModel hmm;

		public HiddenMarkovModel(Expectation<TInput, TOutput>[] training, HiddenMarkovModel.GenerationMethod generationMethod) {
			ExpandingCollator<TInput> tinputs = new ExpandingCollator<TInput>();
			ExpandingCollator<TOutput> toutputs = new ExpandingCollator<TOutput>();

			// collect the types
			Expectation<int, int>[] intExpectation =
				Array.ConvertAll(training, x =>
					new Expectation<int, int>(
						Array.ConvertAll(x.expectation, y =>
							new Pair<int, int>() {
								a = tinputs[y.a],
								b = toutputs[y.b]
							}
						)));

			inputs = new FixedCollator<TInput>(tinputs);
			outputs = new FixedCollator<TOutput>(toutputs);

			hmm = new HiddenMarkovModel(intExpectation, tinputs.Count, toutputs.Count, generationMethod);
		}

		public double Viterbi(TOutput[] outs, out TInput[] ins) {

			int[] intInputs;
			double d = hmm.Viterbi(Array.ConvertAll(outs, x => outputs[x]), out intInputs);
			ins = Array.ConvertAll(intInputs, x => inputs[x]);
			return d;
		}
	}

	public class HiddenMarkovModel {
		double[/*pi*/] initial;
		double[/*x*/,/*x*/] transition;
		double[/*x*/,/*y*/] output;


		public HiddenMarkovModel(
			double[/*pi*/] initial,
			double[/*x*/,/*x*/] transition,
			double[/*x*/,/*y*/] output) {
			Initialize(initial, transition, output);
		}

		private void Initialize(
			double[/*pi*/] initial,
			double[/*x*/,/*x*/] transition,
			double[/*x*/,/*y*/] output) {
			this.initial = initial;
			this.transition = transition;
			this.output = output;
		}

		public delegate HiddenMarkovModel GenerationMethod(Expectation<int, int>[] training, int stateCount, int observationCount);

		public HiddenMarkovModel(Expectation<int, int>[] initialData, int stateCount, int observationCount, GenerationMethod generationMethod) {


			// make the new expectations
			Expectation<int, int>[] exp = initialData;

			HiddenMarkovModel hmm = generationMethod(exp, stateCount, observationCount);
			this.Initialize(initial: hmm.initial,
							transition: hmm.transition,
							output: hmm.output);
		}

		public static HiddenMarkovModel AverageOccurance(Expectation<int, int>[] training, int statecount, int observationCount) {

			int[] initialCount = new int[statecount];
			int[,] transitionCount = new int[statecount, statecount];
			int[,] outputCount = new int[statecount, observationCount];

			new List<Expectation<int, int>>(training).ForEach(x => {
				// Count initials
				initialCount[x.expectation[0].a] += 1;

				// Count transitions
				new List<int>(Number.Range(0, x.expectation.Length - 1)).ForEach(index => {
					transitionCount[x.expectation[index].a, x.expectation[index + 1].a] += 1;
				});

				// Count outputs
				new List<int>(Number.Range(0, x.expectation.Length)).ForEach(index => {
					outputCount[x.expectation[index].a, x.expectation[index].b] += 1;
				});
			});

			int totalExamplesOfInitialStates = Number.Sum(initialCount);
			double[] initial = ArrayUtils.ConvertAll(initialCount, x => (double)x / (double)totalExamplesOfInitialStates);

			double[,] transition = new double[statecount, statecount];
			new List<int>(Number.Range(0, statecount)).ForEach(state1 => {
				int sumOfTransitions = Number.Sum((Array.ConvertAll(Number.Range(0, statecount), state2 => transitionCount[state1, state2])));
				new List<int>(Number.Range(0, statecount)).ForEach(state2 => transition[state1, state2] = (double)transitionCount[state1, state2] / (double)sumOfTransitions);
			});

			double[,] output = new double[statecount, observationCount];
			new List<int>(Number.Range(0, statecount)).ForEach(state => {
				int sumOfOutputs = Number.Sum(Array.ConvertAll(Number.Range(0, observationCount), o => outputCount[state, o]));
				new List<int>(Number.Range(0, observationCount)).ForEach(o => output[state, o] = (double)outputCount[state, o] / (double)sumOfOutputs);
			});

			return new HiddenMarkovModel(
				initial,
				transition,
				output
			);
		}

		public double Viterbi(int[] outputs, out int[] inputs) {
			int[] observations = outputs;

			double[,] v = new double[outputs.Length, initial.Length];
			List<int>[] path = new List<int>[initial.Length];

			for (int y = 0; y < initial.Length; y++) {
				v[0, y] = initial[y] * output[y, observations[0]];
				path[y] = new List<int>();
				path[y].Add(y);
			}

			for (int t = 1; t < observations.Length; t++) {
				List<int>[] newpath = new List<int>[initial.Length];

				for (int y = 0; y < initial.Length; y++) {

					// find the highest probability path
					double prob = 0;
					int state = 0;
					for (int y0 = 0; y0 < initial.Length; y0++) {
						double thisprob = v[t - 1, y0] * transition[y0, y] * output[y, observations[t]];
						int thisstate = y0;

						if (prob < thisprob) {
							prob = thisprob;
							state = thisstate;
						}
					}

					v[t, y] = prob;
					newpath[y] = new List<int>(path[state]);
					newpath[y].Add(y);
				}

				path = newpath;
			}

			double finalprob = 0;
			int finalstate = 0;
			for (int y = 0; y < initial.Length; y++) {
				double thisprob = v[observations.Length - 1, y];
				int thisstate = y;
				if (finalprob < thisprob) {
					finalprob = thisprob;
					finalstate = thisstate;
				}
			}

			inputs = path[finalstate].ToArray();
			return finalprob;
		}
	}
}
