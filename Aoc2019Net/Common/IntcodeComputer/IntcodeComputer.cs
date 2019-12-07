using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2019Net.Common
{
    internal static class IntcodeComputer
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

        public static IntcodeComputerResult ExecuteProgram(int[] program, IntcodeComputerParameters parameters)
        {
            var inputs = parameters.Inputs;
            var outputs = new List<int>();
            var inputIndex = parameters.InputIndex;
            var halted = false;
            var i = parameters.StartIndex;
            var exit = false;

            for (; i < program.Length && !exit;)
            {
                var command = program[i].ToString().PadLeft(5, '0');
                var opCode = int.Parse(command[^2..]);
                if (opCode == OpCode.Halt)
                {
                    halted = true;
                    break;
                }

                var modes = command[..^2].Select(c => int.Parse(c.ToString())).Reverse().ToArray();

                var x = GetArgument(program, i + 1, modes[0]);
                var y = GetArgument(program, i + 2, modes[1]);

                switch (opCode)
                {
                    case OpCode.Add:
                        program[program[i + 3]] = x + y;
                        i += 4;
                        break;
                    case OpCode.Multiply:
                        program[program[i + 3]] = x * y;
                        i += 4;
                        break;
                    case OpCode.Input:
                        program[program[i + 1]] = inputs[inputIndex++];
                        i += 2;
                        break;
                    case OpCode.Output:
                        outputs.Add(x);
                        i += 2;
                        if (parameters.BreakOnOutput)
                            exit = true;
                        break;
                    case OpCode.JumpIfTrue:
                        i = x != 0 ? y : i + 3;
                        break;
                    case OpCode.JumpIfFalse:
                        i = x == 0 ? y : i + 3;
                        break;
                    case OpCode.LessThan:
                        program[program[i + 3]] = Convert.ToInt32(x < y);
                        i += 4;
                        break;
                    case OpCode.Equal:
                        program[program[i + 3]] = Convert.ToInt32(x == y);
                        i += 4;
                        break;
                }
            }

            return new IntcodeComputerResult(outputs.ToArray(), halted, i, inputIndex);
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
