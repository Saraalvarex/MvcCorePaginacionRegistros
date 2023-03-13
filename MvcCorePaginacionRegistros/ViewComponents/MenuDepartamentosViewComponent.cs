using Microsoft.AspNetCore.Mvc;
using MvcCorePaginacionRegistros.Models;
using MvcCorePaginacionRegistros.Repositories;

namespace MvcCorePaginacionRegistros.ViewComponents
{
    public class MenuDepartamentosViewComponent: ViewComponent
    {
        //Por supuesto, podemo utilizar inyeccion
        private RepositoryHospital repo;

        public MenuDepartamentosViewComponent(RepositoryHospital repo)
        {
            this.repo = repo;
        }

        //la peticion se realiza mediante el metodo InvokeAsync
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Departamento> departamentos = this.repo.GetDepartamentos();
            return View(departamentos);
        }
    }
}
