using Newtonsoft.Json;
using System.Text;

namespace ConsumirApiChatGPT5834163
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient _httpClient;
        private const string apiKey = "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";

        public MainPage()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
        }

        private async void OnSendMessage_Clicked(object sender, EventArgs e)
        {
            string userInput = userMessageEntry.Text;

            var response = await SendMessageToOpenAI(userInput);
            responseLabel.Text = response;
        }

        private async Task<string> SendMessageToOpenAI(string message)
        {
            var request = new
            {
                model = "GPT-4o",
                messages = new[]
                {
                    new { role = "system", content = "Eres un asistente útil." },
                    new { role = "user", content = message }
                }
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return $"Error: {responseContent}";
            }

            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
            return jsonResponse.choices[0].message.content.ToString();
        }
    }

}
