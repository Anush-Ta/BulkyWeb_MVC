﻿using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _CategoryRepo;
        public CategoryController(ICategoryRepository db)
        {
            _CategoryRepo = db;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _CategoryRepo.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? categoryFromDb = _CategoryRepo.Get(u => u.Id == id);
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id);
            //Category? categoryFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _CategoryRepo.Update(obj);
                _CategoryRepo.Save();
                TempData["success"] = "Updated Category Successfull!";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            //if(obj.Name == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("DisplayOrder", "Cant equal!");
            //}
            if (ModelState.IsValid)
            {
                _CategoryRepo.Add(obj);
                _CategoryRepo.Save();
                TempData["success"] = "Created Category Successfull!";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? categoryFromDb = _CategoryRepo.Get(u => u.Id == id);
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id);
            //Category? categoryFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _CategoryRepo.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _CategoryRepo.Remove(obj);
            _CategoryRepo.Save();
            TempData["success"] = "Deleted Category Successfull!";
            return RedirectToAction("Index", "Category");
        }
    }
}
