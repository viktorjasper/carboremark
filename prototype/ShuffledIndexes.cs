using System;
using System.Collections.Generic;
using System.Text;

namespace GenerateMemTestPattern
{
    class ShuffledIndexes
    {
        private int _arraySize;
        private int[] _array;

        public long SizeOfArray { get { return _array.Length * sizeof(int); } }

        public string HumanFriendlySizeOfArray
        {
            get
            {
                return HumanFriendly.ToMetricStyle(SizeOfArray, "B");
            }
        }

        public void Generate(int arraySize)
        {
            _arraySize = arraySize;
            _array = new int[arraySize];
            GenerateAndShuffle();
        }

        private void GenerateAndShuffle()
        {
            // Initialize array
            for (int i = 0; i < _arraySize; i++)
            {
                _array[i] = i;
            }

            // Shuffle
            //
            // Shuffling this way never results in a self reference
            Random r = new Random((int)(DateTime.Now.Ticks % int.MaxValue));
            int j = _arraySize;
            int value;
            while (j > 1)
            {
                j--;
                int k = r.Next(j); // 0 <= k <= j-1

                // swap
                value = _array[j];
                _array[j] = _array[k];
                _array[k] = value;
            }
        }

        public void PrintIndexes()
        {
            for (int i = 0; i < _arraySize; i++)
            {
                Console.WriteLine(_array[i].ToString("000000000000"));
            }
            Console.WriteLine("Done.");
        }

        public ReadRate MeasurePerformance(double iterationCount)
        {
            ReadRate result = new ReadRate();

            if (iterationCount < 1.0)
            {
                iterationCount = 1;
            }

            long count = (long)(_arraySize * iterationCount);
            long readcount = count;
            int currentIndex = 0;
            DateTime start = DateTime.Now;
            while (count > 0)
            {
                currentIndex = _array[currentIndex];
                count--;
            }
            DateTime end = DateTime.Now;
            result.TotalSeconds = (end - start).TotalSeconds;
            result.Rate = readcount / result.TotalSeconds;
            result.ArraySize = _arraySize;

            return result;
        }
    }
}