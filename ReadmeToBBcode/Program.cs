using System.Text.RegularExpressions;
using System.CommandLine;
using System.Text;
using System.IO;

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

            //Add a Line Feed before any header line if there is not one.
            outText = Regex.Replace(outText, @"[^\n](\n#+.+?\n)", "\n$1", RegexOptions.Multiline);


            ////Add a Line Feed after any header line if there is not one.
            outText = Regex.Replace(outText, @"(\n#+.+\n)(?!\n)", "$1\n", RegexOptions.Multiline);

            outText = Regex.Replace(outText, @"^# (.+?)$", "[size=4][b]$1[/b][/size]", RegexOptions.Multiline);
            outText = Regex.Replace(outText, @"^## (.+?)$", "[size=3][b]$1[/b][/size]", RegexOptions.Multiline);
            outText = Regex.Replace(outText, @"^### (.+?)$", "[size=2][b]$1[/b][/size]", RegexOptions.Multiline);


            //Code
            outText = Regex.Replace(outText, "```(.+?)```", "[code]$1[/code]", RegexOptions.Singleline);

            outText = ConvertTables(outText);
            return outText;
          
        }

        /// <summary>
        /// Converts all the markdown tables in the source to ascii tables.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        static string ConvertTables(string source)
        {
            MarkdownTableGenerator tableGenerator = new MarkdownTableGenerator();

            StringBuilder output = new StringBuilder();   
            List<string> lines = new List<string>(source.Split('\n'));

            int sequence = 0;
            int lastCount = 0;

            //Create contiguous groups of lines that have the same number of pipes, which are mark down column delimiters.
            var pipeGroups = lines.Select(x=>
            {
                int count = x.Count(x=> x == '|');
                if (lastCount != count)
                {
                    sequence++;
                }

                lastCount = count;

                return new
                {
                    Sequence = sequence,
                    PipeCount = count,
                    Line = x,
                };
            }).GroupBy(x=> x.Sequence)
            .ToList();

            foreach (var group in pipeGroups)
            {
                var groupLines = group.ToList();


                string groupText = string.Join('\n', groupLines.Select(x => x.Line));

                if (groupLines[0].PipeCount == 0)
                {
                    output.AppendLine(groupText);
                }
                else
                {
                    output.AppendLine(tableGenerator.CreateTable(groupText, 80));
                }
            }

            return output.ToString();   

        }
        
    }

}
