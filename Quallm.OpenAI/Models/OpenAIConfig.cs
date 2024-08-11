﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quallm.OpenAI.Models;

public class OpenAIConfig {
    public string? ApiKey { get; set; }
    public string? ApiUrl { get; set; } = "https://api.openai.com/v1/chat/completions";
    public string? Model { get; set; } = "gpt-4o-mini";
}
