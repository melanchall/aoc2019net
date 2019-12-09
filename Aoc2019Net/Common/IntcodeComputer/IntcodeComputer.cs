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
            public const int Relative = 2;
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
            public const int RelativeBaseOffset = 9;
            public const int Halt = 99;
        }

        private sealed class Context
        {
            public long RelativeBase { get; set; }
        }

        public static IntcodeComputerResult ExecuteProgram(long[] program, IntcodeComputerParameters parameters)
        {
            var inputs = parameters.Inputs;
            var inputIndex = parameters.InputIndex;
            var instructionPointer = parameters.StartIndex;

            var outputs = new List<long>();
            var halted = false;
            var exit = false;

            var context = new Context();

            for (; instructionPointer < program.Length && !exit;)
            {
                var command = program[instructionPointer].ToString().PadLeft(5, '0');
                var opCode = int.Parse(command[^2..]);
                if (opCode == OpCode.Halt)
                {
                    halted = true;
                    instructionPointer++;
                    break;
                }

                var modes = command[..^2].Select(c => int.Parse(c.ToString())).Reverse().ToArray();

                var x = GetArgument(ref program, instructionPointer + 1, modes[0], context, parameters);
                var y = GetArgument(ref program, instructionPointer + 2, modes[1], context, parameters);

                switch (opCode)
                {
                    case OpCode.Add:
                        SetNumber(ref program, GetAddress((int)program[instructionPointer + 3], modes[2], context), x + y, parameters);
                        instructionPointer += 4;
                        break;
                    case OpCode.Multiply:
                        SetNumber(ref program, GetAddress((int)program[instructionPointer + 3], modes[2], context), x * y, parameters);
                        instructionPointer += 4;
                        break;
                    case OpCode.Input:
                        SetNumber(ref program, GetAddress((int)program[instructionPointer + 1], modes[0], context), inputs[inputIndex++], parameters);
                        instructionPointer += 2;
                        break;
                    case OpCode.Output:
                        outputs.Add(x);
                        instructionPointer += 2;
                        if (parameters.BreakOnOutput)
                            exit = true;
                        break;
                    case OpCode.JumpIfTrue:
                        instructionPointer = x != 0 ? (int)y : instructionPointer + 3;
                        break;
                    case OpCode.JumpIfFalse:
                        instructionPointer = x == 0 ? (int)y : instructionPointer + 3;
                        break;
                    case OpCode.LessThan:
                        SetNumber(ref program, GetAddress((int)program[instructionPointer + 3], modes[2], context), Convert.ToInt32(x < y), parameters);
                        instructionPointer += 4;
                        break;
                    case OpCode.Equal:
                        SetNumber(ref program, GetAddress((int)program[instructionPointer + 3], modes[2], context), Convert.ToInt32(x == y), parameters);
                        instructionPointer += 4;
                        break;
                    case OpCode.RelativeBaseOffset:
                        context.RelativeBase += x;
                        instructionPointer += 2;
                        break;
                    default:
                        instructionPointer++;
                        break;
                }
            }

            return new IntcodeComputerResult(outputs.ToArray(), halted, instructionPointer, inputIndex);
        }

        private static int GetAddress(int baseAddress, int mode, Context context)
        {
            return mode == Mode.Relative ? (int)(baseAddress + context.RelativeBase) : baseAddress;
        }

        private static long GetArgument(ref long[] numbers, int parameterIndex, int parameterMode, Context context, IntcodeComputerParameters parameters)
        {
            switch (parameterMode)
            {
                case Mode.Immediate:
                    return GetNumber(ref numbers, parameterIndex, parameters);
                case Mode.Relative:
                    return GetNumber(ref numbers, (int)(context.RelativeBase + numbers[parameterIndex]), parameters);
                case Mode.Position:
                default:
                    return GetNumber(ref numbers, parameterIndex < numbers.Length ? (int)numbers[parameterIndex] : 0, parameters);
            }
        }

        private static long GetNumber(ref long[] numbers, int index, IntcodeComputerParameters parameters)
        {
            if (index >= numbers.Length)
            {
                if (parameters.ExtendProgram)
                    Array.Resize(ref numbers, index + 1);
                else
                    return -1;
            }

            return numbers[index];
        }

        private static void SetNumber(ref long[] numbers, int index, long number, IntcodeComputerParameters parameters)
        {
            if (index >= numbers.Length)
            {
                if (parameters.ExtendProgram)
                    Array.Resize(ref numbers, (index + 1) * 2);
                else
                    return;
            }

            numbers[index] = number;
        }
    }
}
