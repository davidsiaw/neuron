using System;
using System.Collections.Generic;
using System.Text;
using BlueBlocksLib.BaseClasses;

namespace BlueBlocksLib.StatisticalModels
{
    public class Expectation<TInput, TOutput>
    {
        public readonly Pair<TInput, TOutput>[] expectation;
        public Expectation(Pair<TInput, TOutput>[] expectation)
        {
            this.expectation = expectation;
        }
    }
}
