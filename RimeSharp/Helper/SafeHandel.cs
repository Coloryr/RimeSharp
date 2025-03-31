using System.Runtime.InteropServices;

namespace RimeSharp.Helper;

internal class SafeHandel<T> : IDisposable where T : struct
{
    private T _data;

    public IntPtr Ptr { get; private set; }

    public SafeHandel()
    {
        Ptr = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
        _data = new T();
        Marshal.StructureToPtr(_data, Ptr, false);
    }

    public SafeHandel(T data)
    {
        Ptr = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
        _data = data;
        Marshal.StructureToPtr(_data, Ptr, false);
    }

    public T GetData()
    {
        if (Ptr != IntPtr.Zero)
        {
            _data = Marshal.PtrToStructure<T>(Ptr);
        }

        return _data;
    }

    public void Dispose()
    {
        if (Ptr != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(Ptr);
            Ptr = IntPtr.Zero;
        }
    }
}
