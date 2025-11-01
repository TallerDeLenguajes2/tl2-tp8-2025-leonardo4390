using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_leonardo4390.Models;

namespace tl2_tp8_2025_leonardo4390.Controllers;

public class ProductosController : Controller
{
    private readonly IRepository<Productos> _repositoryProductos;

    public ProductosController()
    {
        _repositoryProductos = new ProductoRepository();
    }

    public IActionResult Index()
    {
        List<Productos> productos = _repositoryProductos.GetAll();
        return View(productos);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
