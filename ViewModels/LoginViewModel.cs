using System.ComponentModel.DataAnnotations;
using System;

public class LoginViewModel
{
    [Required(ErrorMessage = "Debe ingresar un nombre de usuario")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Debe ingresar una contraseña")]
    public string Password { get; set; }

    public string? ErrorMessage { get; set; }
}