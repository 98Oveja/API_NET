using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using API_REST_SP.Models;

namespace API_REST_SP.Controllers
{
    [Route("[controller]")]
    public class ProductosController : Controller
    {
        public readonly string con;

        public ProductosController(IConfiguration configuration)
        {
            con=configuration.getConnectionString("conexion");
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            List<Producto> productos = new();

            using(SqlConnection connection= new(con))
            {
                connection.Open();
                using(SqlCommand cmd=new("sp_obtener_productos",connection))
                {
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    using(SqlDataReader reader = cmd.ExecuteReader){
                        while(reader.Read())
                        {
                            Producto p = new Producto
                            {
                                Id= Convert.ToInt32(reader["Id"]),
                                Nombre=reader["Nombre"].ToString(),
                                Precio=Convert.ToDecimal(reader["Precio"]),
                                Cantidad=Convert.ToInt32(reader["Cantida"]),
                                Descripcion=reader["Descripcion"].ToString(),
                                FechaCreacion=Convert.ToDateTime(reader["FechaCreacion"])
                            };

                            productos.Add(p);
                        }
                    }
                }
            }
            return productos;
        }
        [HttpPost]
        public void Post([FromBody] Producto p)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd=new("sp_insertar_productos", connection))
                {
                    cmd.CommandType= System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("Nombre", p.Nombre);
                    cmd.Parameters.AddWithValue("Price", p.Price);
                    cmd.Parameters.AddWithValue("Cantidad", p.Cantidad);
                    cmd.Parameters.AddWithValue("Descripcion", p.Descripcion);
                    cmd.Parameters.AddWithValue("FechaCreacion", p.FechaCreacion);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        [HttpPut("{id}")]
        public void Put([FromBody] Producto p, int id)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd=new("sp_actualizar_productos", connection))
                {
                    cmd.CommandType= System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("Id", id);
                    cmd.Parameters.AddWithValue("Nombre", p.Nombre);
                    cmd.Parameters.AddWithValue("Price", p.Price);
                    cmd.Parameters.AddWithValue("Cantidad", p.Cantidad);
                    cmd.Parameters.AddWithValue("Descripcion", p.Descripcion);
                    cmd.Parameters.AddWithValue("FechaCreacion", p.FechaCreacion);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd=new("sp_eliminar_productos", connection))
                {
                    cmd.CommandType= System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}