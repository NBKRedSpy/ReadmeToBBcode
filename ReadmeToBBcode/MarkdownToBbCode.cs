using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using static Markdig.Syntax.CodeBlock;

namespace ReadmeToBBcode
{
    public class MarkdownToBbCode
    {
        public string Parse(string markdownSource)
        {

            //MarkdownPipelineBuilder builder = new MarkdownPipelineBuilder()

            //            string source = @"# Some Test Header
            //    body text of the header
            //and more

            //# other header

            //test

            //## sub header
            //";

            //Markdig.Syntax.MarkdownDocument document  = Markdig.Markdown.Parse(source);

            MarkdownDocument document = Markdig.Markdown.Parse(markdownSource);

            StringBuilder sb = new StringBuilder();

            foreach (Block block in document.ToList())
            {

                if(block is HeadingBlock heading)
                {
                    sb.AppendLine($"[size=4][b]{heading.Inline!.ToList()[0].ToString()}[size=4][b]");
                }
                else if (block is CodeBlock codeBlock)
                {

                    sb.Append("[code]");
                    sb.Append(codeBlock.Lines.Lines[0].ToString());
                    sb.AppendLine("[/code]");
                }
                else if (block is ParagraphBlock paragraphBlock)
                {
                    sb.AppendLine(((LiteralInline) paragraphBlock.Inline!.FirstChild!).Content.Text);
                }
                else
                {

                    sb.AppendLine($"---------- {block.GetType().FullName}");
                    //string typeString = block.GetType().Name;
                }


                //ParagraphBlock
                //HeadingBlock

            }


            return sb.ToString();

        }

    }
}
