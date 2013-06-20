using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.FastLane.Infrastructure.Mixins
{
    public static class InheritanceMixins
    {
        public static bool IsOrInherits(this object obj, Type genericDef)
        {
            var type = obj.GetType();
            
            Type definitionWithTypedParameters = type;

            while (definitionWithTypedParameters != null)
            {
                if (definitionWithTypedParameters.IsGenericType)
                {
                    if (definitionWithTypedParameters.GetGenericTypeDefinition() == genericDef)
                    { return true; }
                }

                definitionWithTypedParameters = definitionWithTypedParameters.BaseType;
            }

            return false;
        }
    }
}
