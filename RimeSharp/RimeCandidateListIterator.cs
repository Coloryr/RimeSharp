using System.Runtime.InteropServices;

namespace RimeSharp;

[StructLayout(LayoutKind.Sequential)]
public struct RimeCandidateListIterator
{
    public IntPtr Ptr;
    public int Index;
    public RimeCandidate Candidate;
}
