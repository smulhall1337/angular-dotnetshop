using System.Net.Http.Headers;
using System.Net.Http.Json;
using Core.Entities.Dhr;
using Newtonsoft.Json;

namespace Infrastructure.Services
{
    public class GoogleGeoCoder
    {
        public string GetCounty(GeoCodeAddress address)
        {
            var queryString = address.Street.Replace(" ", "+") + ",+"
                            + address.City.Replace(" ", "+") + ",+"
                            + address.State.Replace(" ", "+") + "+"
                            + address.Zip.Replace(" ", "+");
            string query = "http://maps.googleapis.com/maps/api/geocode/json?sensor=false&address=" + queryString;

            var request = new
            {
                sensor = false,
                address = queryString
            };

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://maps.googleapis.com/maps/api/geocode/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.PostAsJsonAsync(query, request).Result;
            response.EnsureSuccessStatusCode();

            string jsonStr = response.Content.ReadAsStringAsync().Result;
            string county = ParseCountyFromJson(jsonStr);

            return county;
        }

        private string ParseCountyFromJson(string json)
        {
            try
            {
                dynamic jsonobject = JsonConvert.DeserializeObject(json);
                string county = "";

                if (jsonobject.status == "ZERO_RESULTS" || jsonobject.status == "OVER_QUERY_LIMIT")
                {
                    return null;
                }
                if (jsonobject != null && jsonobject.results.Count > 0)
                {
                    foreach (var element in jsonobject.results[0].address_components)
                    {
                        if (element.types[0] == "administrative_area_level_2")
                        {
                            county = element.long_name;
                        }
                    }
                }
                else
                {
                    return null;
                }


                county = county.Replace(" County", "").ToUpper();

                return county;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
