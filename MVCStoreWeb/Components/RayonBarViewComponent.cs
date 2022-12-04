using Microsoft.AspNetCore.Mvc;
using MVCStoreData;

namespace MVCStoreWeb.Components
{
    public class RayonBarViewComponent : ViewComponent
    {
        private readonly AppDbContext context;

        public RayonBarViewComponent(
            AppDbContext context
            )
        {
            this.context = context;
        }

        public IViewComponentResult Invoke()
        {
            var model = context.Rayons.Where(p => p.Enabled).ToList();
            return View(model);
        }
    }
}
