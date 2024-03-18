using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RimeSharp.Helper;

// 一个辅助类，用于处理RimeSchemaList的封送
public static class RimeSchemaListHelper
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RimeSchemaListIn
    {
        public long size;

        /// <summary>
        /// 注意：在非托管代码中，这可能是一个指向第一个元素的指针，而在C#中我们使用数组。
        /// 我们需要使用IntPtr来接收数组的首地址，然后再进行转换。
        /// </summary>
        public IntPtr list; // 注意这里的改动
    }

    public static RimeSchemaList MarshalFromIntPtr(IntPtr ptr)
    {
        // 首先，将ptr指向的内容封送到RimeSchemaList结构体
        var list = Marshal.PtrToStructure<RimeSchemaListIn>(ptr);

        // 然后处理list中的IntPtr，将其转换为RimeSchemaListItem[]
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

        return new RimeSchemaList { size = list.size, list = Array.Empty<RimeSchemaListItem>() };
    }
}