using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITinker.OpenAI.Models;

[Serializable]
internal class Message {
    public string role { get; set; } = string.Empty;
    public string content { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
}
