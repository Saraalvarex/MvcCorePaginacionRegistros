using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCorePaginacionRegistros.Data;
using MvcCorePaginacionRegistros.Models;
using System.Collections.Generic;
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
        //ALTER PROCEDURE SP_GRUPO_EMPLEADOS_OFICIO_NUMREGISTROS
        //(
        //  @POSICION INT,
        //  @OFICIO NVARCHAR(20), 
        //  @NUMREGISTROS INT OUT
        //)
        //AS
        //SELECT @NUMREGISTROS = COUNT(EMP_NO) FROM EMP WHERE OFICIO = @OFICIO
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
        //   --SET @NUMREGISTROS = @@ROWCOUNT
        //GO
        //EXEC SP_GRUPO_EMPLEADOS_OFICIO_NUMREGISTROS 'VENDEDOR', 1
        //CREATE PROCEDURE SP_GRUPO_EMPLEADOS_OFICIO_RANGO
        //(@POSICION INT, @OFICIO NVARCHAR(20), @NUMREGISTROS INT OUT, @RANGO INT)
        //        AS
        //        SELECT @NUMREGISTROS = COUNT(EMP_NO) FROM EMP WHERE OFICIO = @OFICIO
        //           SELECT EMP_NO, APELLIDO, OFICIO, DIR, FECHA_ALT, SALARIO, COMISION, DEPT_NO
        //           FROM(
        //                 SELECT* FROM
        //                 (
        //                   SELECT CAST(
        //                          ROW_NUMBER() OVER (ORDER BY APELLIDO) AS INT) AS POSICION,
        //                          EMP_NO, APELLIDO, OFICIO, DIR, FECHA_ALT, SALARIO, COMISION, DEPT_NO
        //                   FROM EMP
        //                   WHERE OFICIO = @OFICIO
        //                 ) AS QUERY
        //                 WHERE QUERY.POSICION>=@POSICION AND QUERY.POSICION<(@POSICION + @RANGO)
        //                ) AS QUERY
        //           ORDER BY APELLIDO
        //           GO
        #endregion
        private HospitalContext context;
        public RepositoryHospital(HospitalContext context)
        {
            this.context = context;
        }
        public int GetNumeroRegistrosVistaDepartamentos()
        {
            return this.context.VistaDepartamentos.Count();
        }
        public int GetNumeroRegistrosEmpleados()
        {
            return this.context.Empleados.Count();
        }
        public int GetNumRegistrosEmpleadosOficio(string oficio)
        {
            var consulta = from datos in this.context.Empleados
                           where datos.Oficio==oficio
                           select datos;
            return consulta.Count();
        }
        
        public async Task<PaginarEmpleados> GetGrupoEmpleadosOficioAsync(int posicion, string oficio, int rango)
        {
            string sql = "SP_GRUPO_EMPLEADOS_OFICIO_RANGO @POSICION, @OFICIO, @NUMREGISTROS OUT, @RANGO";
            //string sql = "SP_GRUPO_EMPLEADOS_OFICIO_NUMREGISTROS @POSICION, @OFICIO, @NUMREGISTROS OUT";
            //string sql = "SP_GRUPO_EMPLEADOS_OFICIO @POSICION, @OFICIO";
            SqlParameter pamposicion = new SqlParameter("@POSICION", posicion);
            SqlParameter pamoficio = new SqlParameter("@OFICIO", oficio);
            SqlParameter pamnumregistros = new SqlParameter("@NUMREGISTROS", -1);
            SqlParameter pamrango = new SqlParameter("@RANGO", rango);
            pamnumregistros.Direction = ParameterDirection.Output;
            var consulta = this.context.Empleados.FromSqlRaw(sql, pamposicion, pamoficio, pamnumregistros, pamrango);
            List<Empleado> empleados = await consulta.ToListAsync();
            int numregistros = (int)pamnumregistros.Value;
            return new PaginarEmpleados
            {
                NumRegistros = numregistros,
                Rango = rango,
                Empleados=empleados
            };
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
