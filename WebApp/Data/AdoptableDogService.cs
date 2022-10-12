namespace WebApp.Data
{
    public class AdoptableDogService
    {
        public Task<AdoptableDog[]> GetDogsAsync()
        {
            using var client = new HttpClient();
            var builder = new UriBuilder("https://localhost:7271/AdoptableDog");            
            var url = builder.ToString();
            var res = client.GetStringAsync(url);
            var content = res.Result;
            List<AdoptableDog> dogs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AdoptableDog>>(content);
            return Task.FromResult(dogs.ToArray());
        }
    }
}