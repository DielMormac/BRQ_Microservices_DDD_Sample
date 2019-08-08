using Microsoft.EntityFrameworkCore;
using Transaction.Domain.Entities;
namespace Transaction.Infra.Repository
{
    public class AccountTransactionRepository : DbContext
    {
        ///<inheritdoc/>
        public AccountTransactionRepository(DbContextOptions<AccountTransactionRepository> options)
           : base(options)
        {
        }

        /// <summary>
        /// Stored <see cref="AccountTransaction"/> data.
        /// </summary>
        public DbSet<AccountTransaction> Data { get; set; }
    }
}