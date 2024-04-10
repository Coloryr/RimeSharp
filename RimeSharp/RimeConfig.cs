using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RimeSharp;

[StructLayout(LayoutKind.Sequential)]
public struct RimeConfig
{
    public IntPtr ptr;
}
