using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RimeSharp;

public struct RimeSchemaList
{
    public long size;
    /// <summary>
    /// RimeSchemaListItem* list;
    /// </summary>
    public RimeSchemaListItem[] list;
}
