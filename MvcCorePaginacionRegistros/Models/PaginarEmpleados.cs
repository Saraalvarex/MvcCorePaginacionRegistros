using System.ComponentModel.DataAnnotations.Schema;

namespace MvcCorePaginacionRegistros.Models
{
    public class PaginarEmpleados
    {
        public int NumRegistros { get; set; }
        public int Rango { get; set; }
        public List<Empleado> Empleados { get; set; }
        //Tambien podria guardar la posicion
    }
}
