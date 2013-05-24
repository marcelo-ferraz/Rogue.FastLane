using System;

namespace Rogue.FastLane.Collections.Items
{
    /// <summary>
    /// Abstraction of a value that can be achieved one time and then its default value all the others.
    /// </summary>
    /// <remarks>
    /// Usually used as a offset for multilevel interactions.
    /// </remarks>
    public class ValueOneTime
    {
        private int _value;

        private Func<int> _getOffset;

        public int DefaultValue { get; set; }

        public int Value
        {
            get { return _getOffset(); }
            set
            {
                _getOffset =
                    () =>
                    {
                        _getOffset =
                            () => DefaultValue;
                        return _value = value;
                    };
            }
        }

        public void Reset()
        {
            Value = _value;
        }
    }
}