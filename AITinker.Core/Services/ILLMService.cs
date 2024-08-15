using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AITinker.Core.Models;

namespace AITinker.Core.Services;
public interface ILLMService {
    Task<LLMResponse> SendMessage(string message, string extra);
}
