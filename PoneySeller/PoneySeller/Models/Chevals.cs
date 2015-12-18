using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoneySeller.Models
{
    public class Cheval
    { 
        public int ID { get; set; }
        public string Nom { get; set; }
        public string IDRace { get; set; }
        public string Prix { get; set; }
        public string Sexe { get; set; }
        public string IDProprio { get; set; }

         public Cheval() { }

         public Cheval(string sNom, string sIDRace, string sPrix, string sSexe, string sIDProprio) 
         {
             Nom = sNom;
             IDRace = sIDRace;
             Prix = sPrix;
             Sexe = sSexe;
             IDProprio = sIDProprio;           
         }
     
    }

    public class Chevals : SqlExpressUtilities.SqlExpressWrapper
    {
        public Cheval cheval { get; set; }

        public Chevals(object cs)
            : base(cs)
        {
            cheval = new Cheval();
        }

        public Chevals()
        {
            cheval = new Cheval();
        }
    } 
}