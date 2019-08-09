using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public interface IMobileCodeSender
    {
        Task<string> SendAsync(string phoneNumber);

        Task<bool> CheckAsync(string phoneNumber, string code);
    }
}
