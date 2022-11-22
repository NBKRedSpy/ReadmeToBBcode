using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadmeToBBcode
{
    public class MarkdownTableGenerator
    {

        public string CreateTable(string source, int maxWidth = 100, bool removeCodeBlocks = true, bool useBbCodeFont = true)
        {
            using (StringReader reader = new StringReader(source))
            {
                return CreateTable(reader, maxWidth, removeCodeBlocks, useBbCodeFont);
            }
        }

        public string CreateTable(TextReader textReader, int maxWidth = 100, bool removeCodeBlocks = true, bool useBbCodeFont = true)
        {


            var table = new Table();
            table.Border = TableBorder.Markdown;

            string? line = textReader.ReadLine();

            if(line is null)
            {
                return "";
            }

            string[] headerRowColumns = MarkDownRowToColumns(line);
            table.AddColumns(headerRowColumns);

            bool isFirstRow = true;

            while ((line = textReader.ReadLine()) is not null)
            {
                //check for mark down header line
                if (line.Trim('|', '-').Length > 0)
                {
                    if(removeCodeBlocks)
                    {
                        line = line.Replace("```", "");
                    }

                    if(isFirstRow)
                    {
                        isFirstRow = false;
                    }
                    else
                    {
                        table.AddEmptyRow();
                    }

                    string[] row = MarkDownRowToColumns(line);
                    table.AddRow(row);
                    
                }
            }


            string output = TableToString(maxWidth, table);

            output = output.Trim('\r', '\n', ' ', '\t');

            if (useBbCodeFont)
            {
                output = $"[font=Courier New] {Environment.NewLine}{output}{Environment.NewLine}[/font]";
            }

            return output;
            
        }

        private static string TableToString(int maxWidth, Table table)
        {
            StringBuilder outBuilder = new StringBuilder();
            StringWriter stringWriter = new(outBuilder);
            AnsiConsoleOutput output = new(stringWriter);


            //AnsiConsoleFactory factory = new();

            var console = AnsiConsole.Create(new AnsiConsoleSettings()
            {
                Out = output,
            }) ;

            if(maxWidth != 0)
            {
                console.Profile.Width = maxWidth;
            }

            console.Write(table);
            return outBuilder.ToString();
        }

        


        private string[] MarkDownRowToColumns(string line)
        {
            //Remove border pipes.
            string converted = line.Trim(' ', '|', '\t').EscapeMarkup();

            return converted.Split('|').Select(x => x.Trim()).ToArray();
        }
    }
}
