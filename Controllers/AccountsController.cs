using ManejadorPresupuestos.Data.Repositories.Interfaces;
using ManejadorPresupuestos.Models;
using ManejadorPresupuestos.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejadorPresupuestos.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountTypeRepository accountTypeRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IUsersService usersService;

        public AccountsController(IAccountTypeRepository accountTypeRepository,
            IUsersService usersService, IAccountRepository accountRepository)
        {
            this.accountTypeRepository = accountTypeRepository;
            this.accountRepository = accountRepository;
            this.usersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = usersService.GetUserId();
            var accountsOfAccountsT = await accountRepository.Search(userId);

            var model = accountsOfAccountsT.GroupBy(x => x.AccountType)
                                           .Select(group => new IndexAccountViewModel
                                           {
                                               AccountType = group.Key,
                                               Accounts = group.AsEnumerable()
                                           }).ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = usersService.GetUserId();
            var model = new AccountCreateViewModel();

            model.AccountTypes = await GetAccountTypes(userId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountCreateViewModel accountCVM)
        {
            var userId = usersService.GetUserId();
            var accountType = await accountTypeRepository.GetAccountById(accountCVM.AccountTypeId, userId);

            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            if (!ModelState.IsValid)
            {
                accountCVM.AccountTypes = await GetAccountTypes(userId);
                return View(accountCVM);
            }

            await accountRepository.Create(accountCVM);
            return RedirectToAction("Index");
        }
        // Get account types
        private async Task<IEnumerable<SelectListItem>> GetAccountTypes(int userId)
        {
            var accountsTypes = await accountTypeRepository.GetAccounts(userId);
            return accountsTypes.Select(x => new SelectListItem(x.AccountName, x.Id.ToString()));
        }
    }
}
