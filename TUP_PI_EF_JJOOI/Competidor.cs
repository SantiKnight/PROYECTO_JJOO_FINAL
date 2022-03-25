using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUP_PI_EF_JJOOI
{
    class Competidor
    {
        private int numero;
        private string nombre;
        private int disciplina;
        private string sexo;
        private DateTime fechaNacimiento;
        public int pNumero
        {
            get { return numero; }
            set { numero = value; }
        }
        public string pNombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
        public int pDisciplina
        {
            get { return disciplina; }
            set { disciplina = value; }
        }
        public string pSexo
        {
            get { return sexo; }
            set { sexo = value; }
        }
        public DateTime pFechaNacimiento
        {
            get { return fechaNacimiento; }
            set { fechaNacimiento = value; }
        }
        public Competidor()
        {
            this.numero = 0;
            this.nombre = "";
            this.disciplina = 0;
            this.sexo = "";
            this.fechaNacimiento = DateTime.Today;
        }
        public Competidor(int numero, string nombre,int disciplina, string sexo,DateTime fechaNacimiento)
        {
            this.numero = numero;
            this.nombre = nombre;
            this.disciplina = disciplina;
            this.sexo = sexo;
            this.fechaNacimiento = fechaNacimiento;
        }
        public override string ToString()
        {
            return numero + " - " + nombre;
        }
    }
}
