using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RimeSharp;

public struct RimeConfigIterator
{
    public IntPtr list;
    public IntPtr map;
    public int index;
    public string key;
    public string path;
}
