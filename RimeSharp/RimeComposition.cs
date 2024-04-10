using System.Runtime.InteropServices;

namespace RimeSharp;

[StructLayout(LayoutKind.Sequential)]
public struct RimeComposition
{
    public int length;
    public int cursor_pos;
    public int sel_start;
    public int sel_end;
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string preedit;
}
