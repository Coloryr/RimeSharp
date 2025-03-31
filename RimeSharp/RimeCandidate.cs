using System.Runtime.InteropServices;

namespace RimeSharp;

public struct RimeCandidate
{
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string Text;
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string Comment;
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string Reserved;
}
