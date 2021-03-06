﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoneySeller.Controllers
{
    public class GestionController : Controller
    {
        [HttpPost]
        public ActionResult Profil(String TB_Fullname, String TB_Adresse, String TB_Ville, String TB_Telephone, String TB_Password, String TB_PasswordConfirm)
        {
            PoneySeller.Models.Users desUsagers = new PoneySeller.Models.Users(Session["MyPonies"]);
            desUsagers.SQLTableName = "Usagers";

            desUsagers.SelectByFieldName("Email", Session["Username"].ToString());

            desUsagers.Next();

            if (TB_Fullname != "" && TB_Adresse != "" && TB_Ville != "" && TB_Telephone != "" && TB_Password != "" && TB_PasswordConfirm != "")
            {
                desUsagers.usager.NomComplet = TB_Fullname;
                desUsagers.usager.Adresse = TB_Adresse;
                desUsagers.usager.Ville = TB_Ville;
                desUsagers.usager.Telephone = TB_Telephone;
                desUsagers.usager.Password = TB_Password;

                desUsagers.Update();

                ViewBag.UpdateSuccess = "Votre compte a été modifié";
            }
            else
            {
                ViewBag.ErreurVide = "Tout les champs doivent être remplis et au moins une photo doit être choisi.";
            }
            return View(new PoneySeller.Models.Jumbotron());
        }

        public ActionResult Profil()
        {
            if ((bool)Session["UserValid"])
            {
                PoneySeller.Models.Users desUsagers = new PoneySeller.Models.Users(Session["MyPonies"]);
                desUsagers.SQLTableName = "Usagers";

                desUsagers.SelectByFieldName("Email", Session["Username"].ToString());

                desUsagers.Next();

                ModelState.SetModelValue("TB_Fullname", new ValueProviderResult(desUsagers.usager.NomComplet, string.Empty, new CultureInfo("en-US")));
                ModelState.SetModelValue("TB_Adresse", new ValueProviderResult(desUsagers.usager.Adresse, string.Empty, new CultureInfo("en-US")));
                ModelState.SetModelValue("TB_Ville", new ValueProviderResult(desUsagers.usager.Ville, string.Empty, new CultureInfo("en-US")));
                ModelState.SetModelValue("TB_Telephone", new ValueProviderResult(desUsagers.usager.Telephone, string.Empty, new CultureInfo("en-US")));
                ModelState.SetModelValue("TB_Password", new ValueProviderResult(desUsagers.usager.Password, string.Empty, new CultureInfo("en-US")));
                ModelState.SetModelValue("TB_PasswordConfirm", new ValueProviderResult(desUsagers.usager.Password, string.Empty, new CultureInfo("en-US")));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new PoneySeller.Models.Jumbotron());
        }

        public ActionResult Gestion(int? _idcheval)
        {
            ViewBag.Update = "non";
            PoneySeller.Models.Users desUsagers = new PoneySeller.Models.Users(Session["MyPonies"]);
            string commande = "select id from chevaux where id = " + _idcheval + "and idproprio = (select id from usagers where email = '" + Session["Username"].ToString() + "')";
            if (Session["Username"].ToString() == "" && !(bool)Session["UserValid"] && !desUsagers.ExecuteCommandBoolReturn(commande))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.IdDuCheval = 1;

            if (_idcheval != null)
            {
                ViewBag.Update = "oui";
                int ID = _idcheval.GetValueOrDefault();
                ViewBag.IdDuCheval = ID;
                String[] InfoCheval = desUsagers.GetInfoChevaux(ID);

                ModelState.SetModelValue("TB_Nom", new ValueProviderResult(InfoCheval[0], string.Empty, new CultureInfo("en-US")));
                ModelState.SetModelValue("TB_Age", new ValueProviderResult(InfoCheval[1], string.Empty, new CultureInfo("en-US")));
                ModelState.SetModelValue("RB_Sexe", new ValueProviderResult(InfoCheval[2], string.Empty, new CultureInfo("en-US")));
                ModelState.SetModelValue("TB_Prix", new ValueProviderResult(InfoCheval[3], string.Empty, new CultureInfo("en-US")));
                ModelState.SetModelValue("TB_Race", new ValueProviderResult(InfoCheval[4], string.Empty, new CultureInfo("en-US")));
                ViewBag.Image1 = InfoCheval[5];
            }
            else
            {
                ViewBag.Image1 = "BasePicture.png";
            }
            return View(new PoneySeller.Models.Jumbotron());
        }

        [HttpPost]
        public ActionResult Gestion(String TB_Nom, String TB_Age, String RB_Sexe, String TB_Prix, HttpPostedFileBase FileUpload1, String TB_Update, String TB_IDCheval,
                                    String CB_Discipline, String TB_Race, String btn)
        {
            if (btn == "Enregistrer")
            {
                if (TB_Nom != "" && TB_Age != "" && RB_Sexe != "" && TB_Prix != "" && CB_Discipline != "" && TB_Race != "")
                {
                    string commandesql = "";
                    if (TB_Update == "non")
                    {
                        PoneySeller.Models.Users desUsagers = new PoneySeller.Models.Users(Session["MyPonies"]);
                        int IDProprio = desUsagers.GetID(Session["Username"].ToString());
                        commandesql = "insert into chevaux (nom, age, race, prix, sexe, idproprio, photoguid) values ( '" + TB_Nom + "'," + TB_Age + ",'" + TB_Race + "', " + TB_Prix + ", '" + RB_Sexe + "', " + IDProprio + ", '" + FileUpload1.FileName.ToString() + "')";
                        if (ExecuteCommande(commandesql) > 0)
                        {
                            ViewBag.Reussi = "Cheval ajouté !";
                        }
                        else
                        {
                            ViewBag.NonReussi = "Erreur dans l'enregistrement du cheval !";
                        }
                    }
                    else if (TB_Update == "oui")
                    {
                        commandesql = "update chevaux set nom = '" + TB_Nom.ToString() + "', " +
                                                         "race = '" + TB_Race.ToString() + "', " +
                                                         "age = " + TB_Age.ToString() + ", " +
                                                         "prix = '" + TB_Prix.ToString() + "', " +
                                                         "sexe = '" + RB_Sexe.ToString() + "', " +
                                                         "photoguid = '" + FileUpload1.FileName.ToString() + "' " +
                                                         "where ID = " + TB_IDCheval;

                        if (ExecuteCommande(commandesql) > 0)
                        {
                            ViewBag.Reussi = "Cheval Modifié !";
                        }
                        else
                        {
                            ViewBag.NonReussi = "Erreur dans la modifcation du cheval !";
                        }
                    }
                }
                else
                {
                    ViewBag.ErreurVide = "Tout les champs doivent être remplis et au moins une photo doit être choisi.";
                }
            }
            else if (btn == "Supprimer")
            {
                string commandesql = "DELETE from Chevaux where id = " + TB_IDCheval;
                if (ExecuteCommande(commandesql) > 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.NonReussi = "Erreur dans la suppression du cheval !";
                }
            }


            return View(new PoneySeller.Models.Jumbotron());
        }

        int ExecuteCommande(string cmd)
        {
            int LigneModifie = 0;
            PoneySeller.Models.Users desUsagers = new PoneySeller.Models.Users(Session["MyPonies"]);

            LigneModifie = desUsagers.ExecuteCommandIntReturn(cmd);

            return LigneModifie;
        }
    }
}