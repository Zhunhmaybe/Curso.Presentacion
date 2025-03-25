using System.Text;
using Newtonsoft.Json;

namespace Curso.ConsumeAPI
{
    public static class CRUD<T>
    {
        public static T Created(string urlApi, T data)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi);
                client.DefaultRequestHeaders.Accept.Add(
                    System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json")
                );
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                var request = new HttpRequestMessage(HttpMethod.Post, urlApi);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");


                var response = client.SendAsync(request);
                response.Wait();

                json = response.Result.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<T>(json);

                return result;
            }
        }
        public static T[] Read(string urlApi)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = client.GetStringAsync(urlApi);
                    response.Wait();

                    var json = response.Result;
                    var result = JsonConvert.DeserializeObject<T[]>(json);
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw new Exception("La API está cargando o ha ocurrido un error", ex);
            }
        }
        public static T Read_ById(string urlApi, int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = client.GetStringAsync(urlApi + "/" + id);
                response.Wait();

                var json = response.Result;
                var result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }

        }
        public static bool Update(string urlApi, int id, T data)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi);
                client.DefaultRequestHeaders.Accept.Add(
                    System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json")
                );
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                var request = new HttpRequestMessage(HttpMethod.Put, urlApi + "/" + id);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = client.SendAsync(request);
                response.Wait();

                json = response.Result.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<T>(json);

                if (response.Result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static bool Delete(string urlApi, int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi);
                client.DefaultRequestHeaders.Accept.Add(
                    System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json")
                );

                var request = new HttpRequestMessage(HttpMethod.Delete, urlApi + "/" + id);

                var response = client.SendAsync(request);
                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
