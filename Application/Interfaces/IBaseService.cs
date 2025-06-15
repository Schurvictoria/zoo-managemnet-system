using System;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBaseService
    {
        Task<bool> ValidateAsync();
    }
} 