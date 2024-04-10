using System.Runtime.InteropServices;

namespace RimeSharp;

[StructLayout(LayoutKind.Sequential)]
public struct RimeConfigIterator
{
    public IntPtr list;
    public IntPtr map;
    public int index;
    public string key;
    public string path;
}
