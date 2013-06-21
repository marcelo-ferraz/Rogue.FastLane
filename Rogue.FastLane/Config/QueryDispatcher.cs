using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Rogue.FastLane.Collections.Items;
using Rogue.FastLane.Collections.State;
using Rogue.FastLane.Items;
using Rogue.FastLane.Queries;
using Rogue.FastLane.Queries.Mixins;
using Rogue.FastLane.Collections.Mixins;
using Rogue.FastLane.Queries.Dispatchers;

namespace Rogue.FastLane.Config
{
    public class QueryDispatchmentResolver<TItem>
    {
        private Func<IDispatcher<TItem>> _getDispatcher4UniqueKeyQuery;

        protected internal Func<IDispatcher<TItem>> GetDispatcher4UniqueKeyQuery 
        {
            get 
            {
                return _getDispatcher4UniqueKeyQuery ?? (_getDispatcher4UniqueKeyQuery = 
                    () => 
                        new UniqueKeyDispatcher<TItem>()); 
            }
            set 
            {
                _getDispatcher4UniqueKeyQuery = value;
            } 
        }

        //protected internal Func<TItem, SimpleDispatcher<TItem>> Dispatch4DuplicateKey { get; set; }
        


    }
}