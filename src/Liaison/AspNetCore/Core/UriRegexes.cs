using System.Text.RegularExpressions;

namespace Liaison.AspNetCore.Core
{
    internal class UriRegexes
    {
        public static readonly Regex TemplateRegex = new Regex( @"(?<templatestring>\{([0-9a-zA-Z\-]+)(?:\}))" );
    }
}