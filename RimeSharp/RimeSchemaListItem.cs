using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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