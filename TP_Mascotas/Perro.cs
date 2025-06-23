using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Mascotas
{
    public class Perro : Mascota, IVacunable
    {
        // Constructor sin parámetros para XML
        public Perro() : base() { }

        // Constructor con parámetros para uso normal
        public Perro(string nombre, bool vacunada, string raza = "")
            : base(nombre, "Perro", vacunada, raza)
        {
        }

        public bool EstaVacunada()
        {
            return Vacunada;
        }

        public override void MostrarInfo()
        {
            Console.WriteLine($"[PERRO] Nombre: {Nombre}, Edad: {Edad}, Vacunada: {Vacunada}, Raza: {Raza}");
        }
    }


}
