using System.Runtime.InteropServices;

namespace RimeSharp.Helper;

internal class RimeContextHelper
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct RimeContextIn
    {
        public readonly int DataSize;
        // v0.9
        public RimeComposition Composition;
        public RimeMenuIn Menu;
        // v0.9.2
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string CommitTextPreview;
        public IntPtr SelectLabels;

        public RimeContextIn()
        {
            DataSize = Marshal.SizeOf<RimeContextIn>() - sizeof(int);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct RimeMenuIn
    {
        public int PageSize;
        public int PageNo;
        public bool IsLastPage;
        public int HighlightedCandidateIndex;
        public int NumCandidates;
        /// <summary>
        /// RimeCandidate*
        /// </summary>
        public IntPtr Candidates;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string SelectKeys;
    }

    public static RimeContext MarshalFromIntPtr(RimeContextIn list)
    {
        var context = new RimeContext
        {
            DataSize = list.DataSize,
            Composition = list.Composition
        };
        context.Menu.PageSize = list.Menu.PageSize;
        context.Menu.PageNo = list.Menu.PageNo;
        context.Menu.IsLastPage = list.Menu.IsLastPage;
        context.Menu.HighlightedCandidateIndex = list.Menu.HighlightedCandidateIndex;
        context.Menu.NumCandidates = list.Menu.NumCandidates;
        context.Menu.SelectKeys = list.Menu.SelectKeys;

        if (context.Menu.NumCandidates > 0 && list.Menu.Candidates != IntPtr.Zero)
        {
            var array = new RimeCandidate[list.Menu.NumCandidates];

            var current = list.Menu.Candidates;
            var itemSize = Marshal.SizeOf(typeof(RimeCandidate));
            for (var i = 0; i < list.Menu.NumCandidates; i++)
            {
                array[i] = Marshal.PtrToStructure<RimeCandidate>(current);
                current = IntPtr.Add(current, itemSize);
            }

            context.Menu.Candidates = array;
        }

        if (context.Menu.PageSize > 0 && list.SelectLabels != IntPtr.Zero)
        {
            var array = new string[context.Menu.PageSize];

            var current = list.Menu.Candidates;
            var itemSize = Marshal.SizeOf(typeof(RimeCandidate));
            for (var i = 0; i < list.Menu.NumCandidates; i++)
            {
                array[i] = Marshal.PtrToStringUTF8(current) ?? "";
                current = IntPtr.Add(current, itemSize);
            }

            context.SelectLabels = array;
        }

        return context;
    }
}
