using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Transaction.API.Handlers;
using Transaction.Domain.Entities;
using Transaction.Domain.Enum;
using Transaction.Infra.Factory;
using Transaction.Infra.Repository;


namespace Transaction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountTransactionController : ControllerBase
    {
        private readonly AccountTransactionRepository _accountTransactionRepository;
        private readonly AccountRepository _accountRepository;
        private readonly RepositoryFactory _repositoryFactory;
        private readonly AccountTransactionsHandler _accountsTransactionHandler;

        /// <summary>
        /// Initialize Account Transactions Controller services..
        /// </summary>
        /// <param name="accountTransactionRepository"><see cref="AccountTransactionRepository"/> reference via dependency injection.</param>
        /// <param name="accountRepository"><see cref="AccountRepository"/> reference via dependency injection.</param>
        /// <param name="repositoryFactory"><see cref="RepositoryFactory"/> reference via dependency injection.</param>
        /// <param name="accountsTransactionHandler"><see cref="AccountTransactionsHandler"/> reference via dependency injection.</param>
        public AccountTransactionController(AccountTransactionRepository accountTransactionRepository,
            AccountRepository accountRepository,
            RepositoryFactory repositoryFactory,
            AccountTransactionsHandler accountsTransactionHandler)
        {
            _accountTransactionRepository = accountTransactionRepository;
            _accountRepository = accountRepository;
            _repositoryFactory = repositoryFactory;
            _accountsTransactionHandler = accountsTransactionHandler;
        }

        /// <summary>
        /// Get all transactions.
        /// </summary>
        /// <returns>All available transactions.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountTransaction>>> Get()
        {
            return await _accountTransactionRepository.Data.ToListAsync();
        }

        /// <summary>
        /// Get a specific transaction via Id.
        /// </summary>
        /// <param name="id">transaction Id</param>
        /// <returns>Transaction the match with given id.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountTransaction>> GetById(ulong id)
        {
            var transactions = await _accountTransactionRepository.Data.FindAsync(id);

            if (transactions == null)
            {
                return NotFound();
            }

            return transactions;
        }

        /// <summary>
        /// Request new transaction to database.
        /// </summary>
        /// <param name="entity"><see cref="AccountTransaction"/> entity to be inserted in database.</param>
        /// <returns>AccountTransaction entity added to database.</returns>
        [HttpPost]
        public async Task<ActionResult<AccountTransaction>> Post(AccountTransaction entity)
        {
            //Check for not null entity.
            if (entity == null)
                return BadRequest();

            //Validate entity via handler.
            if (_accountsTransactionHandler.Handle(entity) == null)
                return BadRequest();

            var destinationAccount = _accountRepository.Data.FirstOrDefaultAsync
                    (doc => doc.AccountNumber == entity.DestinationAccount);

            if (destinationAccount.Result == null)
                return BadRequest();

            var originAccount = _accountRepository.Data.FirstOrDefaultAsync
                    (doc => doc.AccountNumber == entity.OriginAccount);

            //Check Transaction.Type
            switch (entity.Type)
            {
                case TransactionType.Transfer:
                    if (originAccount.Result == null)
                        return BadRequest();

                    
                    if (_accountsTransactionHandler.CheckBalance(originAccount.Result.Balance, entity.Amount))
                        return BadRequest();

                    originAccount.Result.Balance = _accountsTransactionHandler
                        .UpdateBalance(originAccount.Result.Balance, -entity.Amount);

                    destinationAccount.Result.Balance = _accountsTransactionHandler
                        .UpdateBalance(destinationAccount.Result.Balance, entity.Amount);

                    _accountRepository.Update(originAccount.Result);
                    _accountRepository.Update(destinationAccount.Result);

                    await _accountRepository.SaveChangesAsync();

                    break;
                case TransactionType.Deposit:
                    destinationAccount.Result.Balance = _accountsTransactionHandler
                        .UpdateBalance(destinationAccount.Result.Balance, entity.Amount);

                    _accountRepository.Update(destinationAccount.Result);

                    await _accountRepository.SaveChangesAsync();

                    break;
                case TransactionType.Withdraw:
                    if (_accountsTransactionHandler.CheckBalance(destinationAccount.Result.Balance, entity.Amount))
                        return BadRequest();

                    destinationAccount.Result.Balance = _accountsTransactionHandler
                        .UpdateBalance(destinationAccount.Result.Balance, entity.Amount);

                    _accountRepository.Update(destinationAccount.Result);

                    await _accountRepository.SaveChangesAsync();

                    break;
            };

            //Store transactions.
            _accountTransactionRepository.Data.Add(entity);
            //Return result.
            await _accountTransactionRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }
    }
}