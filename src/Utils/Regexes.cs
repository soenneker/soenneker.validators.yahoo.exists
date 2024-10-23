using System.Text.RegularExpressions;

namespace Soenneker.Validators.Yahoo.Exists.Utils
{
    internal partial class Regexes
    {
        [GeneratedRegex("s=(?<acrumb>[^;]*)&d")]
        internal static partial Regex Acrumb();

        [GeneratedRegex(@"<input type=""hidden"" value=""(?<sessionIndex>.*)"" name=""sessionIndex"">")]
        internal static partial Regex SessionIndex();
    }
}
