using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MiPrimeraAplicacion
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient _httpClient;
        private static readonly Random _random = new Random();

        public MainPage()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
        }

        private async void OnFetchPokemonClicked(object sender, EventArgs e)
        {
            string apiUrl = "https://pokeapi.co/api/v2/pokemon?limit=10";

            try
            {
                var response = await _httpClient.GetStringAsync(apiUrl);
                var pokemonList = JsonConvert.DeserializeObject<PokemonApiResponse>(response);

                PokemonCardsContainer.Children.Clear();

                foreach (var pokemon in pokemonList.Results)
                {
                    await AddPokemonCard(pokemon.Url);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Error fetching Pokémon data: " + ex.Message, "OK");
            }
        }

        private async void OnFetchRandomPokemonClicked(object sender, EventArgs e)
        {
            int randomId = _random.Next(1, 500);
            string apiUrl = $"https://pokeapi.co/api/v2/pokemon/{randomId}";

            try
            {
                await AddPokemonCard(apiUrl);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Error fetching Pokémon data: " + ex.Message, "OK");
            }
        }

        private void OnClearListClicked(object sender, EventArgs e)
        {
            PokemonCardsContainer.Children.Clear();
        }

        private async Task AddPokemonCard(string pokemonUrl)
        {
            var pokemonDetails = await _httpClient.GetStringAsync(pokemonUrl);
            var pokemonData = JsonConvert.DeserializeObject<PokemonDetails>(pokemonDetails);

            var card = new Frame
            {
                BorderColor = Colors.Gray,
                CornerRadius = 10,
                Padding = 10,
                Margin = 5,
                BackgroundColor = Colors.LightYellow,
                Content = new VerticalStackLayout
                {
                    Children =
                    {
                        new Label { Text = $"Name: {pokemonData.Name}", FontSize = 18, FontAttributes = FontAttributes.Bold, TextColor = Colors.DarkBlue },
                        new Label { Text = $"Weight: {pokemonData.Weight}", FontSize = 14 },
                        new Label { Text = $"Height: {pokemonData.Height}", FontSize = 14 },
                        new Image { Source = pokemonData.Sprites.FrontDefault, HeightRequest = 100, WidthRequest = 100, HorizontalOptions = LayoutOptions.Center }
                    }
                }
            };

            PokemonCardsContainer.Children.Add(card);
        }
    }

    // Clases para deserializar las respuestas de la API
    public class PokemonApiResponse
    {
        [JsonProperty("results")]
        public List<PokemonResult> Results { get; set; }
    }

    public class PokemonResult
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class PokemonDetails
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("weight")]
        public int Weight { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("sprites")]
        public PokemonSprites Sprites { get; set; }
    }

    public class PokemonSprites
    {
        [JsonProperty("front_default")]
        public string FrontDefault { get; set; }
    }
}
