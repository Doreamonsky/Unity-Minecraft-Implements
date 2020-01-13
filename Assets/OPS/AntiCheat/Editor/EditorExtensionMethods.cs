using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPS.AntiCheat.Editor
{
    internal static class EditorExtensionMethods
    {
        internal static bool IsArrayOrList(this Type listType)
        {
            if (!listType.IsArray)
            {
                if (listType.IsGenericType && listType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        internal static Type GetArrayOrListElementType(this Type listType)
        {
            if (!listType.IsArray)
            {
                if (listType.IsGenericType && listType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    return listType.GetGenericArguments()[0];
                }
                return null;
            }
            return listType.GetElementType();
        }
    }
}
