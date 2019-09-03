using System.Collections.Generic;

namespace Liaison.Extensions
{
    internal static class DictionaryExtensions
    {
        public static void CopyTo< TKey, TValue >(
            this IDictionary<TKey, TValue> source,
            IDictionary<TKey, TValue> target,
            bool overwrite = false )
        {
            source.Keys.ForEach( key =>
            {
                if ( target.ContainsKey( key ) && !overwrite )
                {
                    return;
                }

                if ( target.ContainsKey( key ) && overwrite )
                {
                    target[ key ] = source[ key ];
                    return;
                }

                target.Add( key, source[ key ] );
            } );
        }
    }
}