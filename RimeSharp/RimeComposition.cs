using System.Runtime.InteropServices;

namespace RimeSharp;

[StructLayout(LayoutKind.Sequential)]
public struct RimeComposition
{
    public int Length;
    public int CursorPos;
    public int SelectStart;
    public int SelectEnd;
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string Preedit;
}
