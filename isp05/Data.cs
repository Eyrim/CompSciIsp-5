using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace isp05
{
    public static class Data
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string AddQuotes(string s)
        {
            Regex rx = new Regex(@"[\s|,]");
            
            string toReturn = "";

            for (int i = 0; i < s?.Length; i++)
            {
                if (!(rx.IsMatch(s[i].ToString())))
                {
                    toReturn += s[i];
                }
                else
                {
                    if (s[i] == '.')
                        continue;

                    toReturn += " ";
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Sanitise(string s)
        {
            Regex rx = new Regex(@"[\s|\.]");

            string toReturn = s;

            for (int i = 0; i < toReturn?.Length; i++)
            {
                toReturn = rx.Replace(toReturn, "");
            }

            

            

            return toReturn;
        }

        /// <summary>
        /// Returns a System.Collections.Generic.List of each line in the file, each line is a new list element
        /// </summary>
        /// <param name="filePath">The path of the file to read</param>
        /// <returns>System.Collections.Generic.List (string)</returns>
        public static List<string> ReadFile(string filePath)
        {
            // The list to store the output from the file
            List<string> fileContent = new List<string>();
            
            
            // Garbage collected file reading
            using (StreamReader sr = new StreamReader(filePath))
            {
                // The line that has just been read from file
                string lineRead = "";
                do
                {
                    // Reads the entire current line
                    lineRead = sr.ReadLine();
                    
                    // Adds the next line from the text file to the list to be returned
                    fileContent.Add(item:Convert.ToString(lineRead));
                    
                    // If sr.ReadLine() is null, EOF has been reached
                } while (lineRead != null);
            }
            
            // Remove the header line
            // RemoveAt(0) renumbers the other elements to fit the removal
            fileContent.RemoveAt(index:0);
            
            return fileContent;
        }
    }
}