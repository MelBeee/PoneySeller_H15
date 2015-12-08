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
                ModelState.SetModelValue("RB_Race", new ValueProviderResult(InfoCheval[4], string.Empty, new CultureInfo("en-US")));
                ViewBag.Image1 = "BasePicture.png";
                if (InfoCheval[5] != "")
                {
                    ViewBag.Image1 = InfoCheval[5];
                }
            }
            else
            {
                ViewBag.Image1 = "BasePicture.png";
            }
            return View(new PoneySeller.Models.Jumbotron());
        }

        [HttpPost]
        public ActionResult Gestion(String TB_Nom, String TB_Age, String RB_Sexe, String TB_Prix, HttpPostedFileBase FileUpload1, String TB_Update, String TB_IDCheval,
                                    String CB_Discipline)
        {
            if (TB_Nom != "" && TB_Age != "" && RB_Sexe != "" && TB_Prix != "" && CB_Discipline != "")
            {
                string commandesql = "";
                if (TB_Update == "non")
                {
                    if (ExecuteCommande(commandesql))
                    {
                        ViewBag.Reussi = "Cheval ajouté !";
                    }
                    else
                    {
                        ViewBag.NonReussi = "Erreur dans l'enregistrement du cheval !";
                    }
                }
                else
                {
                    if (ExecuteCommande(commandesql))
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

            return View(new PoneySeller.Models.Jumbotron());
        }

        bool ExecuteCommande(string cmd)
        {



            return true;
        }
    }
}