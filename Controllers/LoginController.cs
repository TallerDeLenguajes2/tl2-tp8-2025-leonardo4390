using System;
using Microsoft.AspNetCore.Mvc;

public class LoginController : Controller
{
    private readonly IAuthenticationService _authenticationService;

    public LoginController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    // GET: /Login
    [HttpGet]
    public IActionResult Index()
    {
        return View(new LoginViewModel());
    }

    // POST: /Login/Login
    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", model);
        }

        // Validar credenciales
        if (_authenticationService.Login(model.Username, model.Password))
        {
            return RedirectToAction("Index", "Home");
        }

        model.ErrorMessage = "Credenciales inválidas.";
        return View("Index", model);
    }

    // Cerrar sesión
    [HttpGet]
    public IActionResult Logout()
    {
        _authenticationService.Logout();
        return RedirectToAction("Index");
    }
}