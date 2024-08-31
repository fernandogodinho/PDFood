using pdfood.api.DTO.FakeStore;
using System.Text.Json;

namespace pdfood.api.Services
{
    public class FakeStoreService
    {
        private readonly HttpClient _httpClient;

        public FakeStoreService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<FSProductDTO> CreateProductAsync(FSProductDTO dto)
        {
            using HttpResponseMessage response = await _httpClient.PostAsync("products", SerializeJson(dto));

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadFromJsonAsync<FSProductDTO>();

            return jsonResponse;
        }


        public async Task UpdateProductAsync(FSProductDTO dto)
        {
            using HttpResponseMessage response = await _httpClient.PutAsync($"products/{dto.Id}", SerializeJson(dto));

            response.EnsureSuccessStatusCode();
        }

        private StringContent SerializeJson(FSProductDTO dto)
        {
            return new StringContent(
                JsonSerializer.Serialize(
                    new
                    {
                        dto
                    }));
        }
    }
}
