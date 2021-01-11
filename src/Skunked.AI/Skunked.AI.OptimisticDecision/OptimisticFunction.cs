using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Skunked.PlayingCards;

namespace Skunked.AI.OptimisticDecision
{
    public static class OptimisticFunction
    {
        [FunctionName("Optimistic")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", "get", Route = null)] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var serializedCards = JsonConvert.DeserializeObject<List<SerializedCard>>(requestBody);
            var hand = serializedCards.Select(card => card.Create()).ToList();
            var toThrow = global::CardToss.maxAverage(hand);
            return new OkObjectResult(toThrow);
        }
    }

    public class SerializedCard
    {
        public Rank Rank { get; set; }
        public Suit Suit { get; set; }

        public Card Create()
        {
            return new Card(this.Rank, this.Suit);
        }
    }
}
