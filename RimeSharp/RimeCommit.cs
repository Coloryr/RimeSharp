using System.Runtime.InteropServices;

namespace RimeSharp;

[StructLayout(LayoutKind.Sequential)]
public struct RimeCommit
{
    public readonly int DataSize;

    // v0.9
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string text;

    public RimeCommit()
    {
        DataSize = Marshal.SizeOf<RimeCommit>() - sizeof(int);
    }
}
