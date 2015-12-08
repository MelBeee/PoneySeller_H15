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
        string NomComplet { get; set; }
        string Adresse { get; set; }
        string Ville { get; set; }
        string Telephone { get; set; }
        string Password { get; set; }
        string Email { get; set; }

        public User() { }

        public User(string sNomComplet, string sAdresse, string sVille, string sTelephone, string sPassword, string sEmail) 
        {
            NomComplet = sNomComplet;
            Adresse = sAdresse;
            Ville = sVille;
            Telephone = sTelephone;
            Password = sPassword;
            Email = sEmail;
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