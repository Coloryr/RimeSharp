using System.Runtime.InteropServices;

namespace RimeSharp;

[StructLayout(LayoutKind.Sequential)]
public struct RimeStatus
{
    public readonly int DataSize;

    // v0.9
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string SchemaId;
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string SchemaName;
    public bool IsDisabled;
    public bool IsComposing;
    public bool IsAsciiMode;
    public bool IsFullShape;
    public bool IsSimplified;
    public bool IsTraditional;
    public bool IsAsciiPunct;

    public RimeStatus()
    {
        DataSize = Marshal.SizeOf<RimeStatus>() - sizeof(int);
    }
}
