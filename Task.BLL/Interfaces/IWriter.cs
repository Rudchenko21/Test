using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.BLL.Interfaces
{
    public interface IWriter // todo looks like unuseless interface
    {
        void WriteToFile(string filename, string Text);
    }
}
