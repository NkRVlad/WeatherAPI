using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Weather_Forecast.Controllers
{
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private string Key = "51b6d812b01690f6ac09ded7415ea7da";

        private readonly ILogger<WeatherForecastController> _logger;
      
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("api/GetCurrentWeather")]
        public async Task<IActionResult> GetCurrentWeather(string City)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://api.openweathermap.org");

                    var response = await client.GetAsync($"/data/2.5/weather?q={City}&appid={Key}&units=metric");

                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();

                    var rawWeather = JsonConvert.DeserializeObject<List>(stringResult);

                    return Ok(new
                    {
                        name = rawWeather.Name,
                        Date = Convert.ToDateTime(new DateTime(1970, 1, 1).AddSeconds(rawWeather.dt)).ToShortDateString(),
                        temp = rawWeather.Main.temp,
                        temp_max = rawWeather.Main.temp_max,
                        temo_min = rawWeather.Main.temp_min,
                        speed_wind = rawWeather.Wind.speed,
                        clouds = rawWeather.Clouds.all
                    });
                }catch(HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                }
            }
        }

        [HttpGet]
        [Route("api/GetForecast")]
        public async Task<IActionResult> GetForecast(string City)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://api.openweathermap.org");

                    var response = await client.GetAsync($"/data/2.5/forecast?q={City}&appid={Key}&units=metric");

                    response.EnsureSuccessStatusCode();

                    List<List> model = new List<List>();

                    var stringResult = await response.Content.ReadAsStringAsync();

                    List<List> result = JObject.Parse(stringResult)["list"].ToObject<List<List>>();

                    foreach (List i in result)
                    {
                        List list = new List()
                        {
                            Name = City,
                            Date = Convert.ToDateTime(new DateTime(1970, 1, 1).AddSeconds(i.dt)).ToString(),
                            Main = new Main
                            {
                                temp = i.Main.temp,
                                temp_max = i.Main.temp_max,
                                temp_min = i.Main.temp_min
                            },
                            Wind = new Wind { speed = i.Wind.speed },
                            Clouds = new Clouds { all = i.Clouds.all }
                         };
                        model.Add(list);
                    }
                    return Ok(new List<List>(model));

                }catch(HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                }
            }

        }
    }
}
