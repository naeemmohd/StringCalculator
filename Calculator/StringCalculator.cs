﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            ValidateInput(numbers);
            numbers = RemoveInvalidNumbers(numbers);

            return numbers.Sum(i => i);
        }

        private static List<int> Tokenize(string input)
        {
            input = ReplaceUserSpecifiedDelimiterWithComma(input);
            input = ReplaceNewLineDelimiterWithComma(input);
            return input.Split(DefaultDelimiter.ToCharArray())
                        .Select(int.Parse)
                        .ToList();
        }

        private static string ReplaceNewLineDelimiterWithComma(string input)
        {
            input = input.Replace("\n", DefaultDelimiter);
            return input;
        }

        private static string ReplaceUserSpecifiedDelimiterWithComma(string input)
        {
            if (input.StartsWith("//"))
            {
                var delimiters = ExtractDelimiters(ref input);

                if (delimiters != null)
                {
                    foreach (var delimiter in delimiters)
                    {
                        input = input.Replace(delimiter, DefaultDelimiter);
                    }

                    return input;
                }
            }
            return input;
        }

        private static List<string> ExtractDelimiters(ref string input)
        {
            var delimitersRangeRegex = new Regex("//(.+)\n");

            Match rangeMatch = delimitersRangeRegex.Match(input);
            
            if (!rangeMatch.Success)
            {
                return null;
            }

            input = delimitersRangeRegex.Replace(input, "");

            string delimitersString = rangeMatch.Groups[1].Value;
            return GetDelimitersCollection(delimitersString);
        }

        private static List<string> GetDelimitersCollection(string delimitersString)
        {
            var delimiterCollectionRegex = new Regex(@"\[(.+?)]"); ;
            var delimiters = new List<string>();

            MatchCollection mc = delimiterCollectionRegex.Matches(delimitersString);

            if (mc.Count > 0)
            {
                foreach (Match m in mc)
                {
                    for (int i = 0; i < m.Groups.Count; i++)
                    {
                        delimiters.Add(m.Groups[i].Value);
                    }
                }
            }
            else
            {
                delimiters.Add(delimitersString);
            }

            return delimiters;
            
        }

        private static void ValidateInput(IEnumerable<int> args)
        {
            var negativeNumbers = args.Where(i => i < 0).ToList();
            if (negativeNumbers.Any())
            {
                throw new ArgumentException("Negative numbers are not allowed: " + String.Join(DefaultDelimiter, negativeNumbers));
            }
        }

        private List<int> RemoveInvalidNumbers(IEnumerable<int> numbers)
        {
            return numbers.Where(x => x < 1000).ToList();
            ;
        }
    }
}
