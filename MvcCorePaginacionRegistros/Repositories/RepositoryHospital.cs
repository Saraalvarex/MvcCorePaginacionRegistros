﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCorePaginacionRegistros.Data;
using MvcCorePaginacionRegistros.Models;

namespace MvcCorePaginacionRegistros.Repositories
{
    public class RepositoryHospital
    {
        #region PROCEDURES
        //CREATE PROCEDURE SP_GRUPO_DEPARTAMENTOS
        //(@POSICION INT)
        //AS
        //    SELECT DEPT_NO, DNOMBRE, LOC
        //    FROM V_DEPARTAMENTOS_INDIVIDUAL
        //    WHERE POSICION>= @POSICION AND POSICION<(@POSICION+2)
        //GO
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
