using System;
using System.Collections.Generic;
using System.Text;
using BlueBlocksLib.SetUtils;
using BlueBlocksLib.BaseClasses;
using BlueBlocksLib.Collections;

namespace BlueBlocksLib.StatisticalModels {

	struct ClassFeatureList {
		public int cls;
		public int[] features;
	}

	class NaiveBayesClassifier<TFeatures, TClass> {
		FixedCollator<TFeatures> ffeats;
		FixedCollator<TClass> fclasses;
		NaiveBayesClassifier classifier;

		public NaiveBayesClassifier(Pair<TClass, TFeatures[]>[] training) {

			ExpandingCollator<TFeatures> tfeats = new ExpandingCollator<TFeatures>();
			ExpandingCollator<TClass> tclass = new ExpandingCollator<TClass>();

			ClassFeatureList[] cfl = Array.ConvertAll(training, x =>
				new ClassFeatureList() {
					cls = tclass[x.a], features =
					Array.ConvertAll(x.b, feat => tfeats[feat])
				}
				);

			ffeats = new FixedCollator<TFeatures>(tfeats);
			fclasses = new FixedCollator<TClass>(tclass);

			classifier = new NaiveBayesClassifier(cfl, fclasses.Count, ffeats.Count);
		}

		public TClass Classify(TFeatures[] feats) {
			return fclasses[classifier.Classify(Array.ConvertAll(feats, x => ffeats[x]))];
		}

		public TClass[] ListSimilar(TFeatures[] feats) {
			return
				Array.ConvertAll(classifier.ListBySimilarity(
				Array.ConvertAll(feats, x => ffeats[x])),
				x => fclasses[x]);
		}
	}

	class NaiveBayesClassifier {

		double aPrioriEstimate;
		// P(C[i])
		double[] classProbabilities;
		// P(F[j]|C[i])
		Table<int, int, double> featureGivenClassProbabilities = new Table<int, int, double>();

		// lookup to improve speed
		Dictionary<int, Dictionary<int, bool>> featuresToClassMap = new Dictionary<int, Dictionary<int, bool>>();

		public NaiveBayesClassifier(ClassFeatureList[] training, int numOfClasses, int numOfFeatures) {
			int numOfDocuments = training.Length;
			classProbabilities = new double[numOfClasses];

			Dictionary<int, List<int>> classToDocMap = new Dictionary<int, List<int>>();

			foreach (var cfl in training) {
				if (!classToDocMap.ContainsKey(cfl.cls)) {
					classToDocMap[cfl.cls] = new List<int>();
				}
				classToDocMap[cfl.cls].AddRange(cfl.features);
			}

			int vocabSize = numOfFeatures;

			foreach (var cls in classToDocMap) {
				classProbabilities[cls.Key] = (double)cls.Value.Count / (double)numOfDocuments;
				Dictionary<int, int> featCounts = new Dictionary<int, int>();
				foreach (var feat in cls.Value) {

					if (!featuresToClassMap.ContainsKey(feat)) {
						featuresToClassMap[feat] = new Dictionary<int, bool>();
					}
					featuresToClassMap[feat][cls.Key] = true;

					if (!featCounts.ContainsKey(feat)) {
						featCounts[feat] = 0;
					}
					featCounts[feat]++;
				}

				foreach (var kvpair in featCounts) {
					int feature = kvpair.Key;
					int clss = cls.Key;
					int featurecount = kvpair.Value;
					int countOfAllFeaturesInClass = cls.Value.Count;

					featureGivenClassProbabilities[feature, clss] =
						(double)(featurecount + 1) / (double)(countOfAllFeaturesInClass + vocabSize);
				}
			}

			aPrioriEstimate = 1.0 / vocabSize / numOfClasses;
		}

		public int Classify(int[] features) {
			List<Pair<int, double>> probForClassGivenFeature = OrderClasses(features);
			return probForClassGivenFeature[0].a;
		}

		public int[] ListBySimilarity(int[] features) {
			List<Pair<int, double>> probForClassGivenFeature = OrderClasses(features);

			return Array.ConvertAll(probForClassGivenFeature.ToArray(), x => x.a);
		}

		private List<Pair<int, double>> OrderClasses(int[] features) {
			List<Pair<int, double>> probForClassGivenFeature = new List<Pair<int, double>>();

			Dictionary<int, bool> classesThatHaveTheseFeatures = new Dictionary<int, bool>();
			foreach (int feat in features) {
				foreach (int cls in featuresToClassMap[feat].Keys) {
					classesThatHaveTheseFeatures[cls] = true;
				}
			}

			foreach (int classid in classesThatHaveTheseFeatures.Keys) {
				double pc = Math.Log(classProbabilities[classid]);
				foreach (int feat in features) {
					Pair<int, int> featInClass = new Pair<int, int>() { a = feat, b = classid };
					double prob = featureGivenClassProbabilities[feat, classid];
					if (prob == 0) {
						pc += Math.Log(aPrioriEstimate);
					} else {
						pc += Math.Log(prob);
					}
				}
				probForClassGivenFeature.Add(new Pair<int, double>() {
					a = classid,
					b = pc
				});
			}

			probForClassGivenFeature.Sort((x, y) => y.b.CompareTo(x.b));
			return probForClassGivenFeature;
		}

	}

}
