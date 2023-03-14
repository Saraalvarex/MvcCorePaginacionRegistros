using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#region VISTA
//CREATE VIEW V_GRUPO_EMPLEADOS
//AS
//	SELECT CAST(
//	ROW_NUMBER() OVER(ORDER BY EMP_NO) AS INT) AS POSICION,
//    ISNULL(EMP_NO, 0) AS EMP_NO, APELLIDO, OFICIO, DIR, FECHA_ALT
//    , SALARIO, COMISION, DEPT_NO FROM EMP
//GO
#endregion
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
