using System.Text.RegularExpressions;
using System.CommandLine;

namespace ReadmeToBBcode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {

                HandleCommandLine(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }

        public static void ConvertToBbCode(string fileName)
        {
            Console.WriteLine(MarkdownToBbCodeOldString(File.ReadAllText(fileName)));
        }

        public static void MarkdownTableToAsciiCommand(string? filename, int maxTableWidth, bool removeCodeBlocks, 
            bool useBbCodeFont)
        {
            StreamReader stream;

            if (filename is null)
            {
                //Read from std input
                 stream = new StreamReader(Console.OpenStandardInput());
            }
            else
            {
                stream = new StreamReader(filename);
            }


            MarkdownTableGenerator tableGenerator = new MarkdownTableGenerator();

            Console.Write(tableGenerator.CreateTable(stream, maxTableWidth, removeCodeBlocks, useBbCodeFont));

            //Console.Write(asciiTable.ConvertTable(stream));

        }


        private static void HandleCommandLine(string[] args)
        {

            Command convertCommand = SetupConvertCommand();
            Command tableCommand = SetupTableCommand();

            RootCommand rootCommand = new()
            {
                convertCommand,
                tableCommand,
            };

            rootCommand.Invoke(args);
        }

        private static Command SetupTableCommand()
        {
            //---------- Table command 
            var tablePathValue = new Option<string>("path", "The path to the file to read.  If not set, the program will use stdin");

            Option<int> maxTableWidthOption = new(
                name: "maxTableWidth",
                getDefaultValue: () => 100,
                description: "The maximum width the table.  Set to 0 to not limit.");

            var useBbCodeFontOption = new Option<bool>("use-bbcode-font", () => true, "If true, wraps the table in the Courier New font");
            useBbCodeFontOption.Arity = ArgumentArity.ZeroOrOne;


            var removeCodeBlockOption = new Option<bool>("remove-code-block", () => true, "Removes and mardown code blocks (```) in the text");
            removeCodeBlockOption.Arity = ArgumentArity.ZeroOrOne;

            var tableCommand = new Command("table", "Creates an ASCII table from markdown table input")
            {
                tablePathValue,
                maxTableWidthOption,
                removeCodeBlockOption,
                useBbCodeFontOption,

            };

            tableCommand.SetHandler((string path, int maxTableWidth, bool removeCodeBlocks, bool useBbCodeFont) =>
            {
                MarkdownTableToAsciiCommand(path, maxTableWidth, removeCodeBlocks, useBbCodeFont);
            }, tablePathValue, maxTableWidthOption, removeCodeBlockOption, useBbCodeFontOption);

            return tableCommand;
        }

        private static Command SetupConvertCommand()
        {
            //---------- Convert options
            var pathValue = new Option<string>("path", "The path to the file");


            var convertCommand = new Command("convert", "Converts markdown to NexusMods format bbcode")
            {
                pathValue,
            };

            convertCommand.SetHandler((string path) =>
            {
                ConvertToBbCode(path);
            }, pathValue);
            return convertCommand;
        }

        static string MarkdownToBbCodeOldString(string source)
        {

            string outText = source.Replace("\r\n", "\n");

            outText = Regex.Replace(outText, @"^# (.+?)$", "[size=4][b]$1[/b][/size]", RegexOptions.Multiline);
            outText = Regex.Replace(outText, @"^## (.+?)$", "[size=3][b]$1[/b][/size]", RegexOptions.Multiline);
            outText = Regex.Replace(outText, @"^### (.+?)$", "[size=2][b]$1[/b][/size]", RegexOptions.Multiline);

            //Code
            outText = Regex.Replace(outText, "```(.+?)```", "[code]$1[/code]", RegexOptions.Singleline);

            return outText;
          
        }
    }
}
