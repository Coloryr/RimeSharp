using System.Runtime.InteropServices;

namespace RimeSharp;

public unsafe struct RimeMenu
{
    public int page_size;
    public int page_no;
    public bool is_last_page;
    public int highlighted_candidate_index;
    public int num_candidates;
    /// <summary>
    /// RimeCandidate*
    /// </summary>
    public RimeCandidate[] candidates;
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string select_keys;
}
