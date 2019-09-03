using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Liaison.AspNetCore
{
    public class RouteItem : IEquatable<RouteItem>
    {
        public string Route { get; set; }
        public IDictionary<string, object> RequestParameters { get; set; }
        public HttpMethods Method { get; set; }
        public Type RequestType { get; set; }
        public string DataObjectKey { get; set; }
        public RequestDelegate Pipeline { get; set; }
        public WrapResponseDelegate WrapResponseDelegate { get; set; }
        public bool HasRouteParameters { get; set; }

        public bool Equals(
            RouteItem other )
        {
            if ( ReferenceEquals( null, other ) )
            {
                return false;
            }

            if ( ReferenceEquals( this, other ) )
            {
                return true;
            }

            return string.Equals( this.Route, other.Route, StringComparison.OrdinalIgnoreCase ) && this.Method == other.Method;
        }

        public override bool Equals(
            object obj )
        {
            if ( ReferenceEquals( null, obj ) )
            {
                return false;
            }

            if ( ReferenceEquals( this, obj ) )
            {
                return true;
            }

            if ( obj.GetType() != this.GetType() )
            {
                return false;
            }

            return Equals( (RouteItem) obj );
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ( ( this.Route != null ? StringComparer.OrdinalIgnoreCase.GetHashCode( this.Route ) : 0 ) * 397 ) ^ (int) this.Method;
            }
        }

        public static bool operator ==(
            RouteItem left,
            RouteItem right )
        {
            return Equals( left, right );
        }

        public static bool operator !=(
            RouteItem left,
            RouteItem right )
        {
            return !Equals( left, right );
        }
    }
}