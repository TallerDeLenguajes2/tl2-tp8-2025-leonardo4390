using System;

public interface IRepository<T>
{
    public List<T> GetAll();

    public T GetById(int id);

    public void Create(T entidad);
    public void Remove(int id);
    public void Update(T entidad);
    public void AgregarDetalle(int idPre, int idPro, int cantidad);
}