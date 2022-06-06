﻿using System.Text.RegularExpressions;
using System.CommandLine;

namespace ReadmeToBBcode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {

                GetCommandLineCommand(args);

                //Console.WriteLine(MarkdownToBbCodeOldstring(File.ReadAllText(args[0])));

                //string output = markdownToBbCode.Parse(File.ReadAllText(@"C:\src\Battletech\BtShowXp\README.md"));

                //string output = markdownToBbCode.Parse(File.ReadAllText(args[0]));


                //string output = markdownToBbCode.Parse(File.ReadAllText(@"C:\work\test.md"));


                //File.WriteAllText(@"c:\work\bbcode.txt", output);

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


        private static void GetCommandLineCommand(string[] args)
        {

            //Convert options
            var pathValue = new Option<string>("path", "The path to the file");
            var convertCommand = new Command("convert", "Converts markdown to NexusMods format bbcode")
            {
                pathValue
            };

            convertCommand.SetHandler((string path) =>
                {
                    ConvertToBbCode(path);
                },pathValue);

            //Table command 

            var tableCommand= new Command("table", "Creates an ASCII table from markdown table input");

            tableCommand.SetHandler(() =>
            {
                Console.WriteLine("hit table");
            });

            RootCommand rootCommand = new()
            {
                convertCommand,
                tableCommand,
            };

            rootCommand.Invoke(args);
        }

        private static void ConvertTable()
        {
            throw new NotImplementedException();
        }

        static string MarkdownToBbCodeOldString(string source)
        {

            string outText = source.Replace("\r\n", "\n");

            outText = Regex.Replace(outText, @"^# (.+?)$", "[size=4][b]$1[/b][/size]", RegexOptions.Multiline);
            outText = Regex.Replace(outText, @"^## (.+?)$", "[size=3][b]$1[/b][/size]", RegexOptions.Multiline);
            outText = Regex.Replace(outText, @"^### (.+?)$", "[size=2][b]$1[/b][/size]", RegexOptions.Multiline);

            //Code
            outText = Regex.Replace(outText, "```(.+?)```", "[code]$1[/code]", RegexOptions.Singleline);
            //links


            return outText;
            //File.WriteAllText(@"C:\work\netout.txt", outText);

            //[url= http://www.google.com]display stuff[/url]
            //[img]http://blah.png[/img]

            //class Test
            //{
            //    public int val;
            //}
        }
    }
}
