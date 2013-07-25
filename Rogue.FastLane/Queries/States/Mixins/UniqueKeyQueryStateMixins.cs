using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rogue.FastLane.Queries.States;
using Rogue.FastLane.Infrastructure.Positioning;

namespace Rogue.FastLane.Queries.States.Mixins
{
    public static class UniqueKeyQueryStateMixins
    {
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
