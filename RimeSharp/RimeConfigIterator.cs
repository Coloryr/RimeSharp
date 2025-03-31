using System.Runtime.InteropServices;

namespace RimeSharp;

[StructLayout(LayoutKind.Sequential)]
public struct RimeConfigIterator
{
    public IntPtr List;
    public IntPtr Map;
    public int Index;
    public string Key;
    public string Path;
}
