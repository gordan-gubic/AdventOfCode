#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2020.Days
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using Gguc.Aoc.Core.Enums;
    using Gguc.Aoc.Core.Extensions;
    using Gguc.Aoc.Core.Logging;
    using Gguc.Aoc.Core.Services;

    public class Day22 : Day
    {
        private List<string> _source;

        private Queue<int> _player1;
        private Queue<int> _player2;

        public Day22(ILog log, IParser parser) : base(log, parser)
        {
            EnableDebug();
            Initialize();
        }

        /// <inheritdoc />
        protected override void InitParser()
        {
            Parser.Year = 2020;
            Parser.Day = 22;
            Parser.Type = ParserFileType.Example;

            _source = Parser.Parse();
        }

        /// <inheritdoc />
        protected override void ProcessData()
        {
            _player1 = new Queue<int>();
            _player2 = new Queue<int>();

            _ProcessData();
        }

        /// <inheritdoc />
        public override void DumpInput()
        {
            DumpData();
        }

        protected override void ComputePart1()
        {
            var p1deck = new Queue<int>(_player1);
            var p2deck = new Queue<int>(_player2);

            var winner = PlayGame1(p1deck, p2deck);

            Info($"Winner: {winner.winner}.");

            Max(CountScore(winner.deck));
        }

        protected override void ComputePart2()
        {
            var p1deck = new Queue<int>(_player1);
            var p2deck = new Queue<int>(_player2);

            var winner = PlayGame2(p1deck, p2deck);

            Info($"Winner: {winner.winner}.");

            Max(CountScore(winner.deck));
        }

        private (int winner, Queue<int> deck) PlayGame1(Queue<int> p1deck, Queue<int> p2deck)
        {
            var turn = 0;
            while (p1deck.Count > 0 && p2deck.Count > 0)
            {
                turn++;
                var p1Card = p1deck.Dequeue();
                var p2Card = p2deck.Dequeue();

                // Debug($"Turn: {turn}. Player1: {p1card}. Player2: {p2card}");

                if (p1Card > p2Card)
                {
                    p1deck.Enqueue(p1Card);
                    p1deck.Enqueue(p2Card);
                }
                else
                {
                    p2deck.Enqueue(p2Card);
                    p2deck.Enqueue(p1Card);
                }
            }

            var turnWinner = DetectWinner(p1deck, p2deck);

            Debug($"Total Turn: {turn}. Player1: {p1deck.Count}. Player2: {p2deck.Count}");
            return turnWinner;
        }

        private (int winner, Queue<int> deck) PlayGame2(Queue<int> p1deck, Queue<int> p2deck)
        {
            var repetitions = new HashSet<string>();
            var roundWinner = 0;
            var turn = 0;

            while (p1deck.Count > 0 && p2deck.Count > 0)
            {
                turn++;

                var uniqueDeck = GetUniqueDeck(p1deck, p2deck);
                if (repetitions.Contains(uniqueDeck))
                {
                    return (1, null);
                }
                else
                {
                    repetitions.Add(uniqueDeck);
                }

                var p1Card = p1deck.Dequeue();
                var p2Card = p2deck.Dequeue();

                Debug($"... Turn: {turn}. Player1: {p1Card}. Player2: {p2Card}");

                if (p1Card <= p1deck.Count && p2Card <= p2deck.Count)
                {
                    Debug("Subgame Begin ...");

                    var p1SubDeck = CreateDeck(p1deck, p1Card);
                    var p2SubDeck = CreateDeck(p2deck, p2Card);

                    roundWinner = PlayGame2(p1SubDeck, p2SubDeck).winner;
                    
                    Debug("Subgame End ...");
                }
                else
                {
                    roundWinner = DetechHigh(p1Card, p2Card);
                }

                if (roundWinner == 1)
                {
                    p1deck.Enqueue(p1Card);
                    p1deck.Enqueue(p2Card);
                }
                else
                {
                    p2deck.Enqueue(p2Card);
                    p2deck.Enqueue(p1Card);
                }
            }

            var turnWinner = DetectWinner(p1deck, p2deck);

            Debug($"Total Turn: {turn}. Player1: {p1deck.Count}. Player2: {p2deck.Count}");
            return turnWinner;
        }

        private string GetUniqueDeck(IEnumerable<int> p1deck, IEnumerable<int> p2deck)
        {
            var sb = new StringBuilder();

            foreach (var i in p1deck)
            {
                sb.Append((char)(i + 70));
            }
            sb.Append("-");

            foreach (var i in p2deck)
            {
                sb.Append((char)(i + 70));
            }

            // Debug($"UniqueDeck: {sb}");
            return sb.ToString();
        }

        private (int, Queue<int>) DetectWinner(Queue<int> p1deck, Queue<int> p2deck)
        {
            if (p1deck.Count > 0)
            {
                return (1, p1deck);
            }

            return (2, p2deck);
        }

        private int DetechHigh(in int p1Card, in int p2Card) => p1Card > p2Card ? 1 : 2;

        private Queue<int> CreateDeck(IEnumerable<int> deck, in int card)
        {
            var oldDeck = new Queue<int>(deck);
            var newDeck = new Queue<int>();

            for (int i = 0; i < card; i++)
            {
                newDeck.Enqueue(oldDeck.Dequeue());
            }

            return newDeck;
        }

        private long CountScore(IReadOnlyCollection<int> player)
        {
            if (player.Count == 0) return 0L;

            var sum = 0L;
            var list = player.Reverse().ToList();

            for (int i = 0; i < list.Count(); i++)
            {
                sum += list[i] * (i + 1);
            }

            return sum;
        }

        private void _ProcessData()
        {
            var segment = 0;
            foreach (var line in _source)
            {
                if (line.IsWhitespace()) { segment++; continue; }

                if (line.StartsWith("Player")) continue;

                if (segment == 0)
                    _player1.Enqueue(line.ToInt());
                else
                    _player2.Enqueue(line.ToInt());
            }
        }

        [Conditional("LOG")]
        private void DumpData()
        {
            if (!Log.EnableDebug) return;

            Debug();

            _player1.DumpJson("_player1");
            _player2.DumpJson("_player2");
        }
    }
}

#if DUMP
#endif