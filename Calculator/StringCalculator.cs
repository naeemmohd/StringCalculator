﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class StringCalculator : IStringCalculator
    {
        private static string DefaultDelimiter
        {
            get { return ","; }
        }

        public int Add(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return 0;
            }
            var numbers = Tokenize(input);
            return numbers.Sum(i => i);
        }

        private static IEnumerable<int> Tokenize(string input)
        {
            input = input.Replace("\n", DefaultDelimiter);
            return input.Split(DefaultDelimiter.ToCharArray())
                        .Select(int.Parse);
        }
    }
}
