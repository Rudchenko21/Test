using System.IO;
using Task.BLL.Interfaces;

namespace Task.BLL.Models
{
    public class TxtWriter:IWriter
    {
        public void WriteToFile(string filename, string text)
        {
            using (StreamWriter myWriter = new StreamWriter(filename))
            {
                myWriter.WriteLine(text);
            } 
        }
    }
}
