using System.Runtime.InteropServices;

namespace RimeSharp.Helper;

public static class RimeSchemaListHelper
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RimeSchemaListIn
    {
        public long size;

        public IntPtr list;
    }

    public static RimeSchemaList MarshalFromIntPtr(IntPtr ptr)
    {
        var list = Marshal.PtrToStructure<RimeSchemaListIn>(ptr);
        if (list.size > 0 && list.list != IntPtr.Zero)
        {
            var array = new RimeSchemaListItem[list.size];

            var current = list.list;
            var itemSize = Marshal.SizeOf(typeof(RimeSchemaListItem));
            for (var i = 0; i < list.size; i++)
            {
                array[i] = Marshal.PtrToStructure<RimeSchemaListItem>(current);
                current = IntPtr.Add(current, itemSize);
            }

            return new RimeSchemaList { size = list.size, list = array };
        }

        return new RimeSchemaList { size = list.size, list = [] };
    }
}