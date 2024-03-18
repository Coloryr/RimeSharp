using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RimeSharp;

public struct RimeCandidate
{
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string text;
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string comment;
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string reserved;
}
