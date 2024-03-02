using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using InsightBytes.Utilities.Endpoints;

using Newtonsoft.Json;

namespace InsightBytes.BlackBox;

public class Sandwich
{
    public string Name { get; set; }
    public string Message { get; set; }

    public Sandwich(string name, string message)
    {
        Name = name;
        Message = message;
    }
}


internal class Program
{
    private readonly HttpClient _httpClient;

    public Program() { _httpClient = new HttpClient(); }

    static async Task Main(string[] args)
    {
        var newSandwich = new Sandwich("Ham and Cheese", "I love ham and cheese sandwiches.");
        var program = new Program();
        await program.PostSandwich(newSandwich);

        var sandwich = await program.GetSandwichByNameAsync("Ham and Cheese");
       
        if(sandwich != null)
        {
            Console.WriteLine($"{sandwich.Name} : {sandwich.Message}");
        } else
        {
            Console.WriteLine("No sandwich found.");
        }
 

        //await program.SendOfferAsync(sdpOffer);
        //int num = 0;
        //while (num < 9000)
        //{
        //    var sandwichFromGin = await program.GetSandwich();
        //    Console.WriteLine(sandwichFromGin);
        //    num++;
        //}
    }
    ///path?id=1234&name=Manu&value=
    public async Task PostSandwich(Sandwich sandwichData)
    {
        var resp = await _httpClient.PostAsync("http://localhost:8080/sandwich", new StringContent(JsonConvert.SerializeObject(sandwichData), Encoding.UTF8, "application/json"));
        if(!resp.IsSuccessStatusCode)
        {
            Console.WriteLine("Failed to post sandwich.");
        }

        Console.WriteLine(resp.StatusCode);
    }

    public async Task<Sandwich> GetSandwichByNameAsync(string name)
    {
        var resp = await _httpClient.GetAsync($"http://localhost:8080/sandwich/{name}");
        if (resp.IsSuccessStatusCode)
        {
            var sandwich = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Sandwich>(sandwich);
        }

        return null;
    }

    public async Task<string> GetSandwich()
    {
        var resp = await _httpClient.GetAsync("http://localhost:8080/sandwich");
        if (resp.IsSuccessStatusCode)
        {
            var sandwich = await resp.Content.ReadAsStringAsync();
            return sandwich;
        }


        return "No Sandwich";
    }


    public async Task SendOfferAsync(string offer)
    {
        var requestUri = "http://localhost:8080/offer";
        var offerData = new { sdp = offer }; // Assuming offer is your SDP string
        var jsonContent = JsonConvert.SerializeObject(offerData);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync(requestUri, content);
        if(response.IsSuccessStatusCode)
        {
            Console.WriteLine("Offer sent successfully.");
            // Optionally, read the response
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
        } else
        {
            Console.WriteLine("Failed to send offer.");
        }
    }

    public async Task<string> GetOfferOrAnswerAsync(string sessionId)
    {
        var requestUri = $"http://localhost:8080/offer/{sessionId}";
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);
        if(response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Received response: " + responseBody);
            return responseBody;
        } else
        {
            Console.WriteLine("Failed to receive offer/answer.");
            return null;
        }
    }
}

