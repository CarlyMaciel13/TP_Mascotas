// Estas líneas de arriba traen herramientas que vamos a usar en el programa
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization; // Esto sirve para guardar datos en archivos XML
using System.IO;                // Esto nos deja leer y escribir archivos (como .xml)

namespace TP_Mascotas // Nombre del proyecto o grupo de archivos
{
    internal class Program // Esta es la clase principal, donde arranca todo
    {
        // Creamos un objeto que se va a encargar de hablar con la base de datos
        static MascotaBD baseDeDatos = new MascotaBD();

        static void Main(string[] args) // Acá empieza el programa
        {
            int opcion; // Acá vamos a guardar la opción que elija el usuario

            do
            {
                // Mostramos el menú con todas las opciones
                Console.WriteLine("\n===== MENÚ DE MASCOTAS =====");
                Console.WriteLine("1. Agregar Mascota");
                Console.WriteLine("2. Mostrar Mascotas");
                Console.WriteLine("3. Editar Mascota");
                Console.WriteLine("4. Eliminar Mascota");
                Console.WriteLine("5. Contar Mascotas Vacunadas");
                Console.WriteLine("6. Salir");
                Console.Write("Ingrese una opción: ");

                // Leemos lo que escribió el usuario y lo pasamos a número
                int.TryParse(Console.ReadLine(), out opcion);
                Console.WriteLine();

                // Según lo que elija, hacemos algo distinto
                switch (opcion)
                {
                    case 1:
                        AgregarMascota(); // Llama a la función que agrega una mascota
                        break;
                    case 2:
                        MostrarMascotas(); // Muestra todas las mascotas que están en la base
                        break;
                    case 3:
                        EditarMascota(); // Cambia los datos de una mascota
                        break;
                    case 4:
                        EliminarMascota(); // Borra una mascota
                        break;
                    case 5:
                        ContarVacunadas(); // Cuenta cuántas están vacunadas
                        break;
                    case 6:
                        Console.WriteLine("Saliendo..."); // Cierra el programa
                        break;
                    default:
                        Console.WriteLine("Opción inválida."); // Si escribió cualquier cosa
                        break;
                }

            } while (opcion != 6); // Mientras no elija salir, sigue el menú
        }

        // Función para agregar una mascota nueva
        static void AgregarMascota()
        {
            // Pedimos los datos uno por uno
            Console.Write("Nombre: ");
            string nombre = Console.ReadLine();

            Console.Write("Especie (Perro/Gato): ");
            string especie = Console.ReadLine();

            Console.Write("Vacunada? (s/n): ");
            bool vacunada = Console.ReadLine().ToLower() == "s"; // Si puso “s”, asumimos que está vacunada

            Console.Write("Raza (opcional): ");
            string raza = Console.ReadLine();

            Mascota m; // Acá vamos a guardar el objeto que creemos

            // Dependiendo de lo que escribió, creamos un Perro o un Gato
            if (especie.ToLower() == "perro" || especie.ToLower() == "p")
                m = new Perro(nombre, vacunada, raza);
            else if (especie.ToLower() == "gato" || especie.ToLower() == "g")
                m = new Gato(nombre, vacunada, raza);
            else
            {
                Console.WriteLine("Especie no válida.");
                return; // Si puso algo raro, salimos y no hacemos nada
            }

            baseDeDatos.AgregarMascota(m); // Mandamos la mascota a la base de datos
            Console.WriteLine("Mascota agregada con éxito.");

            GuardarMascotasEnXmlDesdeBD(); // También actualizamos el archivo XML
        }

        // Función para mostrar todas las mascotas
        static void MostrarMascotas()
        {
            var mascotas = baseDeDatos.GetMascotas(); // Pedimos la lista a la base
            Console.WriteLine("=== Lista de Mascotas ===");

            foreach (var m in mascotas)
            {
                Console.Write("ID: " + m.Id + " - "); // Mostramos el ID para saber cuál es cuál
                m.MostrarInfo(); // Mostramos el resto de los datos
            }
        }

        // Función para editar una mascota existente
        static void EditarMascota()
        {
            MostrarMascotas(); // Primero mostramos las que hay, así elegís cuál

            Console.Write("Ingrese el ID de la mascota a editar: ");
            int.TryParse(Console.ReadLine(), out int id); // Leemos el ID

            // Nuevos datos que querés poner
            Console.Write("Nuevo nombre: ");
            string nombre = Console.ReadLine();

            Console.Write("Nueva especie (Perro/Gato): ");
            string especie = Console.ReadLine();

            Console.Write("¿Vacunada? (s/n): ");
            bool vacunada = Console.ReadLine().ToLower() == "s";

            Console.Write("Nueva raza: ");
            string raza = Console.ReadLine();

            Mascota m;

            // Creamos la nueva mascota con los datos editados
            if (especie.ToLower() == "perro")
                m = new Perro(nombre, vacunada, raza);
            else if (especie.ToLower() == "gato")
                m = new Gato(nombre, vacunada, raza);
            else
            {
                Console.WriteLine("Especie inválida.");
                return;
            }

            m.Id = id; // Le ponemos el mismo ID para que reemplace a la anterior
            baseDeDatos.EditarMascota(m); // Guardamos los cambios en la base
            Console.WriteLine("Mascota editada con éxito.");

            GuardarMascotasEnXmlDesdeBD(); // Actualizamos el XML también
        }

        // Función para eliminar una mascota
        static void EliminarMascota()
        {
            MostrarMascotas(); // Mostramos la lista antes de borrar

            Console.Write("Ingrese el ID de la mascota a eliminar: ");
            int.TryParse(Console.ReadLine(), out int id); // Leemos el ID

            baseDeDatos.EliminarMascota(id); // Borramos la mascota
            Console.WriteLine("Mascota eliminada con éxito.");

            GuardarMascotasEnXmlDesdeBD(); // Actualizamos el XML
        }

        // Función para contar cuántas mascotas están vacunadas
        static void ContarVacunadas()
        {
            var mascotas = baseDeDatos.GetMascotas(); // Traemos la lista
            int total = 0;

            // Recorremos una por una
            foreach (var m in mascotas)
            {
                // Si la mascota puede ser vacunada y lo está, sumamos
                if (m is IVacunable vac)
                {
                    if (vac.EstaVacunada()) total++;
                }
            }

            Console.WriteLine($"Cantidad de mascotas vacunadas: {total}");
        }

        // Función que guarda las mascotas en un archivo XML
        static void GuardarMascotasEnXmlDesdeBD()
        {
            var mascotas = baseDeDatos.GetMascotas(); // Pedimos los datos actualizados

            try
            {
                // Preparamos el serializador con las clases posibles (Perro y Gato)
                XmlSerializer serializer = new XmlSerializer(
                    typeof(List<Mascota>),
                    new Type[] { typeof(Perro), typeof(Gato) }
                );

                // Abrimos el archivo para escribir
                using (TextWriter writer = new StreamWriter("mascotas.xml"))
                {
                    serializer.Serialize(writer, mascotas); // Guardamos todo en el archivo
                }

                Console.WriteLine("Mascotas guardadas en mascotas.xml");
            }
            catch (Exception ex)
            {
                // Si pasa algo raro, mostramos el error
                Console.WriteLine("Error al guardar en XML: " + ex.Message);
            }
        }
    }
}
