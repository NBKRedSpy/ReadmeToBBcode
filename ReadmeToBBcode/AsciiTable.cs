using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TextTableFormatter;

namespace ReadmeToBBcode
{
    public class AsciiTable
    {
        public string ConvertTable(StreamReader reader)
        {

            //---- Header
            string? firstLine = reader.ReadLine();

            if(firstLine is null)
            {
                return "";
            }
            string[] headerColumns = MarkDownRowToColumns(firstLine);
            TextTable table = new(headerColumns.Length, TableBordersStyle.HEAVY_TOP_AND_BOTTOM, TableVisibleBorders.ALL);

            //debugging.
            table.SetColumnWidthRange(2, 0, 80);
            
            AddLineToTable(table, firstLine);

            string? line;
            while((line = reader.ReadLine()) is not null)
            {
                AddLineToTable(table, line);
            }

            return table.Render();
        }

        private void AddLineToTable(TextTable table, string line)
        {
            string[] columns = MarkDownRowToColumns(line);

            //Debug
            //columns[columns.Length - 1] = String.Join('\n', columns[columns.Length - 1].WordWrap(50));


            foreach (string column in columns)
            {
                table.AddCell(column);
            }
        }

        private string[] MarkDownRowToColumns(string line)
        {
            //Remove border pipes.
            string converted = line.Trim(' ', '|', '\t');

            return converted.Split('|');
        }

    }

}
