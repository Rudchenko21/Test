using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Task.BLL.Models
{
    public class TxtWriter
    {
        public static void WriteToFile(string filename, string Text)
        {
                using (StreamWriter myWriter = new StreamWriter(filename))
                {
                    myWriter.WriteLine(Text);
                }
        }
        public static void WriteToEndFile(string filename, string Text)
        {
                using (StreamWriter myWriter = new StreamWriter(File.Open(filename, FileMode.Append)))
                {
                    myWriter.WriteLine(Text);
                }
        }
    }
}
