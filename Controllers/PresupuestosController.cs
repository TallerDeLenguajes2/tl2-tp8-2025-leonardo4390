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


    public IActionResult Create()
    {
        var nuevoPresupuesto = new Presupuestos
        {
            FechaCreacion = DateTime.Now,
            Detalle = new List<PresupuestosDetalles>()
        };
        return View(nuevoPresupuesto);
    }

    [HttpPost]
    public IActionResult Create(Presupuestos presupuestos)
    {
        if (ModelState.IsValid)
        {
            _repositoryPresupuestos.Create(presupuestos);
            return RedirectToAction(nameof(Index));
        }
        return View(presupuestos);
    }

    public IActionResult Edit(int id)
    {
        var presupuesto = _repositoryPresupuestos.GetById(id);
        if (presupuesto == null)
        {
            return NotFound();
        }
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult Edit(int id, Presupuestos presupuestos)
    {
        if (id != presupuestos.IdPresupuesto)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            _repositoryPresupuestos.Update(presupuestos);
            return RedirectToAction(nameof(Index));
        }
        return View(presupuestos);
    }

    public IActionResult Delet(int id)
    {
        var producto = _repositoryPresupuestos.GetById(id);
        if (producto == null)
        {
            return NotFound();
        }
        return View(producto);
    }

    [HttpPost, ActionName("Delet")]
    public IActionResult DeletConfirirmed(int id)
    {
        _repositoryPresupuestos.Remove(id);
        return RedirectToAction(nameof(Index));
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
