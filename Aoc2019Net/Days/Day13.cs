using System;
using System.Linq;
using Aoc2019Net.Common;

namespace Aoc2019Net.Days
{
    public sealed class Day13 : Day
    {
        private static class TileIds
        {
            public const int Empty = 0;
            public const int Wall = 1;
            public const int Block = 2;
            public const int Paddle = 3;
            public const int Ball = 4;
        }

        private const string InputTokensDelimiter = ",";

        public override object SolvePart1()
        {
            var program = GetInputNumbers(InputTokensDelimiter);
            var result = IntcodeComputer.ExecuteProgram(program, new IntcodeComputerParameters
            {
                ExtendProgram = true
            });

            return result.Outputs.Where((v, i) => i % 3 == 2).Count(tileId => tileId == TileIds.Block);
        }

        public override object SolvePart2()
        {
            var program = GetInputNumbers(InputTokensDelimiter);
            program[0] = 2;

            var outputMode = 0;

            int x = 0, y = 0;
            int prevX = 0, prevY = 0;
            int paddleY = 0, paddleX = 0;

            var score = 0L;
            var joystickPosition = 0;

            var result = IntcodeComputer.ExecuteProgram(program, new IntcodeComputerParameters
            {
                ExtendProgram = true,
                GetInputValue = () => joystickPosition,
                OnOutput = value =>
                {
                    joystickPosition = 0;

                    switch (outputMode)
                    {
                        case 0:
                            prevX = x;
                            x = (int)value;
                            break;
                        case 1:
                            prevY = y;
                            y = (int)value;
                            break;
                        case 2:
                            if (x == -1 && y == 0)
                                score = value;
                            else switch (value)
                            {
                                case TileIds.Paddle:
                                    paddleY = y;
                                    paddleX = x;
                                    break;
                                case TileIds.Ball:
                                    var dx = x - prevX;
                                    var steps = Math.Abs(paddleY - 1 - y);
                                    joystickPosition = Math.Sign(x + steps * dx - paddleX);
                                    break;

                            }
                            break;
                    }

                    outputMode++;
                    if (outputMode > 2)
                        outputMode = 0;
                }
            });

            return score;
        }
    }
}
