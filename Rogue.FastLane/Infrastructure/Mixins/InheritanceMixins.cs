using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.FastLane.Infrastructure.Mixins
{
    public static class InheritanceMixins
    {
        public static bool IsOrInherits<T>(this object obj)
        {
            return obj.IsOrInherits(typeof(T));
        }

        public static bool IsOrInherits(this object obj, Type def)
        {
            var type = obj.GetType();
            
            Type genericDef = null;
            if (def.IsGenericType)
            {
                genericDef = def.GetGenericTypeDefinition();
            }

            Type definitionWithTypedParameters = type;            

            while (definitionWithTypedParameters != null)
            {
                if (definitionWithTypedParameters.IsGenericType)
                {
                    var pureDefinition = 
                        definitionWithTypedParameters.GetGenericTypeDefinition();
                    if (pureDefinition == def)
                    { return true; }
                    if (def.IsGenericType && genericDef == pureDefinition)
                    { return true; }
                }

                definitionWithTypedParameters = definitionWithTypedParameters.BaseType;
            }

            return false;
        }
    }
}
