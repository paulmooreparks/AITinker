using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITinker.Models;
internal class MessageEntry {
    public string? Message { get; set; }
    public MessageSource Source { get; set; }
}

internal enum MessageSource {
    User,
    LLM
}
