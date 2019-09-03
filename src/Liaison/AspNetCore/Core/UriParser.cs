using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Liaison.AspNetCore.Extensions;

namespace Liaison.AspNetCore.Core
{
    internal class UriParser : IUriParser
    {
        private readonly string uriTemplate;
        private readonly IEnumerable<string> templateFields;
        private readonly Regex inversedUriRegex;

        public UriParser(
            string uriTemplate)
        {
            this.uriTemplate = uriTemplate;
            var groupNames = UriRegexes.TemplateRegex
                                       .Matches( uriTemplate );

            this.HasRouteParameters = groupNames.Count > 0;

            this.templateFields = groupNames.Cast<Group>().Select( x => x.Value );

            // "\/api\/customers\/{id}
            var modifiedRegex = this.templateFields.Aggregate( uriTemplate.Replace( "/", "\\/" )
                                                                          .Replace( "?", "\\?" ),
                                                               (
                                                                   acc,
                                                                   value ) => acc.Replace( value,
                                                                                           $"(?<{value.RemoveBraces()}>[0-9a-zA-Z_-]+)" ) );

            // "\/api\/customers\/(?<id>[0-9a-zA-Z_-+)"
            
            var inversedRegex = $"^{modifiedRegex}$";
            
            this.inversedUriRegex = new Regex( inversedRegex, RegexOptions.IgnoreCase );
        }

        public bool HasRouteParameters { get; }

        public string UriTemplate => this.uriTemplate;
        
        public IReadOnlyCollection<string> TemplateFields => this.templateFields.ToList();

        public bool IsMatch(
            string uri )
        {
            return this.inversedUriRegex.IsMatch( uri );
        }

        public IDictionary<string, string> ExtrapolateTemplateValues(
            string uri )
        {
            var matches = this.inversedUriRegex.Matches( uri );
            var match = matches[ 0 ];
            
            var templateResult = new Dictionary<string, string>();

            foreach ( Group group in match.Groups )
            {
                if ( @group is Match )
                {
                    continue;
                }

                var groupName = @group.Name;
                if ( !templateResult.ContainsKey( groupName ) )
                {
                    templateResult.Add( groupName, string.Empty );
                }

                templateResult[ groupName ] = @group.Value;
            }

            return templateResult;
        }
    }
}