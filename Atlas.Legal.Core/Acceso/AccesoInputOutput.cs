using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Net;

namespace Atlas.Legal.Acceso
{
    public class ValidarAccesoWSInput
    {
        public string usuario { get; set; }
        public string password { get; set; }
        //public int funcion { get; set; }
    }

    public class ValidarAccesoWSOutput
    {
        public string access_token { get; set; }
        public DateTime expires_in { get; set; }
        public string token_type { get; set; }
        public string refresh_token { get; set; }
        public string refresh_token_expires_in { get; set; }
        public string path_default { get; set; }
        public HttpStatusCode HttpStatusCode{ get; set; }
    }

    
}
