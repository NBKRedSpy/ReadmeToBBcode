using System.Text.RegularExpressions;

namespace ReadmeToBBcode
{
    internal static class RegexMarkDownConvert
    {

        public static string MarkdownToBbCodeOldString(string source)
        {

            string outText = source.Replace("\r\n", "\n");

            //Add a Line Feed before any header line if there is not one.
            outText = Regex.Replace(outText, @"[^\n](\n#+.+?\n)", "\n$1", RegexOptions.Multiline);


            ////Add a Line Feed after any header line if there is not one.
            outText = Regex.Replace(outText, @"(\n#+.+\n)(?!\n)", "$1\n", RegexOptions.Multiline);

            outText = Regex.Replace(outText, @"^# (.+?)$", "[size=4][b]$1[/b][/size]", RegexOptions.Multiline);
            outText = Regex.Replace(outText, @"^## (.+?)$", "[size=3][b]$1[/b][/size]", RegexOptions.Multiline);
            outText = Regex.Replace(outText, @"^### (.+?)$", "[size=2][b]$1[/b][/size]", RegexOptions.Multiline);

            //Code
            outText = Regex.Replace(outText, "```(.+?)```", "[code]$1[/code]", RegexOptions.Singleline);

            //# self reference links
            outText = Regex.Replace(outText, @"\[(.+)\]\(#(.+\))", @"'$1'", RegexOptions.Multiline);
            //Convert MD link.  Ignore # section references.
            outText = Regex.Replace(outText, @"\[(.+)\]\([^#](.+)\)", @"[url=$2]($1)", RegexOptions.Multiline);

            outText = Program.ConvertTables(outText);
            return outText;

        }
    }
}