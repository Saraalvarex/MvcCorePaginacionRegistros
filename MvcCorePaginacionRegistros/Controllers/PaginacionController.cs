﻿using Microsoft.AspNetCore.Mvc;
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

        public async Task <IActionResult> PaginarGrupoDepartamentos(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            int numRegistros = this.repo.GetNumeroRegistrosVistaDepartamentos();
            ViewBag.REGISTROS = numRegistros;
            List<Departamento> departamentos = await this.repo.GetGrupoDepartamentosAsync(posicion.Value);
            return View(departamentos);
        }

        public async Task <IActionResult> PaginarGrupoVistaDepartamento(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            int numRegistros = this.repo.GetNumeroRegistrosVistaDepartamentos();
            #region EJEMPLO EN "PROCEDURE"
            //DECLARE @POSICION INT
            //SET @POSICION = 1
            //SELECT* FROM V_DEPARTAMENTOS_INDIVIDUAL
            //WHERE POSICION >= @POSICION AND POSICION<(@POSICION+2)
            #endregion
            int numPagina = 1;
            //Crear bucle que vata de N en N dependiendo del registro
            //llefaremos hasta el numero de registros
            string html = "<div>";
            //-------------------------------- 2 es la paginacion que haya 2 en cada pagina
            for (int i = 1; i <= numRegistros; i+=2)
            {
                html+= "<a href='PaginarGrupoVistaDepartamento?posicion="+i+
                "'>Pagina "+numPagina+"</a> | ";
                numPagina += 1;
            }
            html += "</div>";
            ViewBag.LINKS = html;
            ViewBag.REGISTROS = numRegistros;
            List<VistaDepartamento> departamentos = await this.repo.GetGrupoVistaDepartamentoAsync(posicion.Value);
            return View(departamentos);
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
