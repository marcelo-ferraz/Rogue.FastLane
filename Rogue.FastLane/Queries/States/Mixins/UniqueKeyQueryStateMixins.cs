using Rogue.FastLane.Infrastructure.Positioning;
using System;

namespace Rogue.FastLane.Queries.States.Mixins
{
    public static class UniqueKeyQueryStateMixins
    {
        /// <summary>
        /// Creates a map with coordinates whether ephemeral or regular.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="ephemeral">States that the coordinate for index will vanish after the first use</param>
        /// <returns></returns>
        public static Coordinates[] AcquireMap(this UniqueKeyQueryState self, bool ephemeral = false)
        {
            Func<Coordinates> getCoordinates =
                ephemeral ? (Func<Coordinates>)
                (() => new EphemeralCoordinates()) :
                () => new Coordinates();

            var coordinates = 
                new Coordinates[self.LevelCount];

            for (int i = 0; i < self.LevelCount; i++)
            {
                coordinates[i] = getCoordinates();
            }

            return coordinates;
        }
    }
}
