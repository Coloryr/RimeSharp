using System.Runtime.InteropServices;

namespace RimeSharp;

[StructLayout(LayoutKind.Sequential)]
public struct RimeStringSlice
{
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string str;
    public long length;
}
