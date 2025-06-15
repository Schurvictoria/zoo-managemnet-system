using System;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ILoggingService
    {
        Task LogInfoAsync(string message);
        Task LogErrorAsync(string message, Exception ex = null);
    }
} 