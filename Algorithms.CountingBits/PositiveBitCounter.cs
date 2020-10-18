// <copyright file="PositiveBitCounter.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>

namespace Algorithms.CountingBits
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PositiveBitCounter
    {
        public IEnumerable<int> Count(int input)
        {
            if (input < 0) throw new ArgumentException();

            // Convert to binary array
            char[] binaryCharArray = Convert.ToString(input, 2).ToCharArray();

            // Reverse binaryCharArray
            Array.Reverse(binaryCharArray);

            // Convert Char array to int array
            int[] binaryIntArray = binaryCharArray.Select(c => (int)char.GetNumericValue(c)).ToArray();

            // Init positive count and list of results of the algorithm 
            int oneBitsCount = 0;
            List<int> results = new List<int>();

            for (int i = 0; i <= binaryIntArray.Count()-1; i++ )
            { 
                if (binaryIntArray[i] == 1)
                {
                    oneBitsCount++;
                    results.Add(i);
                }
            }

            // Insert oneBitsCount on the first postions in the results list
            results.Insert(0, oneBitsCount);

            return results;
        }


    }
}
