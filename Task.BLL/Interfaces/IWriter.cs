using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.BLL.Interfaces
{
    public interface IWriter
    {
        void WriteToFile(string filename, string Text);
    }
}
