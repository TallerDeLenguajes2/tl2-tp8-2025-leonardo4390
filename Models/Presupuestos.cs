using System;

public class Presupuestos
{
    public int IdPresupuesto { get; set; }
    public string NombreDestinatario { get; set; }
    public DateTime FechaCreacion { get; set; }
    public List<PresupuestosDetalles> Detalle { get; set; } = new List<PresupuestosDetalles>();

    public string FechaCreacionFormateada => FechaCreacion.ToString("dd/MM/yyyy");
    public decimal MontoPresupuesto()
    {
        return Detalle.Sum(d => d.Cantidad * d.Producto.Precio);
    }

    public decimal MontoPresupuestoConIva()
    {
        return MontoPresupuesto() * 1.21m;
    }

    public int CantidadProductos()
    {
        return Detalle.Sum(d => d.Cantidad);
    }
}