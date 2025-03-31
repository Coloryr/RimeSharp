using System.Runtime.InteropServices;

namespace RimeSharp;

public unsafe struct RimeMenu
{
    public int PageSize;
    public int PageNo;
    public bool IsLastPage;
    public int HighlightedCandidateIndex;
    public int NumCandidates;
    /// <summary>
    /// RimeCandidate*
    /// </summary>
    public RimeCandidate[] Candidates;
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string SelectKeys;
}
