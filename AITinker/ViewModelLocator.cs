using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AITinker.ViewModels;

namespace AITinker;
internal class ViewModelLocator {
    public ChatViewModel ChatViewModel => App.ServiceProvider.GetRequiredService<ChatViewModel>();
}
