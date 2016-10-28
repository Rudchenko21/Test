using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.BLL.Interfaces;

namespace Task.BLL.Models
{
    public class TxtWriter
    {
        public static void WriteToFile(string filename, string Text)
        {
            try
            {
                using (StreamWriter myWriter = new StreamWriter(filename))
                {
                    myWriter.WriteLine(Text);
                }
            }
            catch (Exception e)
            {
                //ValidationException.WriteToFile(e);
            }
        }
        public static void WriteToEndFile(string filename, string Text)
        {
            try
            {
                using (StreamWriter myWriter = new StreamWriter(File.Open(filename, FileMode.Append)))
                {
                    myWriter.WriteLine(Text);
                }
            }
            catch (Exception e)
            {
                //ValidationException.WriteToFile(e);
            }
        }
    }
}
