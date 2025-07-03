using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _UnitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Company> objCompanyList = _UnitOfWork.Company.GetAll().ToList();
            return View(objCompanyList);
        }

        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                return View(new Company());
            }
            else
            {
                Company companyObj = _UnitOfWork.Company.Get(u => u.Id == id);
                return View(companyObj);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)
        {
            if (ModelState.IsValid)
            {
                
                if (CompanyObj.Id == 0)
                {
                    _UnitOfWork.Company.Add(CompanyObj);
                }
                else
                {
                    _UnitOfWork.Company.Update(CompanyObj);
                }

                _UnitOfWork.Save();
                TempData["success"] = "Company Created Successfull!";
                return RedirectToAction("Index", "Company");
            }
            else
            {
                return View(CompanyObj);
            }
        }

        

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll() 
        {
            List<Company> objCompanyList = _UnitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDelete = _UnitOfWork.Company.Get(u => u.Id == id);
            if (CompanyToBeDelete == null)
            {
                return Json(new {success = false, message = "Error while deleting"});
            }

            _UnitOfWork.Company.Remove(CompanyToBeDelete);
            _UnitOfWork.Save();

            return Json(new { success = true, message = "Delete Company Successful" });
        }
        #endregion
    }
}
