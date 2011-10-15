using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Csg
{
    public interface ILightsParser
    {
        Light[] ReadFile(string fileName);
    }
}
