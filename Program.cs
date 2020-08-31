using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Help
{
    internal static class OddEvenBinaryProgram
    {

        public static Dictionary<int, bool> EvenCache = new Dictionary<int, bool>();

        private static void Main(string[] args)
        {
            var result = GetInput()
                .TransformInputToArray()
                .TransformTo2Darray()
                .ResetAllEvenNumbers()
                .WalkThroughTheNode();

            Console.WriteLine($"The Maximum Total Sum Of Odd Numbers From Top To Bottom Is:  {result}");
            Console.ReadKey();
        }

        private static string GetInput()
        {
            const string input = @" 215
192 124
117 269 442
218 836 347 235
320 805 522 417 345
229 601 728 835 133 124
248 202 277 433 207 263 257
359 464 504 528 516 716 871 182
461 441 426 656 863 560 380 171 923
381 348 573 533 448 632 387 176 975 449
223 711 445 645 245 543 931 532 937 541 444
330 131 333 928 376 733 017 778 839 168 197 197
131 171 522 137 217 224 291 413 528 520 227 229 928
223 626 034 683 839 052 627 310 713 999 629 817 410 121
924 622 911 233 325 139 721 218 253 223 107 233 230 124 233 ";
            return input;
        }

        private static string[] TransformInputToArray(this string input)
        {
            return input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static int[,] TransformTo2Darray(this string[] arrayOfRowsByNewlines)
        {
            var tableHolder = new int[arrayOfRowsByNewlines.Length, arrayOfRowsByNewlines.Length + 1];

            for (var row = 0; row < arrayOfRowsByNewlines.Length; row++)
            {
                var eachCharactersInRow = arrayOfRowsByNewlines[row].ExtractNumber();

                for (var column = 0; column < eachCharactersInRow.Length; column++)
                    tableHolder[row, column] = eachCharactersInRow[column];
            }
            return tableHolder;
        }

        private static int[] ExtractNumber(this string rows)
        {
            return
                Regex
                    .Matches(rows, "[0-9]+")
                    .Cast<Match>()
                    .Select(m => int.Parse(m.Value)).ToArray();
        }


        private static int[,] ResetAllEvenNumbers(this int[,] tableHolder)
        {
            var length = tableHolder.GetLength(0);
            for (var i = 0; i < length; i++)
            {
                for (var j = 0; j < length; j++)
                {
                    if (tableHolder[i, j] == 0) continue;
                    if (IsEven(tableHolder[i, j]))
                        tableHolder[i, j] = 0;
                }
            }
            return tableHolder;
        }


        private static int WalkThroughTheNode(this int[,] tableHolder)
        {
            var tempresult = tableHolder;
            var length = tableHolder.GetLength(0);

            // walking through the Odd node

            for (var i = length - 2; i >= 0; i--)
            {
                for (var j = 0; j < length; j++)
                {
                    var c = tempresult[i, j];
                    var a = tempresult[i + 1, j];
                    var b = tempresult[i + 1, j + 1];
                    if ((!IsEven(c) && !IsEven(a)) || (!IsEven(c) && !IsEven(b)))
                        tableHolder[i, j] = c + Math.Max(a, b);
                }
            }
            return tableHolder[0, 0];
        }

        public static bool IsEven(this int number)
        {
            // Test whether the parameter is a Even number.
            if (EvenCache.ContainsKey(number))
            {
                bool value;
                EvenCache.TryGetValue(number, out value);
                return value;
            }
            if ((number & 1) == 0)
            {
                if (number == 2)
                {
                    if (!EvenCache.ContainsKey(number)) EvenCache.Add(number, true);
                    return true;
                }
                if (!EvenCache.ContainsKey(number)) EvenCache.Add(number, false);
                return false;
            }

            for (var i = 3; i * i <= number; i += 2)
            {
                if (number % i == 0)
                {
                    if (!EvenCache.ContainsKey(number)) EvenCache.Add(number, false);
                    return false;
                }
            }
            var check = number != 1;
            if (!EvenCache.ContainsKey(number)) EvenCache.Add(number, check);
            return check;
        }
    }

}

