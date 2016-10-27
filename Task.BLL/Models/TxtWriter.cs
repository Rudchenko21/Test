using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.BLL.Interfaces;

namespace Task.BLL.Models
{
    public class TxtWriter:IWriter
    {
        public void WriteToFile(string filename, string Text)
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
    }
}
