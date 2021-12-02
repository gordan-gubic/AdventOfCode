#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2020.Days
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Gguc.Aoc.Core.Enums;
    using Gguc.Aoc.Core.Extensions;
    using Gguc.Aoc.Core.Logging;
    using Gguc.Aoc.Core.Services;
    using Gguc.Aoc.Core.Utils;
    using Gguc.Aoc.Y2020.Models;
    using Gguc.Aoc.Y2020.Util;

    public class Day04 : Day
    {
        private List<string> _source;
        private List<Passport> _data;

        public Day04(ILog log, IParser parser) : base(log, parser)
        {
            EnableDebug();
            Initialize();
        }

        /// <inheritdoc />
        protected override void InitParser()
        {
            Parser.Year = 2020;
            Parser.Day = 4;
            Parser.Type = ParserFileType.Test;

            _source = Parser.Parse();
        }

        /// <inheritdoc />
        protected override void ProcessData()
        {
            _data = new List<Passport>();
            var temp = new List<string>();
            var temp2 = new List<string>();
            var buffer = new StringBuilder();

            foreach (var line in _source)
            {
                if (line.IsWhitespace())
                {
                    var x = buffer.ToString().Replace(' ', ',').Trim(',', ' ');
                    temp.Add(x);
                    buffer.Clear();
                }

                buffer.Append(line);
                buffer.Append(",");
            }

            {
                var x = buffer.ToString().Replace(' ', ',').Trim(',', ' ');
                temp.Add(x);
                buffer.Clear();
            }

            // temp.DumpCollection("Temp");

            foreach (var line in temp)
            {
                var segments = line.Split(',', ':');

                for (int i = 0; i < segments.Length; i += 2)
                {
                    buffer.Append($"\"{segments[i]}\":\"{segments[i + 1]}\",");
                }

                var x = buffer.ToString().Trim(',', ' ');
                temp2.Add($"{{{x}}}");
                buffer.Clear();
            }

            // temp2.DumpCollection("Temp2");

            foreach (var line in temp2)
            {
                _data.Add(line.FromJson<Passport>());
            }
        }

        /// <inheritdoc />
        public override void DumpInput()
        {
            Log.DebugLog(ClassId);

            _data[0].Dump("Item");
            // _data.DumpCollection("List");
        }

        protected override void ComputePart1()
        {
            foreach (var passport in _data)
            {
                var isValid = IsValidPassport1(passport);
                Add(isValid);
            }
        }

        protected override void ComputePart2()
        {
            foreach (var passport in _data)
            {
                var isValid = IsValidPassport1(passport) && IsValidPassport2(passport);
                Add(isValid);
            }
        }

        private string Convert(string input)
        {
            return input;
        }

        private bool IsValidPassport1(Passport passport)
        {
            var isValid = true;

            isValid &= passport.byr.IsNotWhitespace();
            isValid &= passport.iyr.IsNotWhitespace();
            isValid &= passport.eyr.IsNotWhitespace();
            isValid &= passport.hgt.IsNotWhitespace();
            isValid &= passport.hcl.IsNotWhitespace();
            isValid &= passport.ecl.IsNotWhitespace();
            isValid &= passport.pid.IsNotWhitespace();

            // isValid &= passport.cid.IsNotWhitespace();

            return isValid;
        }

        private bool IsValidPassport2(Passport passport)
        {
            var isValid = true;

            isValid &= ValidateYear(passport.byr, 1920, 2020);
            isValid &= ValidateYear(passport.iyr, 2010, 2020);
            isValid &= ValidateYear(passport.eyr, 2020, 2030);
            isValid &= ValidateHeight(passport.hgt);
            isValid &= ValidateHairColor(passport.hcl);
            isValid &= ValidateEyeColor(passport.ecl);
            isValid &= ValidatePassportId(passport.pid);

            // isValid &= passport.cid.IsNotWhitespace();

            return isValid;
        }

        private bool ValidateYear(string value, int min, int max)
        {
            var year = value.ToInt();

            return year.IsBetween(min, max);
        }

        private bool ValidateHeight(string value)
        {
            if (value.EndsWith("cm"))
            {
                var height = value.Replace("cm", "").ToInt();
                return height.IsBetween(150, 193);
            }

            if (value.EndsWith("in"))
            {
                var height = value.Replace("in", "").ToInt();
                return height.IsBetween(59, 76);
            }

            return false;
        }

        private bool ValidateHairColor(string value)
        {
            var pattern = @"^#[0-9a-f]{6}$";
            return value.MatchRegex(pattern);
        }

        private bool ValidateEyeColor(string value)
        {
            var pattern = @"^(amb|blu|brn|gry|grn|hzl|oth)$";
            return value.MatchRegex(pattern);
        }

        private bool ValidatePassportId(string value)
        {
            var pattern = @"^\d{9}$";
            return value.MatchRegex(pattern);
        }
    }
}

#if DUMP

#endif