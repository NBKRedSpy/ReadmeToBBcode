// ============================================================================
//    Author: Kenneth Perkins
//    Date:   May 7, 2021
//    Taken From: http://programmingnotes.org/
//    File:  Utils.cs
//    Description: Handles general utility functions
// ============================================================================
using System;

namespace ReadmeToBBcode
{
    public static class Extensions
    {
        /// <summary>
        /// Takes in a string and splits it into multiple lines (array indices)
        /// of a specified length. If a word is too long to fit on a line,
        /// the entire word gets moved to the next line (the next array index)
        /// </summary>
        /// <param name="text">The text to word wrap</param>
        /// <param name="maxCharactersPerLine">The maximum characters per
        /// line</param>
        /// <returns>A list of strings that contains each line limited by
        /// the specified length</returns>
        public static System.Collections.Generic.List<string> WordWrap(this string text, int maxCharactersPerLine)
        {
            text = text.Trim();
            var result = new System.Collections.Generic.List<string>();
            var words = text.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (text.Length <= maxCharactersPerLine || words.Length == 1)
            {
                result.Add(text);
            }
            else
            {
                foreach (var word in words)
                {
                    var addition = $" {word.Trim()}";
                    var lineIndex = result.Count - 1;
                    var lineCharacters = lineIndex > -1 ? result[lineIndex].Length + addition.Length : 0;

                    if (result.Count < 1 || lineCharacters > maxCharactersPerLine)
                    {
                        // Start new line
                        addition = addition.Trim();
                        result.Add(addition);
                    }
                    else
                    {
                        // Append existing line
                        result[lineIndex] += addition;
                    }
                }
            }
            return result;
        }
    }
}// http://programmingnotes.org/