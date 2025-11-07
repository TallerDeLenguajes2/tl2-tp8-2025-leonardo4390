using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using tl2_tp8_2025_leonardo4390.Models;

public class AgregarProductoViewModels
{
   [Required]
    public int IdPresupuesto { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un producto")]
    public int IdProducto { get; set; }

    [Required(ErrorMessage = "Debe ingresar una cantidad")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero")]
    public int Cantidad { get; set; }

    //public SelectList ListaProductos { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
    public SelectList? ListaProductos{ get; set; }

    
}