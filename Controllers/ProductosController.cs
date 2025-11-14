using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_leonardo4390.Models;

namespace tl2_tp8_2025_leonardo4390.Controllers;

public class ProductosController : Controller
{
    private readonly IProductoRepository _repositoryProductos;
    private readonly IAuthenticationService _authService;

    public ProductosController(
        IProductoRepository productoRepo,
        IAuthenticationService authService)
    {
        _repositoryProductos = productoRepo;
        _authService = authService;
    }

    public IActionResult Index()
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;

        List<Productos> productos = _repositoryProductos.GetAll();
        return View(productos);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        return View();
    }

    [HttpPost]
    public IActionResult Create(Productos producto)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        if (ModelState.IsValid)
        {
            _repositoryProductos.Create(producto);
            return RedirectToAction(nameof(Index));
        }
        return View(producto);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        var producto = _repositoryProductos.GetById(id);
        if (producto == null)
        {
            return NotFound();
        }
        return View(producto);
    }

    [HttpPost]
    public IActionResult Edit(int id, Productos producto)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        if (id != producto.IdProducto)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            _repositoryProductos.Update(producto);
            return RedirectToAction(nameof(Index));
        }
        return View(producto);
    }

    [HttpGet]
    public IActionResult Delet(int id)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;

        var producto = _repositoryProductos.GetById(id);
        if (producto == null)
        {
            return NotFound();
        }
        return View(producto);
    }

    [HttpPost, ActionName("Delet")]
    public IActionResult DeletConfirirmed(int id)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        _repositoryProductos.Remove(id);
        return RedirectToAction(nameof(Index));
    }
    
    private IActionResult? CheckAdminPermissions()
    {
        // 1. Usuario NO logueado
        if (!_authService.IsAuthenticated())
            return RedirectToAction("Index", "Login");

        // 2. Usuario logueado pero NO Admin
        if (!_authService.HasAccessLevel("Administrador"))
            return RedirectToAction(nameof(AccesoDenegado));

        return null; // OK, permitido
    }
    public IActionResult AccesoDenegado()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
