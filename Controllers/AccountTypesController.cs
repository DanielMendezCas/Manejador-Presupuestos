using Dapper;
using ManejadorPresupuestos.Data.Repositories.Interfaces;
using ManejadorPresupuestos.Models;
using ManejadorPresupuestos.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;

namespace ManejadorPresupuestos.Controllers
{
    public class AccountTypesController : Controller
    {
        private readonly IAccountTypeRepository accountTypeRepository;
        private readonly IUsersService usersService;
        public AccountTypesController(IAccountTypeRepository accountTypeRepository,
                                     IUsersService usersService)
        {
           this.accountTypeRepository = accountTypeRepository;
           this.usersService = usersService;
           
        }
        // GET: AccountTypesController
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = usersService.GetUserId();
            var accountType = await accountTypeRepository.GetAccounts(userId);
            return View(accountType);
        }

        // GET: AccountTypesController/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var userId = usersService.GetUserId();
            var accountType = await accountTypeRepository.GetAccountById(id, userId);
            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            return View(accountType);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(AccountType accountType)
        {
            var userId = usersService.GetUserId();
            var existingAccountType =
                await accountTypeRepository.GetAccountById(accountType.Id, userId);

            if(existingAccountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await accountTypeRepository.UpdateAccount(accountType);

            return RedirectToAction("Index");

        }

        // GET: AccountTypesController/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: AccountTypesController/Create
        [HttpPost]
        public async Task<IActionResult> Create(AccountType accountType)
        {
            if (!ModelState.IsValid)
            {
                return View(accountType);
            }

            accountType.UserId = usersService.GetUserId();

            var accountExists = await accountTypeRepository.Exists(accountType.AccountName, accountType.UserId);

            if (accountExists)
            {
                ModelState.AddModelError(nameof(accountType.AccountName), 
                    $"La cuenta {accountType.AccountName} ya existe");
                return View(accountType);
            }

            await accountTypeRepository.CreateAccount(accountType);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> CheckAccountTypeExists(string accountName)
        {
            var userId = usersService.GetUserId();
            var alreadyExists = await accountTypeRepository.Exists(accountName, userId);

            if (alreadyExists)
            {
                return Json($"La cuenta {accountName} ya existe");
            }

            return Json(true);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var userId = usersService.GetUserId();
            var accountType = await accountTypeRepository.GetAccountById(id, userId);

            if(accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(accountType);
        }

        // POST: AccountTypesController/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var userId = usersService.GetUserId();
            var accountType = await accountTypeRepository.GetAccountById(id, userId);

            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await accountTypeRepository.Delete(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Order([FromBody] int[] ids) 
        {
            var userId = usersService.GetUserId();
            var accountTypes = await accountTypeRepository.GetAccounts(userId);
            var idsAccountTypes = accountTypes.Select(x => x.Id);

            var otherIds = ids.Except(idsAccountTypes).ToList();

            //Checking if there are accounts ids that not corresponds to actual user
            if(otherIds.Count > 0)
            {
                return Forbid();
            }

            var orderedAccountTypes = ids.Select((value, index) => 
                new AccountType() { Id = value, Order = index + 1}).AsEnumerable();

            await accountTypeRepository.Order(orderedAccountTypes);

            return Ok();
        }
    }
}
