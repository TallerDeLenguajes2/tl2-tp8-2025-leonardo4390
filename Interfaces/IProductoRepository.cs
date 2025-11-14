using System;
using System.Collections.Generic;
public interface IProductoRepository
{
    List<Productos> GetAll();
    Productos GetById(int id);
    void Create(Productos producto);
    void Update(Productos producto);
    void Remove(int id);
}