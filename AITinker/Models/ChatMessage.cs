using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITinker.Models;

internal class ChatMessage {
    public string? UserMessage { get; set; }
    public string? LLMResponse { get; set; }
}
