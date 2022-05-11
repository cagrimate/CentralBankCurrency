using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Xml;

namespace CentralBankCurrency.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetCurrency(decimal amount, string from, string to)
        {
            XmlDocument doviz = new XmlDocument();
            doviz.Load("https://www.tcmb.gov.tr/kurlar/today.xml");

            var kodFrom = doviz.SelectSingleNode($"//Currency[@Kod=\"{from}\"]/ForexBuying");
            var kodTo = doviz.SelectSingleNode($"//Currency[@Kod=\"{to}\"]/ForexBuying");
            if (kodFrom == null || kodTo == null)
            {
                return this.StatusCode(406, "Girilen Para Birimi Hatalı");
            }
            else
            {
                decimal valueFrom = Convert.ToDecimal(kodFrom.InnerText);
                decimal valueTo = Convert.ToDecimal(kodTo.InnerText);
                decimal sonuc = Math.Round(((valueFrom / valueTo) * amount), 2);

                string sonucYeni = String.Format("{0:#,0.000}", sonuc);
                string amountYeni = String.Format("{0:#,0.000}", amount);
                string text = $"{amountYeni} {from} Equals To {sonucYeni}{to}";


                return this.StatusCode(200, new { Amount = amount, From = from, Sonuc = sonuc, To = to, Text = text });
            }


        }
    }
}
