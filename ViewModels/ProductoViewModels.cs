using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tl2_tp8_2025_leonardo4390.Models;

public class ProductoViewModels
{
    public int IdProducto { get; set; }
    [StringLength(250, ErrorMessage = "No puede los 250 caracteres")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "Campo obligatorio")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El valor debe ser mayour a cero")]
    public decimal Precio{ get; set; }
    
}