using System.Runtime.InteropServices;

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
