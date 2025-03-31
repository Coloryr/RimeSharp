using System.Runtime.InteropServices;

namespace RimeSharp;

[StructLayout(LayoutKind.Sequential)]
public struct RimeModule
{
    public delegate void DType();
    public delegate IntPtr DType1();

    public readonly int DataSize;

    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string ModuleName;
    public DType Initialize;
    public DType Finalize;
    //RimeCustomApi*
    public IntPtr GetApi;
}

[StructLayout(LayoutKind.Sequential)]
public struct RimeCustomApi
{
    public int DataSize;
}