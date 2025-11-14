using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using tl2_tp8_2025_leonardo4390.Models;

namespace tl2_tp8_2025_leonardo4390.Controllers;

public class PresupuestosController : Controller
{
    private readonly IPresupuestoRepository _repositoryPresupuestos;
    private readonly IProductoRepository _repositoryProductos;
    private readonly IAuthenticationService _authService;

     public PresupuestosController(
        IPresupuestoRepository presupuestoRepo,
        IProductoRepository productoRepo,
        IAuthenticationService authService)
    {
        _repositoryPresupuestos = presupuestoRepo;
        _repositoryProductos = productoRepo;
        _authService = authService;
    }
    
    public IActionResult Index()
    {
        var check = CheckReadPermissions();
        if (check != null) return check;
        List<Presupuestos> presupuestos = _repositoryPresupuestos.GetAll();
        return View(presupuestos);
    }

    public IActionResult Detalles(int id)
    {
        var check = CheckReadPermissions();
        if (check != null) return check;
        var presupuesto = _repositoryPresupuestos.GetById(id);
        if (presupuesto == null)
        {
            return NotFound();
        }

        return View(presupuesto);
    }


    [HttpGet]
    public IActionResult Create()
    {
        var check = CheckAdminPermissions();
        if (check != null) return check;
        var vm = new PresupuestoViewModels
        {
            FechaCreacion = DateTime.Now
        };

        return View(vm);
    }


    [HttpPost]
    public IActionResult Create(PresupuestoViewModels vm)
    {
        var check = CheckAdminPermissions();
        if (check != null) return check;
        if (!ModelState.IsValid)
            return View(vm);

        var presupuesto = new Presupuestos
        {
            NombreDestinatario = vm.NombreDestinatario ?? vm.Email,
            FechaCreacion = vm.FechaCreacion,
            Detalle = new List<PresupuestosDetalles>()
        };

        _repositoryPresupuestos.Create(presupuesto);
        
        return RedirectToAction(nameof(Index));

    }



    [HttpGet]
    public IActionResult Edit(int id)
    {
        var check = CheckAdminPermissions();
        if (check != null) return check;
        var presupuesto = _repositoryPresupuestos.GetById(id);
        if (presupuesto == null)
        {
            return NotFound();
        }
        var vm = new PresupuestoViewModels
        {
            IdPresupuesto = presupuesto.IdPresupuesto,
            NombreDestinatario = presupuesto.NombreDestinatario,
            FechaCreacion = presupuesto.FechaCreacion

        };
        return View(vm);
    }

    [HttpPost]
    public IActionResult Edit(int id, PresupuestoViewModels vm)
    {
        var check = CheckAdminPermissions();
        if (check != null) return check;
        if (id != vm.IdPresupuesto)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var presupuesto = new Presupuestos
        {
            IdPresupuesto = vm.IdPresupuesto,
            NombreDestinatario = vm.NombreDestinatario ?? vm.Email,
            FechaCreacion = vm.FechaCreacion
        };
        _repositoryPresupuestos.Update(presupuesto);
        return RedirectToAction("AgregarProducto", new { idPresupuesto = presupuesto.IdPresupuesto });
    }

    [HttpGet]
    public IActionResult AgregarProducto(int id)
    {
        var check = CheckAdminPermissions();
        if (check != null) return check;
        var productos = _repositoryProductos.GetAll();

        var model = new AgregarProductoViewModels
        {
            IdPresupuesto = id,
            ListaProductos = new SelectList(productos, "IdProducto", "Descripcion")
        };

        return View(model);
    }


    [HttpPost]
    public IActionResult AgregarProducto(AgregarProductoViewModels model)
    {
        var check = CheckAdminPermissions();
        if (check != null) return check;

        if (!ModelState.IsValid)
        {
            var productos = _repositoryProductos.GetAll();
            model.ListaProductos = new SelectList(productos, "IdProducto", "Descripcion");
            return View(model);
        }

        _repositoryPresupuestos.AgregarDetalle(model.IdPresupuesto, model.IdProducto, model.Cantidad);

        return RedirectToAction(nameof(Detalles), new { id = model.IdPresupuesto });
    }
    
    [HttpGet]
    public IActionResult Delet(int id)
    {
        var check = CheckAdminPermissions();
        if (check != null) return check;


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
        var check = CheckAdminPermissions();
        if (check != null) return check;
        _repositoryPresupuestos.Remove(id);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult AccesoDenegado()
    {
        return View();
    }

    private IActionResult? CheckReadPermissions()
    {
        if (!_authService.IsAuthenticated())
            return RedirectToAction("Index", "Login");

        if (_authService.HasAccessLevel("Administrador") ||
            _authService.HasAccessLevel("Cliente"))
            return null; // OK

        return RedirectToAction(nameof(AccesoDenegado));
    }

    private IActionResult? CheckAdminPermissions()
    {
        if (!_authService.IsAuthenticated())
            return RedirectToAction("Index", "Login");

        if (!_authService.HasAccessLevel("Administrador"))
            return RedirectToAction(nameof(AccesoDenegado));

        return null;
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
