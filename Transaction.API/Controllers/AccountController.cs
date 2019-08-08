using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Transaction.Domain.Entities;
using Transaction.Infra.Factory;
using Transaction.Infra.Repository;
using Transaction.API.Handlers;

namespace Transaction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountRepository _accountRepository;
        private readonly RepositoryFactory _repositoryFactory;
        private readonly AccountsHandler _accountsHandler;

        /// <summary>
        /// Initialize Account Controller services.
        /// </summary>
        /// <param name="accountRepository"><see cref="AccountRepository"/> reference via dependency injection.</param>
        /// <param name="repositoryFactory"><see cref="RepositoryFactory"/> reference via dependency injection.</param>
        /// <param name="accountsHandler"><see cref="AccountsHandler"/> reference via dependency injection.</param>
        public AccountController(AccountRepository accountRepository,
            RepositoryFactory repositoryFactory,
            AccountsHandler accountsHandler)
        {
            _accountRepository = accountRepository;
            _repositoryFactory = repositoryFactory;
            _accountsHandler = accountsHandler;
        }

        /// <summary>
        /// Get all accounts.
        /// </summary>
        /// <returns>All available accounts.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> Get()
        {
            return await _accountRepository.Data.ToListAsync();
        }

        /// <summary>
        /// Get a specific account via Id.
        /// </summary>
        /// <param name="id">Account Id</param>
        /// <returns>Account the match with given id.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetById(ulong id)
        {
            var accounts = await _accountRepository.Data.FindAsync(id);

            if (accounts == null)
            {
                return NotFound();
            }

            return accounts;
        }

        /// <summary>
        /// Insert new account to database.
        /// </summary>
        /// <param name="entity"><see cref="Account"/> entity to be inserted in database.</param>
        /// <returns>Account entity added to database.</returns>
        [HttpPost]
        public async Task<ActionResult<Account>> Post(Account entity)
        {
            //Check for not null entity.
            if (entity == null)
                return BadRequest();

            //Validate entity via handler.
            if (_accountsHandler.Handle(entity) == null)
                return BadRequest();

            //Check if account already exists.
            var account = _accountRepository.Data.FirstOrDefaultAsync
                (doc => doc.AccountNumber == entity.AccountNumber);
            if (account.Result != null)
                return CreatedAtAction(nameof(GetById), new { id = account.Result.Id }, account.Result);

            //Add to database.
            _accountRepository.Data.Add(entity);
            await _accountRepository.SaveChangesAsync();

            //Return created account.
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }
    }
}
