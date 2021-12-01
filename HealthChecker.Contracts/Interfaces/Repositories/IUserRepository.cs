using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthChecker.Contracts.DTOs;

namespace HealthChecker.Contracts.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<UserDTO> GetUser(string userName);
    }
}
