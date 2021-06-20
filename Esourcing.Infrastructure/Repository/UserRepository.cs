using Esourcing.Core.Entities;
using Esourcing.Core.Repositories;
using Esourcing.Infrastructure.Data;
using Esourcing.Infrastructure.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esourcing.Infrastructure.Repository
{
    public class UserRepository : Repository<AppUser>, IUserRepository
    {
        private readonly WebAppContext _dbContext;

        public UserRepository(WebAppContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
