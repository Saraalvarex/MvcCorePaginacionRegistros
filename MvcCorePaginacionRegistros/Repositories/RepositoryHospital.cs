using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCorePaginacionRegistros.Data;
using MvcCorePaginacionRegistros.Models;
using System.Data;
using System.Diagnostics.Metrics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MvcCorePaginacionRegistros.Repositories
{
    public class RepositoryHospital
    {
        #region PROCEDURES
        //CREATE PROCEDURE SP_GRUPO_EMPLEADOS
        //(@POSICION INT)
        //AS
        //    SELECT EMP_NO, APELLIDO, OFICIO, DIR, FECHA_ALT
        // , SALARIO, COMISION, DEPT_NO
        //    FROM V_GRUPO_EMPLEADOS
        //    WHERE POSICION>= @POSICION AND POSICION<(@POSICION+5)
        //    ORDER BY APELLIDO
        //GO
        //CREATE PROCEDURE SP_GRUPO_EMPLEADOS_OFICIO
        //(@OFICIO NVARCHAR(20), @POSICION INT)
        //AS
        // SELECT EMP_NO, APELLIDO, OFICIO, DIR, FECHA_ALT
        // , SALARIO, COMISION, DEPT_NO
        // FROM(SELECT* FROM
        // (SELECT CAST(
        // ROW_NUMBER() OVER (ORDER BY APELLIDO) AS INT) AS POSICION,
        // EMP_NO, APELLIDO, OFICIO, DIR, FECHA_ALT
        // , SALARIO, COMISION, DEPT_NO
        // FROM EMP
        // WHERE OFICIO = @OFICIO) AS QUERY
        // WHERE QUERY.POSICION>=@POSICION AND QUERY.POSICION<(@POSICION+3))
        //    AS QUERY
        // ORDER BY APELLIDO
        //GO
        //ALTER PROCEDURE SP_GRUPO_EMPLEADOS_OFICIO
        //(
        //  @OFICIO NVARCHAR(20), 
        //  @POSICION INT,
        //  @NUMREGISTROS INT OUTPUT
        //)
        //AS
        //BEGIN
        //   SELECT EMP_NO, APELLIDO, OFICIO, DIR, FECHA_ALT, SALARIO, COMISION, DEPT_NO
        //   FROM(
        //         SELECT* FROM
        //         (
        //           SELECT CAST(
        //                  ROW_NUMBER() OVER (ORDER BY APELLIDO) AS INT) AS POSICION,
        //                  EMP_NO, APELLIDO, OFICIO, DIR, FECHA_ALT, SALARIO, COMISION, DEPT_NO
        //           FROM EMP
        //           WHERE OFICIO = @OFICIO
        //         ) AS QUERY
        //         WHERE QUERY.POSICION>=@POSICION AND QUERY.POSICION<(@POSICION+3)
        //        ) AS QUERY
        //   ORDER BY APELLIDO
        //   SET @NUMREGISTROS = @@ROWCOUNT
        //END
        //GO
        //EXEC SP_GRUPO_EMPLEADOS_OFICIO 'VENDEDOR', 2, 3
        #endregion
        private HospitalContext context;
        public RepositoryHospital(HospitalContext context)
        {
            this.context = context;
        }
        public async Task<List<Empleado>> GetEmpleadosOficio(string oficio, int posicion)
        {
            string sql = "SP_GRUPO_EMPLEADOS_OFICIO @OFICIO @POSICION @NUMREGISTROS OUT";
            SqlParameter pamoficio = new SqlParameter("@OFICIO", oficio);
            SqlParameter pamposicion = new SqlParameter("@POSICION", posicion);
            SqlParameter pamnumregistros = new SqlParameter("@NUMREGISTROS", SqlDbType.Int);
            pamnumregistros.Direction = ParameterDirection.Output;
            int numRegistros = (int)pamnumregistros.Value;
            var consulta = this.context.Empleados.FromSqlRaw(sql, pamoficio, pamposicion, pamnumregistros);
            return await consulta.ToListAsync();
        }
        public int GetNumeroRegistrosVistaDepartamentos()
        {
            return this.context.VistaDepartamentos.Count();
        }
        public int GetNumeroRegistrosEmpleados()
        {
            return this.context.Empleados.Count();
        }

        public async Task<List<Empleado>> GetGrupoEmpleadosAsync(int posicion)
        {
            string sql = "SP_GRUPO_EMPLEADOS @POSICION";
            SqlParameter pamposicion = new SqlParameter("@POSICION", posicion);
            var consulta = this.context.Empleados.FromSqlRaw(sql, pamposicion);
            return await consulta.ToListAsync();
        }
        public async Task<List<Departamento>> GetGrupoDepartamentosAsync(int posicion)
        {
            string sql = "SP_GRUPO_DEPARTAMENTOS @POSICION";
            SqlParameter pamposicion = new SqlParameter("@POSICION", posicion);
            var consulta = this.context.Departamentos.FromSqlRaw(sql, pamposicion);
            return await consulta.ToListAsync();
        }

        public async Task <List<VistaDepartamento>> GetGrupoVistaDepartamentoAsync(int posicion)
        {
            var consulta = from datos in this.context.VistaDepartamentos
                           where datos.Posicion >= posicion && datos.Posicion < (posicion + 2)
                           select datos;
            return await consulta.ToListAsync();
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
