using System;

namespace Liaison.Extensions
{
    internal static class TypeExtensions
    {
        public static bool IsClosingTypeOf(
            this Type source,
            Type openGenericType )
        {
            return source.IsGenericType
                && source.GetGenericTypeDefinition() == openGenericType;
        }

//        public static bool IsClosingTypeOf(
//            this Type source,
//            Type openGenericType,
//            bool interfaceCheck )
//        {
//            if ( source.GetInterfaces().Length > 0 && interfaceCheck )
//            {
//                return source.GetInterfaces()
//                             .Any( t => t.IsClosingTypeOf( openGenericType ) );
//            }
//
//            return source.IsClosingTypeOf( openGenericType );
//        }
    }
}