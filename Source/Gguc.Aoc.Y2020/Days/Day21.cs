#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2020.Days;

    public class Day21 : Day
    {
        private List<string> _source;

        private Regex _regex;
        private Dictionary<string, List<string>> _soloAllergens;
        private Dictionary<List<string>, List<string>> _multipleAllergens;
        private Dictionary<string, string> _uniqueAllergens;
        private HashSet<string> _ingredients;
        private List<string> _allingredients;

        public Day21(ILog log, IParser parser) : base(log, parser)
        {
            EnableDebug();
            Initialize();
        }

        /// <inheritdoc />
        protected override void InitParser()
        {
            Parser.Year = 2020;
            Parser.Day = 21;
            Parser.Type = ParserFileType.Real;

            var pattern = @"^([\w ]+)\(contains ([\w, ]+)\)$";
            _regex = new Regex(pattern, RegexOptions.Compiled);

            _source = Parser.Parse();
        }

        /// <inheritdoc />
        protected override void ProcessData()
        {
            _soloAllergens = new Dictionary<string, List<string>>();
            _multipleAllergens = new Dictionary<List<string>, List<string>>();
            _uniqueAllergens = new Dictionary<string, string>();
            _ingredients = new HashSet<string>();
            _allingredients = new List<string>();

            foreach (var line in _source)
            {
                _ProcessData(line);
            }


            DetectAllergens();
        }

        /// <inheritdoc />
        public override void DumpInput()
        {
            DumpData();
        }

        protected override void ComputePart1()
        {
            var countIngredients = _ingredients.Count;
            var countAllergens = _uniqueAllergens.Count;
            var countAllIngredients = _allingredients.Count;

            Info($"Before... Ingredients: {countIngredients}, Allergens: {countAllergens},  Total Ingredients: {countAllIngredients}");

            RemoveAllergens();

            Info($"After... Total Ingredients: {_allingredients.Count}");

            Result = _allingredients.Count;
        }

        protected override void ComputePart2()
        {
            var aKeys = _uniqueAllergens.Keys.ToList();
            aKeys.Sort();

            var sb = new StringBuilder();
            foreach (var key in aKeys)
            {
                sb.Append($"{_uniqueAllergens[key]},");
            }

            Log.Warn(sb.ToString().Trim(','));
        }

        private void DetectAllergens()
        {
            while (_soloAllergens.Count > 0)
            {
                foreach (var key in _soloAllergens.Keys.ToList())
                {
                    DetectAllergen(key);
                }
            }
        }

        private void RemoveAllergens()
        {
            foreach (var alg in _uniqueAllergens.Values)
            {
                _allingredients.RemoveAll(x => x == alg);
            }
        }

        private void DetectAllergen(string key)
        {
            var ingredients = _soloAllergens[key];

            if (ingredients.Count == 1)
            {
                _uniqueAllergens[key] = ingredients[0];
                CleanIngredients(key, ingredients[0]);
                return;
            }

            foreach (var multipleAllergen in _multipleAllergens.Where(x => x.Key.Contains(key)))
            {
                ingredients = ingredients.Intersect(multipleAllergen.Value).ToList();
            }

            if (ingredients.Count == 1)
            {
                _uniqueAllergens[key] = ingredients[0];
                CleanIngredients(key, ingredients[0]);
                return;
            }

            _soloAllergens[key] = ingredients;
        }

        private void CleanIngredients(string alg, string ing)
        {
            _soloAllergens.Remove(alg);

            foreach (var a in _soloAllergens)
            {
                a.Value.Remove(ing);
            }

            foreach (var a in _multipleAllergens)
            {
                a.Value.Remove(ing);
            }

            foreach (var key in _multipleAllergens.Keys.ToList())
            {
                if (!key.Contains(alg))
                {
                }

                var newKey = key.ToList();
                newKey.Remove(alg);
                var newVal = _multipleAllergens[key];
                _multipleAllergens.Remove(key);

                if (newKey.Count == 0) continue;

                if (newKey.Count == 1 && !_soloAllergens.ContainsKey(newKey[0]))
                {
                    _soloAllergens.Add(newKey[0], newVal);
                    continue;
                }

                _multipleAllergens.Add(newKey, newVal);
            }
        }

        private void _ProcessData(string input)
        {
            var m = _regex.Match(input);

            var values = m.Groups[1].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var keys = m.Groups[2].Value.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var value in values)
            {
                _ingredients.Add(value);
                _allingredients.Add(value);
            }

            if (keys.Length == 1)
            {
                _soloAllergens[keys[0]] = values.ToList();
                return;
            }

            _multipleAllergens[keys.ToList()] = values.ToList();
        }

        [Conditional("LOG")]
        private void DumpData()
        {
            if (!Log.EnableDebug) return;

            Debug();

            Print(_soloAllergens);
            Print(_multipleAllergens);
            _ingredients.DumpJson("_ingredients");
            _uniqueAllergens.DumpJson("_uniqueAllergens");
        }

        private void Print<TK, TV>(IDictionary<TK, TV> dict)
        {
            foreach (var kv in dict)
            {
                var key = kv.Key.ToJson();
                var value = "";

                if (kv.Value is ICollection a) value = a.Count.ToString();
                else value = kv.Value.ToString();

                Console.WriteLine($"[{key}] = {value}");
            }
        }
    }

#if DUMP

#endif