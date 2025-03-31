using System.Runtime.InteropServices;

namespace RimeSharp;

[StructLayout(LayoutKind.Sequential)]
public struct RimeSchemaListItem
{
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string SchemaId;

    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string Name;

    public IntPtr Reserved;
}