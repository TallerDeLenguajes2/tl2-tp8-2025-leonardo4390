using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

public class ProductoRepository : IRepository<Productos>
{
    private string cadenaConexion = "Data Source=DB/Tienda.db";

    public List<Productos> GetAll()
    {
        var queryString = @"SELECT * FROM Productos";
        List<Productos> productos = new List<Productos>();

        using (var connection = new SqliteConnection(cadenaConexion))
        {
            var command = new SqliteCommand(queryString, connection);
            connection.Open();

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var producto = new Productos();
                    producto.IdProducto = Convert.ToInt32(reader["IdProducto"]);
                    producto.Descripcion = reader["Descripcion"].ToString();
                    producto.Precio = Convert.ToInt32(reader["Precio"]);
                    productos.Add(producto);
                }
            }

            connection.Close();
        }

        return productos;
    }


    public Productos GetById(int idProducto)
    {
        var connection = new SqliteConnection(cadenaConexion);
        var producto = new Productos();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Productos WHERE IdProducto = @idProducto";
        command.Parameters.Add(new SqliteParameter("@idProducto", idProducto));

        connection.Open();
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                producto.IdProducto = Convert.ToInt32(reader["IdProducto"]);
                producto.Descripcion = reader["Descripcion"].ToString();
                producto.Precio = Convert.ToInt32(reader["Precio"]);
            }
        }

        connection.Close();
        return producto;
    }

    public void Create(Productos producto)
    {
        var query = "INSERT INTO Productos (Descripcion, Precio) VALUES (@descripcion, @precio)";
        using (var connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            var command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@descripcion", producto.Descripcion));
            command.Parameters.Add(new SqliteParameter("@precio", producto.Precio));

            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void Update(Productos producto)
    {
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            var command = connection.CreateCommand();
            command.CommandText = @"UPDATE Productos 
                                    SET Descripcion = @descripcion, Precio = @precio 
                                    WHERE IdProducto = @id";

            command.Parameters.Add(new SqliteParameter("@id", producto.IdProducto));
            command.Parameters.Add(new SqliteParameter("@descripcion", producto.Descripcion));
            command.Parameters.Add(new SqliteParameter("@precio", producto.Precio));

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void Remove(int id)
    {
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Productos WHERE IdProducto = @id";
            command.Parameters.Add(new SqliteParameter("@id", id));

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void AgregarDetalle(int idPre, int idPro, int cantidad)
    {
         throw new NotImplementedException("Este método no se utiliza en ProductoRepository.");
    }

}