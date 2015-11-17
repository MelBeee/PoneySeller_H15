using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoneySeller.Models
{
    public class ImageModel 
    {
        public int IDImage { get; set; }

        public ImageModel()
        {
            IDImage = 1;
            Refresh();
        }

        public void Refresh()
        {
            IDImage = GetAleatoire();
        }

        public int GetAleatoire()
        {
            Random rdn = new Random();
            int math = rdn.Next(1, 10);


            return math;
        }
	}
}