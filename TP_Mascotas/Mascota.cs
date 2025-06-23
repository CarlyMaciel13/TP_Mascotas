using System.Xml.Serialization;
using System;

namespace TP_Mascotas
{
    // Se especifica que la clase base `Mascota` puede contener objetos de tipo `Perro` y `Gato` al momento de serializar.
    // Esto es obligatorio para que el XmlSerializer sepa cómo tratar con clases derivadas.
    [XmlInclude(typeof(Perro))]
    [XmlInclude(typeof(Gato))]
    public abstract class Mascota
    {
        // Identificador único de la mascota, se corresponde con el campo clave primaria en la base de datos.
        public int Id { get; set; }

        // Nombre de la mascota.
        public string Nombre { get; set; }

        // Especie de la mascota, por ejemplo “Perro” o “Gato”.
        public string Especie { get; set; }

        // Edad de la mascota en años.
        public int Edad { get; set; }

        // Indica si la mascota está vacunada (true/false).
        public bool Vacunada { get; set; }

        // Raza de la mascota, no es obligatorio completarlo.
        public string Raza { get; set; }

        // Constructor vacío requerido para que la clase pueda ser serializada correctamente.
        public Mascota() { }

        // Constructor parametrizado utilizado para inicializar objetos con valores.
        // También se llama al método que asigna una edad aleatoria entre 1 y 14 años.
        public Mascota(string nombre, string especie, bool vacunada, string raza = "")
        {
            Nombre = nombre;             // Asigna el nombre recibido al atributo.
            Especie = especie;           // Asigna la especie recibida al atributo.
            Vacunada = vacunada;         // Asigna el estado de vacunación.
            Raza = raza;                 // Asigna la raza, si se pasó algún valor.
            AsignarEdadAleatoria();      // Llama al método que genera la edad aleatoria.
        }

        // Este método asigna una edad aleatoria entre 1 y 14 años a la mascota.
        // Utiliza la clase Random del sistema.
        public void AsignarEdadAleatoria()
        {
            Random random = new Random();        // Se instancia un generador de números aleatorios.
            Edad = random.Next(1, 15);           // Se asigna un número aleatorio entre 1 y 14 inclusive.
        }

        // Método virtual que se puede sobrescribir desde las clases hijas (Perro, Gato).
        // Muestra por consola los datos de la mascota formateados en una línea.
        public virtual void MostrarInfo()
        {
            Console.WriteLine($"Nombre: {Nombre}, Especie: {Especie}, Edad: {Edad}, Vacunada: {Vacunada}, Raza: {Raza}");
        }
    }
}

