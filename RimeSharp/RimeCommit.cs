using System.Runtime.InteropServices;

namespace RimeSharp;

[StructLayout(LayoutKind.Sequential)]
public struct RimeCommit
{
    public int data_size;
    // v0.9
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string text;
}
