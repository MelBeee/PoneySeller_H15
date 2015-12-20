using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlExpressUtilities;

namespace PoneySeller.Models
{
    public class User
    {
        int ID { get; set; }
        public string Email { get; set; }
        public string Adresse { get; set; }
        public string Ville { get; set; }
        public string Telephone { get; set; }
        public string Password { get; set; }
        public string NomComplet { get; set; }

        public User() { }

        public User(string sNomComplet, string sAdresse, string sVille, string sTelephone, string sPassword, string sEmail) 
        {
            Email = sEmail;
            Adresse = sAdresse;
            Ville = sVille;
            Telephone = sTelephone;
            Password = sPassword;
            NomComplet = sNomComplet;
        }
    }

    public class Users : SqlExpressUtilities.SqlExpressWrapper
    {
        public User usager { get; set; }
        
        public Users(object cs) : base(cs)
        {
            usager = new User();
        }

        public Users() 
        {
            usager = new User(); 
        }
    } 
}