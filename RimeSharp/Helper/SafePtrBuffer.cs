using System.Runtime.InteropServices;

namespace RimeSharp.Helper;

internal class SafePtrBuffer(int size) : IDisposable
{
    public int Size => size;
    public IntPtr Ptr { get; private set; } = Marshal.AllocHGlobal(size);

    public void Dispose()
    {
        if (Ptr != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(Ptr);
            Ptr = IntPtr.Zero;
        }
    }
}
