using Microsoft.AspNetCore.Mvc;
using StoreApiIntegration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace DemoApplication.Areas.Client.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("~/")]
        public async Task<IActionResult> IndexAsync([FromServices] IHttpClientFactory httpClientFactory)
        {
            var httpClient = httpClientFactory.CreateClient();

            //var requestMessage = new HttpRequestMessage
            //{
            //    Method = HttpMethod.Get,
            //    RequestUri = new Uri("https://fakestoreapi.com/products")
            //};

            //var responseMessage = await httpClient.SendAsync(requestMessage);

            var responseMessage = await httpClient.GetAsync("https://fakestoreapi.com/products"); //short-version of Send
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                var content = await responseMessage.Content.ReadAsStringAsync();

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                return Ok(JsonSerializer.Deserialize<List<ProductDto>>(content, options));
            }

            return Ok(new List<ProductDto>() { });
        }
    }
}
