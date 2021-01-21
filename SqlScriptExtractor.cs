using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SqlScriptExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the script file path :");
            var sqlInputFilePath = Console.ReadLine();

            Console.WriteLine("Enter the output folder path");
            var outputFolderPath = Console.ReadLine();


            Console.WriteLine("Parsing script file..");

            ParseSql(sqlInputFilePath, outputFolderPath);

            Console.WriteLine("Parsing Completed");

            Console.ReadLine();
        }

        static void ParseSql(string sqlInputFilePath, string outputFolderPath)
        {
            List<string> sqlTexts = new List<string>();

            sqlTexts = new List<string>(File.ReadAllLines(sqlInputFilePath));

            var filename = "";

            int idx = 1;

            foreach (var sqlText in sqlTexts)
            {
                var text = GetObjectTextMatch(sqlText, idx);

                if (text != "")
                {
                    filename = text;

                    if (idx == 1)
                    {
                        File.AppendAllLines(outputFolderPath + "\\" + filename, new[] { sqlText });
                        ++idx;
                    }
                    else
                    {
                        File.AppendAllLines(outputFolderPath + "\\" + filename, new[] { sqlText });
                        ++idx;
                    }
                }
                else if (filename != "")
                {
                    File.AppendAllLines(outputFolderPath + "\\" + filename, new[] { sqlText });
                }
                else
                { 
                
                }
            }
        }

        static string GetObjectTextMatch(string sqlText, int counter = 0)
        {
            string strRegex = @"^/\*\*\*\*\*\*\sObject\:";

            string strRegex1 = @"\[\w+\]";

            bool multipartFile = false;

            string filename = "";

            Regex myRegex = new Regex(strRegex, RegexOptions.None);

            foreach (Match myMatch in myRegex.Matches(sqlText))
            {
                if (myMatch.Success)
                {
                    Regex myRegex1 = new Regex(strRegex1, RegexOptions.None);

                    foreach (Match myMatch1 in myRegex1.Matches(sqlText))
                    {
                        if (myMatch1.Success)
                        {
                            if (filename == "")
                            {
                                filename = myMatch1.Value;
                            }
                            else
                            {
                                filename += "." + myMatch1.Value;
                                multipartFile = true;
                            }
                        }
                    }
                }
            }

            if (filename != "")
            {
                if (!multipartFile)
                {
                    filename = "[" + counter + "]." + filename;
                }

                filename = filename + ".sql";
            }

            return filename;

        }
    }
}
