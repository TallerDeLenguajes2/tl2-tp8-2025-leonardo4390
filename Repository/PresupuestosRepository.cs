using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

public class PresupuestosRepository:IRepository<Presupuestos>
{
    private string cadenaConexion = "Data Source=DB/Tienda.db";

    public void Create(Presupuestos presupuesto)
    {
        var query = @"INSERT INTO Presupuestos (nombreDestinatario, fechaCreacion) 
                      VALUES (@nombre, @fecha)";
        using (var connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            var command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@nombre", presupuesto.NombreDestinatario));
            command.Parameters.Add(new SqliteParameter("@fecha", presupuesto.FechaCreacion));
            command.ExecuteNonQuery();

            command.CommandText = "SELECT last_insert_rowid()";
            presupuesto.IdPresupuesto = Convert.ToInt32(command.ExecuteScalar());

            foreach (var item in presupuesto.Detalle)
            {
                var detalleCmd = new SqliteCommand(@"INSERT INTO PresupuestosDetalles
                    (idPresupuesto, idProducto, cantidad) VALUES (@idPresupuesto, @idProducto, @cantidad)", connection);
                detalleCmd.Parameters.Add(new SqliteParameter("@idPresupuesto", presupuesto.IdPresupuesto));
                detalleCmd.Parameters.Add(new SqliteParameter("@idProducto", item.Producto.IdProducto));
                detalleCmd.Parameters.Add(new SqliteParameter("@cantidad", item.Cantidad));
                detalleCmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public List<Presupuestos> GetAll()
    {
        var lista = new List<Presupuestos>();
        var query = "SELECT * FROM Presupuestos";

        using (var connection = new SqliteConnection(cadenaConexion))
        {
            var command = new SqliteCommand(query, connection);
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var presupuesto = new Presupuestos
                    {
                        IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]),
                        NombreDestinatario = reader["nombreDestinatario"].ToString(),
                        FechaCreacion = Convert.ToDateTime(reader["fechaCreacion"]),
                        Detalle = new List<PresupuestosDetalles>()
                    };
                    lista.Add(presupuesto);
                }
            }
            connection.Close();
        }

        return lista;
    }

    public Presupuestos GetById(int id)
    {
        var presupuesto = new Presupuestos();
        var queryPresupuesto = "SELECT * FROM Presupuestos WHERE idPresupuesto = @id";
        var queryDetalle = @"SELECT pd.cantidad, p.idProducto, p.Descripcion, p.Precio 
                             FROM PresupuestosDetalles pd 
                             JOIN Productos p ON pd.idProducto = p.idProducto 
                             WHERE pd.idPresupuesto = @id";

        using (var connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();

            var command = new SqliteCommand(queryPresupuesto, connection);
            command.Parameters.Add(new SqliteParameter("@id", id));
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                    presupuesto.NombreDestinatario = reader["nombreDestinatario"].ToString();
                    presupuesto.FechaCreacion = Convert.ToDateTime(reader["fechaCreacion"]);
                    presupuesto.Detalle = new List<PresupuestosDetalles>();
                }
            }

            var detalleCmd = new SqliteCommand(queryDetalle, connection);
            detalleCmd.Parameters.Add(new SqliteParameter("@id", id));
            using (var reader = detalleCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var producto = new Productos
                    {
                        IdProducto = Convert.ToInt32(reader["idProducto"]),
                        Descripcion = reader["Descripcion"].ToString(),
                        Precio = Convert.ToInt32(reader["Precio"])
                    };
                    var detalle = new PresupuestosDetalles(producto, Convert.ToInt32(reader["cantidad"]));
                    presupuesto.Detalle.Add(detalle);
                }
            }

            connection.Close();
        }

        return presupuesto;
    }

    public void AgregarDetalle(int idPresupuesto, int idProducto, int cantidad)
    {
        var query = @"INSERT INTO PresupuestosDetalles (idPresupuesto, idProducto, cantidad) 
                  VALUES (@idPresupuesto, @idProducto, @cantidad)";

        using (var connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();

            var command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
            command.Parameters.Add(new SqliteParameter("@idProducto", idProducto));
            command.Parameters.Add(new SqliteParameter("@cantidad", cantidad));

            command.ExecuteNonQuery();
        }
    }



    public void Remove(int idPresupuesto)
    {
        using (var connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            var cmdDetalle = connection.CreateCommand();
            cmdDetalle.CommandText = "DELETE FROM PresupuestosDetalles WHERE IdPresupuesto = @id";
            cmdDetalle.Parameters.Add(new SqliteParameter("@id", idPresupuesto));
            cmdDetalle.ExecuteNonQuery();
            var cmdPresupuesto = connection.CreateCommand();
            cmdPresupuesto.CommandText = "DELETE FROM Presupuestos WHERE IdPresupuesto = @id";
            cmdPresupuesto.Parameters.Add(new SqliteParameter("@id", idPresupuesto));
            cmdPresupuesto.ExecuteNonQuery();

            connection.Close();
        }
    }

    public void Update(Presupuestos presupuesto)
    {
        var query = @"UPDATE Presupuestos SET nombreDestinatario = @nombre, fechaCreacion = @fecha 
                  WHERE idPresupuesto = @id";

        using (var connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            var command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@nombre", presupuesto.NombreDestinatario));
            command.Parameters.Add(new SqliteParameter("@fecha", presupuesto.FechaCreacion));
            command.Parameters.Add(new SqliteParameter("@id", presupuesto.IdPresupuesto));
            command.ExecuteNonQuery();
        }

    }

}