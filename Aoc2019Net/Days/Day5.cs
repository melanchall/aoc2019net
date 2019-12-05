using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2019Net.Days
{
    public sealed class Day5 : Day
    {
        private static class Mode
        {
            public const int Position = 0;
            public const int Immediate = 1;
        }

        private static class OpCode
        {
            public const int Add = 1;
            public const int Multiply = 2;
            public const int Input = 3;
            public const int Output = 4;
            public const int JumpIfTrue = 5;
            public const int JumpIfFalse = 6;
            public const int LessThan = 7;
            public const int Equal = 8;
            public const int Halt = 99;
        }

        private const string InputTokensDelimiter = ",";

        protected override object SolvePart1() => GetDiagnosticCode(1);

        protected override object SolvePart2() => GetDiagnosticCode(5);

        private int GetDiagnosticCode(int inputId)
        {
            var numbers = GetInputNumbers(InputTokensDelimiter);
            var outputs = new List<int>();

            for (var i = 0; i < numbers.Length;)
            {
                var command = numbers[i].ToString().PadLeft(5, '0');
                var opCode = int.Parse(command[^2..]);
                if (opCode == OpCode.Halt)
                    break;

                var modes = command[..^2].Select(c => int.Parse(c.ToString())).Reverse().ToArray();

                var x = GetArgument(numbers, i + 1, modes[0]);
                var y = GetArgument(numbers, i + 2, modes[1]);

                switch (opCode)
                {
                    case OpCode.Add:
                        numbers[numbers[i + 3]] = x + y;
                        i += 4;
                        break;
                    case OpCode.Multiply:
                        numbers[numbers[i + 3]] = x * y;
                        i += 4;
                        break;
                    case OpCode.Input:
                        numbers[numbers[i + 1]] = inputId;
                        i += 2;
                        break;
                    case OpCode.Output:
                        outputs.Add(x);
                        i += 2;
                        break;
                    case OpCode.JumpIfTrue:
                        i = x != 0 ? y : i + 3;
                        break;
                    case OpCode.JumpIfFalse:
                        i = x == 0 ? y : i + 3;
                        break;
                    case OpCode.LessThan:
                        numbers[numbers[i + 3]] = Convert.ToInt32(x < y);
                        i += 4;
                        break;
                    case OpCode.Equal:
                        numbers[numbers[i + 3]] = Convert.ToInt32(x == y);
                        i += 4;
                        break;
                }
            }

            return outputs.Last();
        }

        private static int GetArgument(int[] numbers, int parameterIndex, int parameterMode)
        {
            if (parameterIndex >= numbers.Length)
                return -1;

            switch (parameterMode)
            {
                case Mode.Immediate:
                    return numbers[parameterIndex];
                case Mode.Position:
                default:
                    return numbers[parameterIndex] < numbers.Length ? numbers[numbers[parameterIndex]] : -1;
            }
        }
    }
}
