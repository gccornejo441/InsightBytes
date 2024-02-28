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

internal class Program
{
    private readonly HttpClient _httpClient;

    public Program() { _httpClient = new HttpClient(); }

    static async Task Main(string[] args)
    {
        var program = new Program();

        var sdpOffer = @"v=0
o=- 5841829617509545627 1709000931 IN IP4 0.0.0.0
s=-
t=0 0
a=fingerprint:sha-256 52:74:14:CD:57:F7:20:26:D1:1E:C0:CC:5C:D4:61:A0:AF:16:61:BB:70:BE:70:5C:F8:B9:C4:D7:76:10:70:9D
a=extmap-allow-mixed
a=group:BUNDLE 0
m=application 9 UDP/DTLS/SCTP webrtc-datachannel
c=IN IP4 0.0.0.0
a=setup:actpass
a=mid:0
a=sendrecv
a=sctp-port:5000
a=ice-ufrag:xqpbWsfYSDiUJdbC
a=ice-pwd:MFTlHpwBEyxmFmcLFctEXCflyLUaaljX";

        await program.SendOfferAsync(sdpOffer);
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

