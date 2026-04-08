using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace ChatBotApp.Services
{
    public class ChatService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "xx";

        public ChatService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetBotResponse(string message)
        {
            var request = new
            {
                model = "gpt-5-nano",
                messages = new[]
                {
                    new { role = "user", content = message }
                }
            };

            var json = JsonConvert.SerializeObject(request);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            var response = await _httpClient.PostAsync(
                "https://api.openai.com/v1/chat/completions", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return $"OpenAI API Error ({response.StatusCode}): {error}";
            }
            var result = await response.Content.ReadAsStringAsync();

            dynamic data = JsonConvert.DeserializeObject(result);

            if (data == null || data.choices == null || data.choices.Count == 0)
            {
                return "Sorry, I could not get a response from OpenAI.";
            }

            return data.choices[0].message.content?.ToString() ?? "No content received";

        }
    }
}
