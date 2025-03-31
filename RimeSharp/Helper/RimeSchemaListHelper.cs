using System.Runtime.InteropServices;

namespace RimeSharp.Helper;

internal static class RimeSchemaListHelper
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RimeSchemaListIn
    {
        public long Size;

        public IntPtr List;
    }

    public static RimeSchemaList MarshalFromIntPtr(IntPtr ptr)
    {
        var list = Marshal.PtrToStructure<RimeSchemaListIn>(ptr);
        if (list.Size > 0 && list.List != IntPtr.Zero)
        {
            var array = new RimeSchemaListItem[list.Size];

            var current = list.List;
            var itemSize = Marshal.SizeOf(typeof(RimeSchemaListItem));
            for (var i = 0; i < list.Size; i++)
            {
                array[i] = Marshal.PtrToStructure<RimeSchemaListItem>(current);
                current = IntPtr.Add(current, itemSize);
            }

            return new RimeSchemaList { Size = list.Size, List = array };
        }

        return new RimeSchemaList { Size = list.Size, List = [] };
    }
}