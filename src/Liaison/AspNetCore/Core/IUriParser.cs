using System.Collections.Generic;

namespace Liaison.AspNetCore.Core
{
    internal interface IUriParser
    {
        string UriTemplate { get; }
        
        IReadOnlyCollection<string> TemplateFields { get; }

        bool HasRouteParameters { get; }
        
        bool IsMatch(
            string uri );

        IDictionary<string, string> ExtrapolateTemplateValues(
            string uri );
    }
}