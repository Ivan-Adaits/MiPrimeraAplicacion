using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json; // Necesitas agregar este paquete para manejar JSON

namespace MiPrimeraAplicacion
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient _httpClient;

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
                // Obtenemos la lista de Pokémon desde la API
                var response = await _httpClient.GetStringAsync(apiUrl);
                var pokemonList = JsonConvert.DeserializeObject<PokemonApiResponse>(response);

                // Limpiamos el contenedor de las tarjetas
                PokemonCardsContainer.Children.Clear();

                // Para cada Pokémon, obtenemos detalles adicionales
                foreach (var pokemon in pokemonList.Results)
                {
                    var pokemonDetails = await _httpClient.GetStringAsync(pokemon.Url);
                    var pokemonData = JsonConvert.DeserializeObject<PokemonDetails>(pokemonDetails);

                    // Creamos una tarjeta para mostrar los detalles
                    var card = new Frame
                    {
                        BorderColor = Colors.Gray,
                        CornerRadius = 10,
                        Padding = 10,
                        Margin = 5,
                        Content = new VerticalStackLayout
                        {
                            Children =
                            {
                                new Label { Text = $"Name: {pokemonData.Name}", FontSize = 18, FontAttributes = FontAttributes.Bold },
                                new Label { Text = $"Weight: {pokemonData.Weight}" },
                                new Label { Text = $"Height: {pokemonData.Height}" }
                            }
                        }
                    };

                    // Agregamos la tarjeta al contenedor
                    PokemonCardsContainer.Children.Add(card);
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                await DisplayAlert("Error", "Error fetching Pokémon data: " + ex.Message, "OK");
            }
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
    }
}