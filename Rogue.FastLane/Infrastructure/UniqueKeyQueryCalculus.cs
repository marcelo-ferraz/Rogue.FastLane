using System;
using Rogue.FastLane.Queries.States;

namespace Rogue.FastLane.Infrastructure
{
    public static class UniqueKeyQueryCalculus
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
                Math.Ceiling(Math.Log(length < 2 ? 2 : length, maxLength)) + 1;
        }
        
        public static UniqueKeyQueryState Calculate4UniqueKey(int totalLength, int maxComparisons)
        {
            if (totalLength == 4196352) { System.Diagnostics.Debugger.Break(); }

            var state =
                new UniqueKeyQueryState();

            state.MaxIteractionsPerSegment =
                maxComparisons;

            state.MaxLengthPerNode =
                GetMaxLength(maxComparisons);

            state.LevelCount = CountLevels(
                totalLength, state.MaxLengthPerNode);
            
            Decimal percentageUsed = 
                totalLength / (decimal) Math.Pow(state.MaxLengthPerNode, state.LevelCount);
            int i = 0;
            for (; i < state.LevelCount; i++)
            {
                state.Levels[i] =
                    new UniqueKeyQueryState.Level
                    {
                        Index = i,
                        TotalOfSpaces = (int)Math.Ceiling(Math.Pow(state.MaxLengthPerNode, i)),
                        TotalUsed = WorkAroundCSharpLimitations(state, percentageUsed, i)
                    };
            }

            state.Levels[--i] =
                new UniqueKeyQueryState.Level
                {
                    Index = i,
                    TotalOfSpaces = (int)Math.Ceiling(Math.Pow(state.MaxLengthPerNode, i)),
                    TotalUsed = totalLength
                };

            return state;
        }

        private static int WorkAroundCSharpLimitations(UniqueKeyQueryState state, decimal percentageUsed, int i)
        {
            var value = decimal.Multiply(
                (decimal) Math.Pow(state.MaxLengthPerNode, i + 1), percentageUsed);

            var integerVal = 
                (int) Math.Round(value);

            var lessThanZeroValue = 
                Decimal.Subtract(value, integerVal);

            var factorOfCorrection =
                (decimal) (1 / Math.Pow(state.MaxLengthPerNode, i + 1));

            return (int)(lessThanZeroValue >= factorOfCorrection ?
                Math.Ceiling(value) :
                integerVal);
        }
    }
}