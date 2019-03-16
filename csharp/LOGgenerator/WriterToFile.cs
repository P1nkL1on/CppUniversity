using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace LOGgenerator
{
    public static class WriterToFile
    {
        public static void writedown (List<string> what){
            // Create a string array with the lines of text

            // Set a variable to the My Documents path.
            string mydocpath =
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Write the string array to a new file named "WriteLines.txt".
            int numer = 0;
            foreach (string log in what)
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(mydocpath,"log_"+(++numer)+".txt"))) {
                    outputFile.WriteLine(log);
            }
        }
    }
}
