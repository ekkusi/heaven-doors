using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ChatDescrimintionPreventorProj
{
    public class ChatDiscriminationPreventor
    {

        private const string API_URL = "http://95.179.229.215:3000/evaluate";
        private float detectionThreshold;
        private HttpClient httpClient;
        private ProfanityFilter.ProfanityFilter profanityFilter;

        public ChatDiscriminationPreventor(float detectionThreshold = 0.8f)
        {
            this.detectionThreshold = detectionThreshold;
            profanityFilter = new ProfanityFilter.ProfanityFilter();
            this.httpClient = new HttpClient();
        }


        public async Task<HttpResponseMessage> CheckHatespeech(string message)
        {
            //await PostJsonContent(API_URL, httpClient);
            var postUser = new User { message = message, analyze = true };
            HttpResponseMessage result = await httpClient.PostAsJsonAsync(API_URL, postUser);
            //var result = await httpClient.GetAsync(API_URL);

            return result;//new DiscriminationResult("This text contains hate speech. You are a fucking idiot", true);
        }

        public List<string> DetectProfanitiesSync(string sentence)
        {
            var swearList = profanityFilter.DetectAllProfanities(sentence);

            List<string> r = new List<string>();

            foreach (var item in swearList)
            {
                r.Add(item);
            }

            return r;
        }
    }

    public class User
    {
        public string message { get; set; }
        public bool analyze { get; set; }
    }

    public struct DiscriminationResult
    {
        public string message;
        public bool containsHatespeech;
        public DiscriminationResult(string message, bool containsHatespeech)
        {
            this.message = message;
            this.containsHatespeech = containsHatespeech;
        }
    }

}
