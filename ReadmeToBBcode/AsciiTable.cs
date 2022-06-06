using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            string[] headerColumns = firstLine.Split('|');
            TextTable table = new(headerColumns.Length);

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
            foreach (string column in line.Split('|'))
            {
                table.AddCell(column.Trim());
            }
        }

    }

}
