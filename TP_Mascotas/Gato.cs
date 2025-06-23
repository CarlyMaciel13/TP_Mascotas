using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Mascotas
{
    public class Gato : Mascota, IVacunable
    {
        public Gato() : base() { }

        public Gato(string nombre, bool vacunada, string raza = "")
            : base(nombre, "Gato", vacunada, raza)
        {
        }

        public bool EstaVacunada()
        {
            return Vacunada;
        }

        public override void MostrarInfo()
        {
            Console.WriteLine($"[GATO] Nombre: {Nombre}, Edad: {Edad}, Vacunada: {Vacunada}, Raza: {Raza}");
        }
    }

}
