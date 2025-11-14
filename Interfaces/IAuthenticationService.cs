using System;

 public interface IAuthenticationService
{
    // Verifica credenciales y genera sesión
    bool Login(string username, string password);

    // Cierra la sesión del usuario
    void Logout();

    // Indica si actualmente hay un usuario autenticado
    bool IsAuthenticated();

    // Verifica si el usuario autenticado tiene el rol requerido
    bool HasAccessLevel(string requiredAccessLevel);
}