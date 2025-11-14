using System;
using System.Collections.Generic;

public interface IPresupuestoRepository
{
    List<Presupuestos> GetAll();
    Presupuestos GetById(int id);
    void Create(Presupuestos presupuesto);
    void Update(Presupuestos presupuesto);
    void Remove(int id);

    void AgregarDetalle(int idPresupuesto, int idProducto, int cantidad);
}