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
        public string CreateTable(StreamReader textStream, int maxWidth)
        {


            var table = new Table();
            table.Border = TableBorder.Markdown;

            string? line = textStream.ReadLine();

            if(line is null)
            {
                return "";
            }

            string[] headerRowColumns = MarkDownRowToColumns(line);
            table.AddColumns(headerRowColumns);

            bool isFirstRow = true;

            while ((line = textStream.ReadLine()) is not null)
            {
                //check for mark down header line
                if (line.Trim('|', '-').Length > 0)
                {

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

            return TableToString(maxWidth, table);
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
                Ansi = AnsiSupport.No,
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
