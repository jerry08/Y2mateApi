using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y2mateApi;

public class Link
{
    public string Id { get; set; } = default!;

    /// <summary>
    /// Size in MB
    /// </summary>
    public string Size { get; set; } = default!;

    public string Quality { get; set; } = default!;

    public FileType FileType { get; set; }
}