using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NetCoreSeguridadEmpleados.Data;
using NetCoreSeguridadEmpleados.Models;

namespace NetCoreSeguridadEmpleados.Repositories
{
    public class RepositoryHospital
    {
        private HospitalContext context;
        public RepositoryHospital(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<Empleado>> GetEmpledosAsync()
        {
            return await this.context.Empleados.ToListAsync();
        }
        public async Task<Empleado> FindEmpleado(int idEmpleado)
        {
            return await this.context.Empleados.FirstOrDefaultAsync(x => x.IdEmpleado == idEmpleado);
        }
        public async Task<List<Empleado>> GetEmpleadosDepartamentoAsync(int idDepartamento)
        {
            return await this.context.Empleados
                .Where(z => z.IdDepartamento == idDepartamento)
                .ToListAsync();
        }
        public async Task UpdateSalarioEmpleadosAsync(int idDepartamento, int incremento)
        {
            List <Empleado> empleados = await this.GetEmpleadosDepartamentoAsync(idDepartamento);
            foreach(Empleado emp in empleados)
            {
                emp.Salario += incremento;
            }
            await this.context.SaveChangesAsync();
        }
        public async Task<Empleado> LogInEmpleadoAsync
            (string apellido, int idEmpleado)
        {
            Empleado empleado = await this.context.Empleados.FirstOrDefaultAsync
                (z => z.Apellido == apellido
                && z.IdEmpleado == idEmpleado);
            return empleado;
        }
        public async Task DeleteEmpleado(int idEmpleado)
        {
            string sql = "delete from emp where emp_no=@idempleado";

            SqlParameter pamId = new SqlParameter("@idempleado", idEmpleado);

            await this.context.Database.ExecuteSqlRawAsync(sql, pamId);
        }
        public async Task<bool> BuscarSubordinados(int idEmpleado)
        {
            return await this.context.Empleados
                .AnyAsync(z => z.Subordinado == idEmpleado);
        }
    }
}
