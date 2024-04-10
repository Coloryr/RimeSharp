using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RimeSharp;

[StructLayout(LayoutKind.Sequential)]
public struct RimeStatus
{
    public int data_size;
    // v0.9
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string schema_id;
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string schema_name;
    public bool is_disabled;
    public bool is_composing;
    public bool is_ascii_mode;
    public bool is_full_shape;
    public bool is_simplified;
    public bool is_traditional;
    public bool is_ascii_punct;
}
