using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreSeguridadEmpleados.Filters;
using NetCoreSeguridadEmpleados.Models;
using NetCoreSeguridadEmpleados.Repositories;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NetCoreSeguridadEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryHospital repo;
        public EmpleadosController(RepositoryHospital repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<Empleado> empleados = await this.repo.GetEmpledosAsync();
            return View(empleados);
        }
        public async Task<IActionResult> Details(int idEmpleado)
        {
            Empleado empleado = await this.repo.FindEmpleado(idEmpleado);
            return View(empleado);
        }
        [AuthorizeEmpleados]
        public IActionResult PerfilEmpleado()
        {

            return View();
        }

        [AuthorizeEmpleados]
        public async Task <IActionResult> Compis()
        {
            //RECUPERAMOS EL CLAIM DEL USUARIO VALIDADO
            string dato = 
                HttpContext.User.FindFirstValue("Departamento");
            int idDepartamento = int.Parse(dato);
            List<Empleado> compañeros = 
                await this.repo.GetEmpleadosDepartamento(idDepartamento);
            return View(compañeros);
        }
    }
}
