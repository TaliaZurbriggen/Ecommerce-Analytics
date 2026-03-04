using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using EcommerceETL.Models.ApiModels;

namespace EcommerceETL.Services
{
    public class ExtractService
    {
        private readonly HttpClient _httpClient;
        private const string BASE_URL = "https://dummyjson.com";

        public ExtractService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<ApiProduct>> ObtenerProductosAsync()
        {
            Console.WriteLine("Extrayendo productos...");

            var json = await _httpClient.GetStringAsync($"{BASE_URL}/products?limit=0"); //la api trae hasta 30 por defecto, por eso pongo limit=0, para que los traiga todos
            var response = JsonConvert.DeserializeObject<ProductsResponse>(json);

            Console.WriteLine($"{response!.Products.Count} productos obtenidos");
            return response.Products;
        }

        public async Task<List<ApiCart>> ObtenerCarritosAsync()
        {
            Console.WriteLine("Extrayendo carritos...");

            var json = await _httpClient.GetStringAsync($"{BASE_URL}/carts?limit=0");
            var response = JsonConvert.DeserializeObject<CartsResponse>(json);

            Console.WriteLine($"{response!.Carts.Count} carritos obtenidos");
            return response.Carts;
        }

        public async Task<List<ApiUser>> ObtenerUsuariosAsync()
        {
            Console.WriteLine("Extrayendo usuarios...");

            var json = await _httpClient.GetStringAsync($"{BASE_URL}/users?limit=0");
            var response = JsonConvert.DeserializeObject<UsersResponse>(json);

            Console.WriteLine($"{response!.Users.Count} usuarios obtenidos");
            return response.Users;
        }
    }
}
