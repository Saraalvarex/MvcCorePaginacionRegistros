using Microsoft.EntityFrameworkCore;
using MvcCorePaginacionRegistros.Data;
using MvcCorePaginacionRegistros.Models;

namespace MvcCorePaginacionRegistros.Repositories
{
    public class RepositoryHospital
    {
        private HospitalContext context;
        public RepositoryHospital(HospitalContext context)
        {
            this.context = context;
        }
        public int GetNumeroRegistrosVistaDepartamentos()
        {
            return this.context.VistaDepartamentos.Count();
        }

        public async Task <VistaDepartamento> GetVistaDepartamento(int posicion)
        {
            VistaDepartamento vista = await this.context.VistaDepartamentos.FirstOrDefaultAsync(x=>x.Posicion==posicion);
            return vista;
        }
        public List<Departamento> GetDepartamentos()
        {
            var consulta = from datos in this.context.Departamentos
                           select datos;
            return consulta.ToList();
        }
        public List<Empleado> GetEmpleados(int iddepartamento)
        {
            var consulta = from datos in this.context.Empleados
                           where datos.DeptNo==iddepartamento
                           select datos;
            //return this.context.Empleados.Where(data => data.DeptNo == idDept).ToList();
            return consulta.ToList();
        }
    }
}
