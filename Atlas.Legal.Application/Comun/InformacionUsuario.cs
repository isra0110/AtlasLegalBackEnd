using System;
using System.Collections.Generic;
using System.Text;

namespace Atlas.Legal.Comun
{
    [Serializable]
    public class InformacionUsuario
    {
        public int IdUsuario { get; set; }        
        public string Usuario { get; set; }
        public int IdRol { get; set; }
    }
}
