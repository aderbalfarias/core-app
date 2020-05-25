using System.Collections.Generic;

namespace CoreApp.Domain.CSharp8
{
    public static class UsingDeclarations
    {
        public static int WriteLinesToFile(IEnumerable<string> lines)
        {
            using var file = new System.IO.StreamWriter("WriteLines2.txt");

            // Notice how we declare skippedLines after the using statement.
            int skippedLines = 0;

            foreach (string line in lines)
            {
                if (!line.Contains("Second"))
                    file.WriteLine(line);
                else
                    skippedLines++;
            }

            // Notice how skippedLines is in scope here.
            return skippedLines;

            // file is disposed here
        }

        public static int WriteLinesToFileV2(IEnumerable<string> lines)
        {
            // We must declare the variable outside of the using block
            // so that it is in scope to be returned.
            int skippedLines = 0;

            using (var file = new System.IO.StreamWriter("WriteLines2.txt"))
                foreach (string line in lines)
                    if (!line.Contains("Second"))
                        file.WriteLine(line);
                    else
                        skippedLines++;
                 
            // file is disposed here

            return skippedLines;
        }
    }
}
