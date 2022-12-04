﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCStoreData;
using System.Data;

namespace MVCStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrators, ProductManagers")]
    public class RayonsController : Controller
    {
        private readonly AppDbContext context;

        public RayonsController(
            AppDbContext context
            )
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = await context.Rayons.ToListAsync();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Rayon { Enabled = true });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Rayon model)
        {
            model.DateCreated = DateTime.UtcNow;
            model.Enabled = true;

            context.Rayons.Add(model);
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = "Reyon ekleme işlemi başarıyla tamamlanmıştır!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                TempData["error"] = "Aynı isimli başka bir reyon bulunduğundan ekleme işlemi yapılamıyor!";
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Beklenmedik hata, daha sonra tekrar deneyiniz!";
                return View(model);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await context.Rayons.FindAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Rayon model)
        {

            context.Rayons.Update(model);
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = "Reyon güncelleme işlemi başarıyla tamamlanmıştır!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["error"] = "Aynı isimli başka bir reyon bulunduğundan güncelleme işlemi yapılamıyor!";
                return View(model);
            }

        }


        [HttpGet]
        public async Task<IActionResult> Remove(Guid id)
        {
            var model = await context.Rayons.FindAsync(id);
            context.Rayons.Remove(model);
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = "Reyon silme işlemi başarıyla tamamlanmıştır!";
            }
            catch (Exception)
            {
                TempData["error"] = $"{model.Name} isimli reyon bir ya da daha fazla kayıt ile ilişkili olduğu için silme işlemi tamamlanamıyor!";
            }
            return RedirectToAction(nameof(Index));

        }

    }
}
