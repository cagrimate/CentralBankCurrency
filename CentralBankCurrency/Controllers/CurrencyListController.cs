using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Xml;

namespace CentralBankCurrency.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyListController : ControllerBase
    {
        [HttpGet]
        public List<dynamic> GetCurrencies()
        {
            List<dynamic> currencyName = new List<dynamic>();

            XmlDocument doviz = new XmlDocument();
            doviz.Load("https://www.tcmb.gov.tr/kurlar/today.xml");


            var currencies = doviz.SelectNodes($"//Currency");

            foreach (XmlNode currenciesNode in currencies)
            {
                string kod = currenciesNode.Attributes["Kod"].Value;
                string isim = currenciesNode.SelectSingleNode("Isim").InnerText;
                currencyName.Add(new { Name = isim, Code = kod });
            }
            return currencyName;
        }
    }
}
