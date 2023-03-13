using Microsoft.AspNetCore.Mvc;
using MvcCorePaginacionRegistros.Models;
using MvcCorePaginacionRegistros.Repositories;

namespace MvcCorePaginacionRegistros.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryHospital repo;
        public EmpleadosController(RepositoryHospital repo)
        {
            this.repo = repo;
        }
        public IActionResult Index(int iddepartamento)
        {
            List<Empleado> empleados = this.repo.GetEmpleados(iddepartamento);
            return View(empleados);
        }
    }
}
