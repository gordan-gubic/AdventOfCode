#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2020.Days
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Gguc.Aoc.Core.Enums;
    using Gguc.Aoc.Core.Extensions;
    using Gguc.Aoc.Core.Logging;
    using Gguc.Aoc.Core.Models;
    using Gguc.Aoc.Core.Services;
    using Gguc.Aoc.Y2020.Enums;

    public class Day12 : Day
    {
        private List<Instruction<Direction>> _source;
        private List<Instruction<Direction>> _data;

        public Day12(ILog log, IParser parser) : base(log, parser)
        {
            EnableDebug();
            Initialize();
        }

        /// <inheritdoc />
        protected override void InitParser()
        {
            Parser.Year = 2020;
            Parser.Day = 12;
            Parser.Type = ParserFileType.Real;

            _source = Parser.Parse(ConvertInput);
        }

        /// <inheritdoc />
        protected override void ProcessData()
        {
            _data = _source;
        }

        /// <inheritdoc />
        public override void DumpInput()
        {
            DumpData();
        }

        protected override void ComputePart1()
        {
            var position = new Point(0, 0);
            var bearing = 90;

            foreach (var instruction in _data)
            {
                ProcessInstruction1(ref position, ref bearing, instruction);
            }

            position.DumpJson("position");

            Result = position.ManhattanDistance();
        }

        protected override void ComputePart2()
        {
            var position = new Point(0, 0);
            var waypoint = new Point(10, 1);

            foreach (var instruction in _data)
            {
                ProcessInstruction2(ref position, ref waypoint, instruction);
            }

            position.DumpJson("waypoint");
            position.DumpJson("position");

            Result = position.ManhattanDistance();
        }

        private void ProcessInstruction1(ref Point position, ref int bearing, in Instruction<Direction> instruction)
        {
            // Log.DebugLog(ClassId, $"position=[{position}]. Bearing=[{bearing}]. Instruction=[{instruction}]");

            switch (instruction.Operation)
            {
                case Direction.N:
                case Direction.S:
                case Direction.E:
                case Direction.W:
                case Direction.F:
                    Move(ref position, bearing, instruction);
                    break;

                case Direction.L:
                case Direction.R:
                    RotateShip(ref bearing, instruction);
                    break;
            }
        }

        private void ProcessInstruction2(ref Point position, ref Point waypoint, in Instruction<Direction> instruction)
        { 
            // Log.DebugLog(ClassId, $"position=[{position}]. waypoint=[{waypoint}]. Instruction=[{instruction}]");

            switch (instruction.Operation)
            {
                case Direction.N:
                case Direction.S:
                case Direction.E:
                case Direction.W:
                    Move(ref waypoint, 0, instruction);
                    break;

                case Direction.L:
                case Direction.R:
                    RotateWaypoint(position, ref waypoint, instruction);
                    break;

                case Direction.F:
                    MoveShip(ref position, ref waypoint, instruction);
                    break;
            }
        }

        private Direction GetDirection(Direction instructionOperation, in int bearing)
        {
            var dir = new Dictionary<int, Direction>
            {
                [0] = Direction.N,
                [90] = Direction.E,
                [180] = Direction.S,
                [270] = Direction.W,
            };

            switch (instructionOperation)
            {
                case Direction.N: return Direction.N;
                case Direction.S: return Direction.S;
                case Direction.E: return Direction.E;
                case Direction.W: return Direction.W;
                case Direction.F: return dir[bearing];
            }

            return dir[bearing];
        }

        private void RotateShip(ref int bearing, in Instruction<Direction> instruction)
        {
            var start = bearing;
            var value = (instruction.Operation == Direction.R) ? instruction.Argument : -instruction.Argument;
            bearing = (360 + bearing + value) % 360;
            // Log.DebugLog(ClassId, $"Start=[{start}]. Instruction=[{instruction}]. Bearing=[{bearing}]");
        }

        private void RotateWaypoint(Point position, ref Point waypoint, in Instruction<Direction> instruction)
        {
            var relative = new Point(waypoint.X-position.X, waypoint.Y - position.Y);
            var rotation = (instruction.Operation == Direction.R) ? instruction.Argument : -instruction.Argument;
            rotation = (360 + rotation) % 360;

            // Log.DebugLog(ClassId, $"Start=[{waypoint}]. relative=[{relative}]. rotation=[{rotation}]. Instruction=[{instruction}].");

            switch (rotation)
            {
                case 0: break;
                case 90: relative = new Point(+relative.Y, -relative.X); break;
                case 180: relative = new Point(-relative.X, -relative.Y); break;
                case 270: relative = new Point(-relative.Y, +relative.X); break;
            }

            waypoint = new Point(position.X + relative.X, position.Y + relative.Y);

            // Log.DebugLog(ClassId, $"End__=[{waypoint}]. relative=[{relative}] ***");
        }

        private void Move(ref Point position, in int bearing, in Instruction<Direction> instruction)
        {
            var x = 0;
            var y = 0;
            var val = instruction.Argument;
            var current = position;

            var dir = GetDirection(instruction.Operation, bearing);
            
            switch (dir)
            {
                case Direction.N: x = +0; y = +val; break;
                case Direction.S: x = +0; y = -val; break;
                case Direction.E: x = +val; y = +0; break;
                case Direction.W: x = -val; y = +0; break;
            }

            current = new Point(current.X + x, current.Y + y);

            position = current;
        }

        private void MoveShip(ref Point position, ref Point waypoint, in Instruction<Direction> instruction)
        {
            // Log.DebugLog(ClassId, $"Start=[{position}]. waypoint=[{waypoint}]. Instruction=[{instruction}]");
          
            var relative = new Point(waypoint.X-position.X, waypoint.Y - position.Y);
            var speed = instruction.Argument;
            var x = relative.X * speed;
            var y = relative.Y * speed;
            
            position = new Point(position.X + x, position.Y + y);
            waypoint = new Point(waypoint.X + x, waypoint.Y + y);

            // Log.DebugLog(ClassId, $"End__=[{position}]. waypoint=[{waypoint}]. relative=[{relative}] ***");
        }

        private Instruction<Direction> ConvertInput(string input)
        {
            var dir = input.Substring(0, 1);
            var num = input.Remove(0, 1);

            return new Instruction<Direction>
            {
                Operation = Enum.Parse<Direction>(dir),
                Argument = num.ToInt()
            };
        }

        [Conditional("LOG")]
        private void DumpData()
        {
            Log.DebugLog(ClassId);

            //_data[0].Dump("Item");
            //_data.DumpCollection("List");
            // _data.DumpJson("List");
        }
    }
}

#if DUMP

#endif