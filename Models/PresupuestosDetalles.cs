using System;

public class PresupuestosDetalles
{
    public Productos Producto { get; set; }
    public int Cantidad { get; set; }
    public PresupuestosDetalles() { }

    public PresupuestosDetalles(Productos producto, int cantidad)
    {
        Producto = producto;
        Cantidad = cantidad;
    }

}