using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcCorePaginacionRegistros.Models
{
    [Table("EMP")]
    public class Empleado
    {
        [Key]
        [Column("emp_no")]
        public int EmpNo { get; set; }
        [Column("apellido")]
        public string Apellido { get; set; }
        [Column("oficio")]
        public string Oficio { get; set; }
        [Column("dir")]
        public int Dir { get; set; }
        [Column("fecha_alt")]
        public DateTime FechaAlta { get; set; }
        [Column("salario")]
        public int Salario { get; set; }
        [Column("comision")]
        public int Comision { get; set; }
        [Column("dept_no")]
        public int DeptNo { get; set; }
    }
}
