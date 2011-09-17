using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Csg
{
    public interface ISceneParser
    {
        TreeOperation ReadFile(string fileName);
    }
}
