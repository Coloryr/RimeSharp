using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RimeSharp;

public struct RimeCandidateListIterator
{
    public IntPtr ptr;
    public int index;
    public RimeCandidate candidate;
}
