using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rogue.FastLane.Collections.State;

namespace Rogue.FastLane.Infrastructure
{
    public static class StructCalculus
    {
        public static int GetworstCaseNumberOfComparisons(int listCount)
        {
            return (int)
                (2 * Math.Log(listCount - 1, 2));
        }

        public static int GetOptimumLength(int maxComparisons)
        {
            return (int) // the inverse of 2*log_2(qtd - 1) 
                Math.Round(Math.Pow(2, (maxComparisons / 2)) + 1);
        }

        public static int CountLevels(int length, int maxLenght)
        {
            return (int) // inverse of lvlCount^maxLenght = totalLength
                Math.Ceiling(Math.Log(length != 1 ? length : 2, maxLenght)) + 1;
        }

        public static Pair[] GetQtdOfNodesPerLevel(int structureLenght, int optimumLength, int levelCount)
        {
            var ammountOfNodesPerLevel = new Pair[levelCount];

            double percentage = structureLenght / Math.Pow(optimumLength, levelCount);

            for (int i = 0; i < ammountOfNodesPerLevel.Length; i++)
            {
                ammountOfNodesPerLevel[i] = new Pair
                {
                    Index = i,
                    Length = (int)Math.Ceiling(Math.Pow(optimumLength, i + 1) * percentage)
                };
            }
            return ammountOfNodesPerLevel;
        }

        public static UniqueKeyQueryState Calculate(int totalLength, int maxComparisons)
        {
            var state =
                new UniqueKeyQueryState();

            state.MaxIteractionsPerSegment =
                maxComparisons;

            state.MaxLengthPerNode =
                GetOptimumLength(maxComparisons);

            state.LevelCount =
                CountLevels(totalLength, state.MaxLengthPerNode);

            int j =
                state.LevelCount + 1;
            for (int i = 0; i < state.LevelCount; i++)
            {
                state.Levels[i] =
                    new UniqueKeyQueryState.Level
                    {
                        Index = i,
                        // 
                        TotalOfSpaces = (int)Math.Ceiling(Math.Pow(state.MaxLengthPerNode, i)),
                        TotalUsed = (int)Math.Ceiling(Math.Pow(totalLength, 1 / j))
                    };
                j--;
            }

            return state;
        }
    }
}