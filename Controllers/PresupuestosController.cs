using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_leonardo4390.Models;

namespace tl2_tp8_2025_leonardo4390.Controllers;

public class PresupuestosController : Controller
{
    private readonly IRepository<Presupuestos> _repositoryPresupuestos;

    public PresupuestosController()
    {
        _repositoryPresupuestos = new PresupuestosRepository();
    }
    
    public IActionResult Index()
    {
        List<Presupuestos> presupuestos = _repositoryPresupuestos.GetAll();
        return View(presupuestos);
    }

    public IActionResult Detalles(int id)
    {
        var presupuesto = _repositoryPresupuestos.GetById(id);
        if (presupuesto == null)
        {
            return NotFound();
        }

        return View(presupuesto);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
