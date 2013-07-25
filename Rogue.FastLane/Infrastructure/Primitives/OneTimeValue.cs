using System;

namespace Rogue.FastLane.Infrastructure.Primitives
{
    /// <summary>
    /// Abstraction of a value that can be achieved one time and then its default value all the others.
    /// </summary>
    /// <remarks>
    /// Usually used as a offset for multilevel interactions.
    /// </remarks>
    public class OneTimeValue<T>
    {
        private T _value;

        private Func<T> _getValue;

        public T DefaultValue { get; set; }

        public T Value
        {
            get { return this._getValue(); }
            set
            {
                this._getValue =
                    () =>
                    {
                        this._getValue =
                            () => this.DefaultValue;
                        return this._value = value;
                    };
            }
        }

        public void Reset()
        {
            this.Value = this._value;
        }
    }
}