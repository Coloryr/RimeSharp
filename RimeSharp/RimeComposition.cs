using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RimeSharp;

public struct RimeComposition
{
    public int length;
    public int cursor_pos;
    public int sel_start;
    public int sel_end;
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string preedit;
}
