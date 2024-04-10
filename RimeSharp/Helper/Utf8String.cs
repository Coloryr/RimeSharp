using System.Text;
using System.Runtime.InteropServices;
using System.Buffers;

namespace RimeSharp.Helper;

internal class Utf8Buffer : SafeHandle
{
    private GCHandle _gcHandle;
    private byte[]? _data;

    public Utf8Buffer(string? s) : base(IntPtr.Zero, true)
    {
        if (s == null)
            return;
        _data = Encoding.UTF8.GetBytes(s);
        _gcHandle = GCHandle.Alloc(_data, GCHandleType.Pinned);
        handle = _gcHandle.AddrOfPinnedObject();
    }

    public int ByteLen => _data?.Length ?? 0;

    protected override bool ReleaseHandle()
    {
        if (handle != IntPtr.Zero)
        {
            handle = IntPtr.Zero;
            _data = null;
            _gcHandle.Free();
        }
        return true;
    }

    public override bool IsInvalid => handle == IntPtr.Zero;

    public static string? StringFromPtr(IntPtr s)
    {
        int len;
        unsafe
        {
            var pstr = (byte*)s;
            if (pstr == null)
                return null;

            for (len = 0; pstr[len] != 0; len++) ;
        }

        var bytes = ArrayPool<byte>.Shared.Rent(len);

        try
        {
            Marshal.Copy(s, bytes, 0, len);
            return Encoding.UTF8.GetString(bytes, 0, len);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(bytes);
        }
    }
}
