using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.FastLane.Queries.States
{
    public class UniqueKeyQueryState
    {
        public class Level
        {
            public int Index { get; set; }
            public int TotalUsed { get; set; }
            public int TotalOfSpaces { get; set; }            
        }
        
        public UniqueKeyQueryState()
        {
            LevelCount = 2;
            _levels = 
                new[] 
                {
                    new UniqueKeyQueryState.Level { Index = 0 },
                    new UniqueKeyQueryState.Level { Index = 1 },
                };                                           
            
            _gLvls = 
                () => 
                    _levels = new Level[LevelCount];
        }

        private Func<Level[]> _gLvls;

        private Level[] _levels { get; set; }

        public int LevelCount { get; set; }
        
        public int MaxLengthPerNode { get; set; }
        public int MaxIteractionsPerSegment { get; set; }

        public Level Last 
        {
            get { return Levels[LevelCount -  1]; }
        }

        public Level[] Levels
        {
            get 
            {   
                return 
                    (_levels ?? _gLvls()).Length != LevelCount ?
                     _gLvls() : 
                    _levels;
            }
        }
    }
}
