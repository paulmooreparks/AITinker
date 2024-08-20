using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITinker.Core.Models;

public class Configurations {
    public Dictionary<string, KitConfiguration>? Table { get; set; }
}

public class KitConfiguration {
    public string? Kit { get; set; }
    public Dictionary<string, object>? Settings { get; set; }
}
