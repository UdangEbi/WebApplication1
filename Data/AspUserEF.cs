using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class AspUserEF : IAspUser
    {
        private readonly ApplicationDbContext _context;
        public AspUserEF(ApplicationDbContext context)
        {
            _context = context;
        }
        public void DeleteUser(string username)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AspUser> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public AspUser GetUserByUsername(string username)
        {
            return _context.AspUsers.FirstOrDefault(u => u.Username == username);
        }

        public bool Login(string username, string password)
        {
            var user = GetUserByUsername(username);
            if (user == null) return false;

            var hashedInput = Helpers.HashHelper.HashPassword(password);
            return user.Password == hashedInput;
        }

        public AspUser RegisterUser(AspUser user)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user), "User cannot be null");
                }
                user.Password = Helpers.HashHelper.HashPassword(user.Password);
                _context.AspUsers.Add(user);
                _context.SaveChanges();
                return user;
            }
            catch (DbUpdateException dbex)
            {
                // Log the exception or handle it as needed
                throw new Exception("An error occured while adding the user", dbex);
            }
            catch (System.Exception ex)
            {
                // Handle other exceptions that may occur
                throw new Exception("An unexpected error occurred", ex);
            }
        }

        public AspUser UpdateUser(AspUser user)
        {
            throw new NotImplementedException();
        }
    }
}