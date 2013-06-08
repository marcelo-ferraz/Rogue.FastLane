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

        public static int GetOptimumLength(int maxDesiredComparisons)
        {
            return //função inversa para achar a quantidade de interações no pior cenário
                (int)Math.Round(Math.Pow(2, (maxDesiredComparisons / 2)) + 1);
        }

        public static int HowManyLevelsShouldExist(ref int length, int optimumLength, int level = 2)
        {
            length = (int)Math.Ceiling((double)length / optimumLength);

            return length > optimumLength ?
                HowManyLevelsShouldExist(ref length, optimumLength, ++level) :
                level;
        }

        public static Pair[] GetQtdOfNodesPerLevel(int structureLenght, int optimumLength, int levelCount)
        {
            var ammountOfNodesPerLevel = new Pair[levelCount];

            double percentage = structureLenght / Math.Pow(optimumLength, levelCount);

            for (int i = 0; i < ammountOfNodesPerLevel.Length; i++)
            {
                ammountOfNodesPerLevel[i] = new Pair { 
                    Index = i,
                    Length = (int)Math.Ceiling(Math.Pow(optimumLength, i + 1) * percentage) };
            }
            return ammountOfNodesPerLevel;
        }

        public static UniqueKeyQueryState Calculate(UniqueKeyQueryState oldState, int newLenght, int maxDesiredComparisons)
        {
            var newState = oldState != null ? 
                oldState.PassOn() :
                new UniqueKeyQueryState();

            newState.MaxNumberOfIteractions = maxDesiredComparisons;

            newState.MaxLenghtPerSegment = 
                GetOptimumLength(maxDesiredComparisons);

            newState.Length = newLenght;

            var lenght = newLenght;
            
            int levelCount = 
                HowManyLevelsShouldExist(ref lenght, newState.MaxLenghtPerSegment);

            if (newState.Levels == null || newState.Levels.Length != levelCount)
            { 
                newState.Levels = 
                    GetQtdOfNodesPerLevel(newLenght, newState.MaxLenghtPerSegment, levelCount);                 
            }

            return newState;
        }
    }
}
