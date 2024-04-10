using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace RimeSharp;

[StructLayout(LayoutKind.Sequential)]
public struct RimeCandidateListIterator
{
    public IntPtr ptr;
    public int index;
    public RimeCandidate candidate;
}
