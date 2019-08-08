using Microsoft.EntityFrameworkCore;
using Transaction.Domain.Entities;

namespace Transaction.Infra.Repository
{
    public class AccountRepository : DbContext
    {
        ///<inheritdoc/>
        public AccountRepository(DbContextOptions<AccountRepository> options)
           : base(options)
        {
        }

        /// <summary>
        /// Stored <see cref="AccountTransaction"/> data.
        /// </summary>
        public DbSet<Account> Data { get; set; }
    }
}
