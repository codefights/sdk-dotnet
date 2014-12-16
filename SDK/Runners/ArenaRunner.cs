﻿namespace CodeFights.SDK.Runners
{
    using System;
    using System.IO;

    using CodeFights.SDK.Protocol;
    using CodeFights.SDK.SampleFighters;

    public class ArenaRunner : IFighterRunner
    {
        private readonly string[] _args;

        private readonly TextWriter _outStream;

        private IFighter _fighter1;

        private IFighter _fighter2;

        private string _nameFighter1;

        private string _nameFighter2;

        private ArenaCommentator _arenaCommentator;

        private int _roundLimit;

        public ArenaRunner(string[] args, TextWriter outStream, int roundLimit = 100)
        {
            _args = args;
            _outStream = outStream;
            _roundLimit = roundLimit;
        }

        public void Run(IFighter fighter)
        {
            ArrangeFight(fighter, _args);
            StartFight();
        }

        private void StartFight()
        {
            if (_fighter1 == null || _fighter2 == null)
            {
                throw new ArgumentException("Must be 2 fighters!");
            }

            _arenaCommentator = new ArenaCommentator(_outStream);
            _arenaCommentator.SetFighterNames(_nameFighter1, _nameFighter2);

            IFighterMove lastMoveFighter1 = null;
            IFighterMove lastMoveFighter2 = null;

            int lastScoreFighter1 = 0;
            int lastScoreFighter2 = 0;

            int lifePointsFighter1 = GameScoringRules.LifePointsPerFight;
            int lifePointsFighter2 = GameScoringRules.LifePointsPerFight;

            while (_roundLimit > 0 && lifePointsFighter1 > 0 && lifePointsFighter2 > 0)
            {
                var moveFighter1 = _fighter1.MakeNextMove(lastMoveFighter2, lastScoreFighter1, lastScoreFighter2);

                if (GameScoringRules.IsInvalidMove(moveFighter1))
                {
                    throw new ArgumentException(_nameFighter1 + " made an illegal move: " + moveFighter1);
                }

                var moveFighter2 = _fighter2.MakeNextMove(lastMoveFighter1, lastScoreFighter2, lastScoreFighter1);

                if (GameScoringRules.IsInvalidMove(moveFighter2))
                {
                    throw new ArgumentException(_nameFighter2 + " made an illegal move: " + moveFighter2);
                }

                lastScoreFighter1 = GameScoringRules.CalculateScore(moveFighter1.AttackedAreas, moveFighter2.BlockedAreas);
                lastScoreFighter2 = GameScoringRules.CalculateScore(moveFighter2.AttackedAreas, moveFighter1.BlockedAreas);

                _arenaCommentator.DescribeRound(moveFighter1, moveFighter2, lastScoreFighter1, lastScoreFighter2);

                lifePointsFighter1 -= lastScoreFighter2;
                lifePointsFighter2 -= lastScoreFighter1;

                lastMoveFighter1 = moveFighter1;
                lastMoveFighter2 = moveFighter2;

                _roundLimit--;
            }

            _arenaCommentator.GameOver(lifePointsFighter1, lifePointsFighter2);
        }

        private void ArrangeFight(IFighter fighter, string[] args)
        {
            _fighter1 = fighter;
            _nameFighter1 = "Your bot";
            _nameFighter2 = _args[1];

            if ("boxer".Equals(args[1], StringComparison.InvariantCultureIgnoreCase))
            {
                _fighter2 = new Boxer();
            }
            else if ("kickboxer".Equals(args[1], StringComparison.InvariantCultureIgnoreCase))
            {
                _fighter2 = new Kickboxer();
            }
            else if ("human".Equals(args[1], StringComparison.InvariantCultureIgnoreCase))
            {
                _fighter2 = new Human();
            }
            else if ("remote".Equals(args[1], StringComparison.InvariantCultureIgnoreCase))
            {
                _fighter2 = _fighter1 = new RemoteFighterDecorator(fighter, args);
                _nameFighter2 = "Opponent bot";
            }
            else
            {
                throw new NotSupportedException("unrecognized built-in bot: " + args[1]);
            }
        }
    }
}
