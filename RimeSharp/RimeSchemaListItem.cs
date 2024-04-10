using System.Runtime.InteropServices;

namespace RimeSharp;

[StructLayout(LayoutKind.Sequential)]
public struct RimeSchemaListItem
{
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string schema_id;

    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string name;

    public IntPtr reserved;
}