using System.Runtime.InteropServices;

namespace RimeSharp;

[StructLayout(LayoutKind.Sequential)]
public struct RimeModule
{
    public delegate void DType();
    public delegate IntPtr DType1();

    public int data_size;

    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string module_name;
    public DType initialize;
    public DType finalize;
    //RimeCustomApi*
    public IntPtr get_api;
}

[StructLayout(LayoutKind.Sequential)]
public struct RimeCustomApi
{
    public int data_size;
}