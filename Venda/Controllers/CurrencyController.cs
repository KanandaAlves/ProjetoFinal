using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace Pecas2.Controllers
{
    public class CurrencyController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetDollarRate()
        {
            string url = "https://economia.awesomeapi.com.br/last/USD-BRL";

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetStringAsync(url);
                    var jsonResponse = JObject.Parse(response);

                    var dollarRate = jsonResponse["USDBRL"]?["bid"]?.ToString();

                    if (!string.IsNullOrEmpty(dollarRate))
                    {
                        return Json(new { rate = dollarRate });
                    }
                    return Json(new { message = "Taxa não disponível" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = $"Erro ao obter taxa: {ex.Message}" });
            }
        }
    }
}
