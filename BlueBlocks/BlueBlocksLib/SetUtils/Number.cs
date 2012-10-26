using System;
using System.Collections.Generic;
using System.Text;

namespace BlueBlocksLib.SetUtils
{
    public static class Number
    {
        public static int[] Range(int start, int count)
        {
            int[] numbers = new int[count];
            for (int i = 0; i < count; i++)
            {
                numbers[i] = i + start;
            }
            return numbers;
        }

        public static int Sum(IEnumerable<int> numbers)
        {
            int sum = 0;
            foreach (int number in numbers)
            {
                sum += number;
            }
            return sum;
        }
    }
}
