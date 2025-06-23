// Importación de espacios de nombres necesarios para trabajar con funcionalidades básicas de C#
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Definición del espacio de nombres del proyecto
namespace TP_Mascotas
{
    // Vuelve a importar algunos namespaces dentro del scope del namespace (innecesario si ya están arriba, pero no genera error)
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    // Clase encargada de interactuar con la base de datos de mascotas
    public class MascotaBD
    {
        // Cadena de conexión al servidor SQL Server local, utilizando autenticación integrada de Windows
        private string connectionString = @"Server=LAPTOP-8GCRCA54\SQLEXPRESS01;Database=MascotasDB;Trusted_Connection=True;";

        // Método público que inserta una nueva mascota en la tabla Mascota
        public void AgregarMascota(Mascota m)
        {
            // Se instancia un objeto de conexión con la cadena definida
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                // Se abre la conexión con la base de datos
                conn.Open();

                // Consulta SQL con parámetros para evitar inyección de SQL
                string consulta = "INSERT INTO Mascota (nombre, especie, edad, vacunada, raza) VALUES (@nombre, @especie, @edad, @vacunada, @raza)";

                // Se crea el comando SQL que ejecutará la consulta anterior
                SqlCommand comando = new SqlCommand(consulta, conn);

                // Se asignan valores a cada parámetro con los atributos del objeto recibido
                comando.Parameters.AddWithValue("@nombre", m.Nombre);
                comando.Parameters.AddWithValue("@especie", m.Especie);
                comando.Parameters.AddWithValue("@edad", m.Edad);
                comando.Parameters.AddWithValue("@vacunada", m.Vacunada);
                comando.Parameters.AddWithValue("@raza", m.Raza);

                // Se ejecuta la consulta SQL (no retorna datos)
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Captura y muestra cualquier error ocurrido durante la inserción
                Console.WriteLine("Error al agregar la mascota: " + ex.Message);
            }
            finally
            {
                // Cierra la conexión, tanto si hubo error como si no
                conn.Close();
            }
        }

        // Método que actualiza una mascota existente en la base de datos
        public void EditarMascota(Mascota m)
        {
            // Se instancia la conexión SQL
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                // Se abre la conexión
                conn.Open();

                // Consulta SQL parametrizada para actualizar los campos de una mascota por ID
                string consulta = "UPDATE Mascota SET nombre = @nombre, especie = @especie, edad = @edad, vacunada = @vacunada, raza = @raza WHERE id = @id";

                // Se crea el comando SQL con la conexión activa
                SqlCommand comando = new SqlCommand(consulta, conn);

                // Se asignan valores a los parámetros desde el objeto recibido
                comando.Parameters.AddWithValue("@id", m.Id);
                comando.Parameters.AddWithValue("@nombre", m.Nombre);
                comando.Parameters.AddWithValue("@especie", m.Especie);
                comando.Parameters.AddWithValue("@edad", m.Edad);
                comando.Parameters.AddWithValue("@vacunada", m.Vacunada);
                comando.Parameters.AddWithValue("@raza", m.Raza);

                // Se ejecuta la actualización en la base de datos
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // En caso de error, se informa al usuario
                Console.WriteLine("Error al editar la mascota: " + ex.Message);
            }
            finally
            {
                // Se asegura que la conexión se cierre
                conn.Close();
            }
        }

        // Método que elimina una mascota por su ID
        public void EliminarMascota(int id)
        {
            // Se instancia la conexión SQL
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                // Abre la conexión
                conn.Open();

                // Consulta SQL parametrizada para eliminar por ID
                string consulta = "DELETE FROM Mascota WHERE id = @id";

                // Se crea y configura el comando SQL
                SqlCommand comando = new SqlCommand(consulta, conn);
                comando.Parameters.AddWithValue("@id", id);

                // Se ejecuta la eliminación
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Se notifica cualquier error
                Console.WriteLine("Error al eliminar la mascota: " + ex.Message);
            }
            finally
            {
                // Se cierra la conexión
                conn.Close();
            }
        }

        // Método que obtiene todas las mascotas almacenadas en la base de datos
        public List<Mascota> GetMascotas()
        {
            // Se declara la lista que se va a retornar
            List<Mascota> lista = new List<Mascota>();

            // Se establece la conexión con SQL Server
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                // Se abre la conexión
                conn.Open();

                // Consulta SQL para seleccionar todos los registros de la tabla Mascota
                string consulta = "SELECT * FROM Mascota";
                SqlCommand comando = new SqlCommand(consulta, conn);

                // Se ejecuta la consulta y se obtiene un lector para recorrer los resultados
                SqlDataReader lector = comando.ExecuteReader();

                // Iteración sobre cada fila de resultados
                while (lector.Read())
                {
                    // Se obtienen los valores de cada columna de forma tipada
                    string especie = lector["especie"].ToString();
                    string nombre = lector["nombre"].ToString();
                    bool vacunada = (bool)lector["vacunada"];
                    string raza = lector["raza"].ToString();
                    int edad = (int)lector["edad"];
                    int id = (int)lector["id"];

                    Mascota m;

                    // Según la especie, se instancia la subclase correspondiente
                    if (especie == "Perro")
                        m = new Perro(nombre, vacunada, raza);
                    else if (especie == "Gato")
                        m = new Gato(nombre, vacunada, raza);
                    else
                        continue; // Si la especie no es conocida, se ignora el registro

                    // Se asignan las propiedades restantes
                    m.Id = id;
                    m.Edad = edad;

                    // Se agrega la mascota a la lista
                    lista.Add(m);
                }
            }
            catch (Exception ex)
            {
                // Se informa cualquier error en la obtención
                Console.WriteLine("Error al obtener las mascotas: " + ex.Message);
            }
            finally
            {
                // Se cierra la conexión en todos los casos
                conn.Close();
            }

            // Se retorna la lista completa de mascotas recuperadas
            return lista;
        }
    }

}
