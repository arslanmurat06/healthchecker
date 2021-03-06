using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HealthChecker.Contracts.DTOs;
using HealthChecker.Contracts.Interfaces.Repositories;
using HealthChecker.DataContext;
using Microsoft.EntityFrameworkCore;

namespace HealthChecker.Core.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDTO> GetUserById(string userId)
        {
            var user = await _context.Users.AsNoTracking()
                                              .Where(u => u.Id == userId)
                                              .Select(u => _mapper.Map<UserDTO>(u))
                                              .FirstOrDefaultAsync();

            return user;
        }

        public async Task<UserDTO> GetUserByName(string username)
        {
            var user = await _context.Users.AsNoTracking()
                                             .Where(u => u.UserName == username)
                                             .Select(u => _mapper.Map<UserDTO>(u))
                                             .FirstOrDefaultAsync();

            return user;

        }
    }
}
