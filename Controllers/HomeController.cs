using BankSystemBota.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BankSystemBota.Controllers
{
    //[ApiController]
    //[Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;
        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            List<DepositInfo> deposits = (from m in _context.Deposit select m).ToList();
            return View(deposits);
        }
        [HttpGet]
        public IActionResult Convertation()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Convertation(CurrencyConverter model)
        {
            if (model.CurrencyType == CurrencyType.Rubl)
            {
                model.Result = (model.Tenge * 0.13).ToString();
            }
            if (model.CurrencyType == CurrencyType.Dollar)
            {
                model.Result = (model.Tenge * 0.0021).ToString();
            }
            if (model.CurrencyType == CurrencyType.Uan)
            {
                model.Result = (model.Tenge * 0.016).ToString();
            }
            return View(model);
        }
        

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult ConverMoney(string tenge, string type)
        {
            string authData = $"color: {type} {tenge}";
            return View(authData);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public async Task<IActionResult> CreateDeposit()
        {
            return View();
        }
        // crud
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDeposit([Bind("Balance,Income,Outcome,CreateDate")] DepositInfo information)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(information);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }

            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " + "Try again or call system admin");
            }

            return View(information);
        }
        public async Task<IActionResult> TableOfInformations()
        {
            return View(await _context.Deposit.ToListAsync());
        }

        //Edit or Update
        [HttpPost, ActionName("EditDepositInformation")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDepositInformation(Guid? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var depositInfo = await _context.Deposit.FirstOrDefaultAsync(s => s.Id == Id);


            if (await TryUpdateModelAsync<DepositInfo>(
                depositInfo, "", s => s.Balance, s => s.Income, s => s.Outcome, s => s.CreateDate))
            {
                try
                {
                    depositInfo = await _context.Deposit.FirstOrDefaultAsync();
                    depositInfo.Balance = depositInfo.Balance + depositInfo.Income - depositInfo.Outcome;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again or call system admin");
                }
            }

            return View(depositInfo);
        }


        public async Task<IActionResult> EditDepositInformation(Guid Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var deposit = await _context.Deposit.FirstOrDefaultAsync(m => m.Id == Id);

            if (deposit == null)
            {
                return NotFound();
            }

            return View(deposit);
        }

        //Details
        public async Task<IActionResult> DetailsOfDeposit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deposit = await _context.Deposit.FirstOrDefaultAsync(m => m.Id == id);

            if (deposit == null)
            {
                return NotFound();
            }

            return View(deposit);
        }




        //Delete 
        public async Task<IActionResult> DeleteDeposit(Guid id, bool? Savechangeserror = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deposit = await _context.Deposit.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (deposit == null)
            {
                return NotFound();
            }

            if (Savechangeserror.GetValueOrDefault())
            {
                ViewData["DeleteError"] = "Delete failed, please try again later ... ";
            }

            return View(deposit);
        }

        //Delete continue
        [HttpPost, ActionName("DeleteDeposit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDeletePerson(Guid id)
        {
            var deposit = await _context.Deposit.FindAsync(id);

            if (deposit == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Deposit.Remove(deposit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(DeleteDeposit), new { id = id, Savechangeserror = true });
            }
        }

        public async Task<IActionResult> DepositList()
        {
            return PartialView("DepositList");
        }
    }
}