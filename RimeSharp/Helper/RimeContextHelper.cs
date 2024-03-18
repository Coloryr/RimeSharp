using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RimeSharp.Helper;

internal class RimeContextHelper
{
    public unsafe struct RimeContextIn
    {
        public int data_size;
        // v0.9
        public RimeComposition composition;
        public RimeMenuIn menu;
        // v0.9.2
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string commit_text_preview;
        public IntPtr select_labels;
    }

    public unsafe struct RimeMenuIn
    {
        public int page_size;
        public int page_no;
        public bool is_last_page;
        public int highlighted_candidate_index;
        public int num_candidates;
        /// <summary>
        /// RimeCandidate*
        /// </summary>
        public IntPtr candidates;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string select_keys;
    }

    public static RimeContext MarshalFromIntPtr(IntPtr ptr)
    {
        // 首先，将ptr指向的内容封送到RimeSchemaList结构体
        var list = Marshal.PtrToStructure<RimeContextIn>(ptr);

        var context = new RimeContext
        {
            data_size = list.data_size,
            composition = list.composition
        };
        context.menu.page_size = list.menu.page_size;
        context.menu.page_no = list.menu.page_no;
        context.menu.is_last_page = list.menu.is_last_page;
        context.menu.highlighted_candidate_index = list.menu.highlighted_candidate_index;
        context.menu.num_candidates = list.menu.num_candidates;
        context.menu.select_keys = list.menu.select_keys;

        if (context.menu.num_candidates > 0 && list.menu.candidates != IntPtr.Zero)
        {
            var array = new RimeCandidate[list.menu.num_candidates];

            var current = list.menu.candidates;
            var itemSize = Marshal.SizeOf(typeof(RimeCandidate));
            for (var i = 0; i < list.menu.num_candidates; i++)
            {
                array[i] = Marshal.PtrToStructure<RimeCandidate>(current);
                current = IntPtr.Add(current, itemSize);
            }

            context.menu.candidates = array;
        }

        if (context.menu.page_size > 0 && list.select_labels != IntPtr.Zero)
        {
            var array = new string[context.menu.page_size];

            var current = list.menu.candidates;
            var itemSize = Marshal.SizeOf(typeof(RimeCandidate));
            for (var i = 0; i < list.menu.num_candidates; i++)
            {
                array[i] = Marshal.PtrToStringUTF8(current) ?? "";
                current = IntPtr.Add(current, itemSize);
            }

            context.select_labels = array;
        }

        return context;
    }
}
