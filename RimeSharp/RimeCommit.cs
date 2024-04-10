using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RimeSharp;

[StructLayout(LayoutKind.Sequential)]
public struct RimeCommit
{
    public int data_size;
    // v0.9
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string text;
}
