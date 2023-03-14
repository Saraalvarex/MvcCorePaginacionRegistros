using Microsoft.AspNetCore.Mvc;
using MvcCorePaginacionRegistros.Models;
using MvcCorePaginacionRegistros.Repositories;

namespace MvcCorePaginacionRegistros.Controllers
{
    public class PaginacionController : Controller
    {
        private RepositoryHospital repo;

        public PaginacionController(RepositoryHospital repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task <IActionResult> PaginarRegistroVistaDepartamentoAsync(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            int numregistros = this.repo.GetNumeroRegistrosVistaDepartamentos();
            //Estamos en la posicion 1, que tenemos que devolver a la vista?
            int siguiente = posicion.Value + 1;
            if (siguiente > numregistros)
            {
                //desabilitar el boton
                siguiente = numregistros;
                ViewBag.HIDDENSIGUIENTE = "hidden";
            }
            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }
            VistaDepartamento vistaDepartamento = await this.repo.GetVistaDepartamento(posicion.Value);
            ViewBag.ULTIMO = numregistros;
            ViewBag.SIGUIENTE = siguiente;
            ViewBag.ANTERIOR = anterior;
            return View(vistaDepartamento);
        }
    }
}
