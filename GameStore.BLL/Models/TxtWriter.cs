using System.IO;
using GameStore.BLL.Interfaces;

namespace GameStore.BLL.Models
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
