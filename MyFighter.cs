using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeFights.model;

namespace CodeFights
{
    class MyFighter : IFighter
    {
        public Move MakeNextMove(Move opponentsLastMove, int myLastScore, int opponentsLastScore)
        {
			if (opponentsLastMove == null) 
				return new Move().AddAttack(Area.Nose);
			
			return opponentsLastMove;
        }
    }
}
