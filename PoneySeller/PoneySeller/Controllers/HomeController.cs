using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using PoneySeller.Controllers;
using PoneySeller.Models;
using System.IO;
using System.Dynamic;
using PoneySeller.Class;

namespace PoneySeller.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new PoneySeller.Models.Jumbotron());
        }

        public ActionResult Deconnection()
        {
            Session["UserValid"] = false;
            Session["Username"] = "";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Index(string TB_Username, string TB_Password, string TB_UsernameRemind, string btn_login, string btn_send)
        {
            PoneySeller.Models.Users desUsagers = new PoneySeller.Models.Users(Session["MyPonies"]);
            if (!string.IsNullOrWhiteSpace(btn_login))
            {
                bool connexionreussite = true;
                if (TB_Username == "" || TB_Password == "")
                {
                    ViewBag.ErrorTBVide = "Tout les champs doivent être remplis";
                    connexionreussite = false;
                }
                if (!desUsagers.VerifierConnection(TB_Username, TB_Password))
                {
                    ViewBag.ErrorConnexion = "Informations invalides. ";
                    connexionreussite = false;
                }

                if (connexionreussite)
                {
                    Session["UserValid"] = true;
                    Session["Username"] = TB_Username;
                }
            }
            else if (!string.IsNullOrWhiteSpace(btn_send))
            {
                bool envoitreussi = true;

                if (TB_UsernameRemind == "")
                {
                    envoitreussi = false;
                    ViewBag.ErreurVide = "Tout les champs doivent être remplis";
                }
                if (desUsagers.ExecuteCommandBoolReturn("SELECT * FROM USAGERS WHERE EMAIL ='" + TB_Username + "'"))
                {
                    envoitreussi = false;
                    ViewBag.ErreurUsername = "Informations invalides.";
                }

                if (envoitreussi)
                {
                    if (EnvoyerLeEmail(TB_UsernameRemind, desUsagers))
                    {
                        ViewBag.Reussi = "Un message a été envoyé à votre adresse courriel.";
                    }
                    else
                    {
                        ViewBag.NonReussi = "Erreur dans l'envoit du message.";
                    }
                }
            }

            return View(new PoneySeller.Models.Jumbotron());
        }

        private bool EnvoyerLeEmail(string TB_Username, PoneySeller.Models.Users unUser)
        {
            string unEmail = TB_Username;
            string Password = unUser.TrouverPassword(TB_Username);
            bool reussi = false;

            if (unEmail != "" && Password != "")
            {
                PoneySeller.Class.Email eMail = new PoneySeller.Class.Email();

                // Vous devez avoir un compte gmail
                eMail.From = "tp1aspemailsender@gmail.com";
                eMail.Password = "melissa1dominic";
                eMail.SenderName = "Melissa et Charie";

                eMail.Host = "smtp.gmail.com";
                eMail.HostPort = 587;
                eMail.SSLSecurity = true;

                eMail.To = unEmail;
                eMail.Subject = "Rappel de Mot de Passe";
                eMail.Body = "Votre mot de passe est le suivant : "
                            + Password
                            + "<br/><br/> Attention de ne pas oublier trop souvent ! <br/> "
                            + "Melissa et Charlie";

                if (eMail.Send())
                    reussi = true;
                else
                    reussi = false;
            }
            else
            {
                reussi = false;
            }
            return reussi;
        }

        [HttpPost]
        public ActionResult Inscription(string TB_Telephone, string TB_FullName, string TB_Adresse, string TB_Ville, string TB_Password, string TB_PasswordConfirm, string TB_Email, string TB_EmailConfirm)
        {
            PoneySeller.Models.Users desUsagers = new PoneySeller.Models.Users(Session["MyPonies"]);
            bool inscriptionreussite = true;
            bool TextBoxRempli = false;
            if (TB_Password != TB_PasswordConfirm)
            {
                ViewBag.ErrorMDP = "Les mots de passe doivent correspondre";
                inscriptionreussite = false;
            }
            if (TB_Email != TB_EmailConfirm)
            {
                ViewBag.ErrorEmail = "Les emails doivent correspondre";
                inscriptionreussite = false;
            }
            if (desUsagers.ExecuteCommandBoolReturn("SELECT * FROM USAGERS WHERE EMAIL ='" + TB_Email + "'"))
            {
                ViewBag.NomUtilise = "Email deja utilisé";
                inscriptionreussite = false;
            }
            if (TB_Telephone != "" && TB_Ville != "" && TB_Adresse != "" && TB_FullName != "" && TB_Email != "" && TB_EmailConfirm != "" && TB_PasswordConfirm != "" && TB_Password != "")
            {
                TextBoxRempli = true;
            }
            else
            {
                ViewBag.ErrorTBVide = "Tout les champs doivent être remplis";
                inscriptionreussite = false;
            }
            
            if (inscriptionreussite && TextBoxRempli)
            {
                string sqlcommand = "insert into usagers (nomcomplet, password, adresse, ville, telephone, email) values (" 
                                  + "'" + TB_FullName + "','" + TB_Password + "','" + TB_Adresse 
                                  + "','" + TB_Ville + "','" + TB_Telephone + "','" + TB_Email + "')";
                if (desUsagers.ExecuteCommandIntReturn(sqlcommand) > 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.NonReussi = "Inscription non reussite";
                }
            }
            return View(new PoneySeller.Models.Jumbotron());
        }

        public ActionResult Inscription()
        {
            return View(new PoneySeller.Models.Jumbotron());
        }
    }
}