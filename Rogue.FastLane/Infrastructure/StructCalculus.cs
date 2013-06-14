using System;
using Rogue.FastLane.Queries.State;

namespace Rogue.FastLane.Infrastructure
{
    public static class StructCalculus
    {
        public static int GetMaxComparisons(int totalLength)
        {
            return (int)
                (2 * Math.Log(totalLength - 1, 2));
        }

        public static int GetMaxLength(int maxComparisons)
        {
            return (int) // the inverse of 2*log_2(qtd - 1) 
                Math.Round(Math.Pow(2, (maxComparisons / 2)) + 1);
        }

        public static int CountLevels(int length, int maxLength)
        {
            #region explanation
            /* 
                to get the level count, I get the inverse of lvlCount^maxLenght = totalLength, 
                obs.: logarithm of anything smaller than 2, will result in 0, wich is not the desired behaviour, the minimum is 1
             */
            #endregion
            return (int) 
                Math.Ceiling(Math.Log(length < 2 ? length : 2, maxLength)) + 1;
        }
        
        public static UniqueKeyQueryState Calculate(int totalLength, int maxComparisons)
        {
            var state =
                new UniqueKeyQueryState();

            state.MaxIteractionsPerSegment =
                maxComparisons;

            state.MaxLengthPerNode =
                GetMaxLength(maxComparisons);

            state.LevelCount = CountLevels(
                totalLength, state.MaxLengthPerNode);

            int j = state
                .LevelCount + 1;
            for (int i = 0; i < state.LevelCount; i++)
            {
                state.Levels[i] =
                    new UniqueKeyQueryState.Level
                    {
                        Index = i,
                        TotalOfSpaces = (int)Math.Ceiling(Math.Pow(state.MaxLengthPerNode, i)),
                        TotalUsed = (int)Math.Ceiling(Math.Pow(totalLength, 1 / j))
                    };
                j--;
            }

            return state;
        }
    }
}