using System;
using System.Collections.Generic;

namespace Liaison.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach< T >(
            this IEnumerable<T> list,
            Action<T> action )
        {
            if ( list == null )
            {
                return;
            }
            
            foreach ( var item in list )
            {
                action( item );
            }
        }
    }
}