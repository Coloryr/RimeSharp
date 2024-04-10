using System.Runtime.InteropServices;

namespace RimeSharp;

[StructLayout(LayoutKind.Sequential)]
public struct RimeCandidateListIterator
{
    public IntPtr ptr;
    public int index;
    public RimeCandidate candidate;
}
