using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ChatDescrimintionPreventorProj
{
    public enum DiscriminationType { OBSCENE, IDENTITY, THREAT, LIGHT }

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

        public async Task<DiscriminationResult> CheckHatespeech(string message)
        {
            var postUser = new User { message = message, analyze = true };
            HttpResponseMessage result = await httpClient.PostAsJsonAsync(API_URL, postUser);

            return new DiscriminationResult(
                (string)json["message"],
                (float)json["evaluation_results"]["toxicity"] > detectionThreshold,
                (float)json["evaluation_results"]["severe_toxicity"] > detectionThreshold,
                (float)json["evaluation_results"]["obscene"] > detectionThreshold,
                (float)json["evaluation_results"]["identity_attack"] > detectionThreshold,
                (float)json["evaluation_results"]["insult"] > detectionThreshold,
                (float)json["evaluation_results"]["threat"] > detectionThreshold,
                (float)json["evaluation_results"]["sexual_explicit"] > detectionThreshold,
                (string)json["adviceMessage"]
            );
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
        public bool isToxic;
        public bool isServerelyToxic;
        public bool isObscene;
        public bool isIdentityAttack;
        public bool isInsult;
        public bool isThreat;
        public bool isSexualExplicit;
        public string adviceMessage;

        public DiscriminationResult(string message,
            bool isToxic,
            bool isServerelyToxic,
            bool isObscene,
            bool isIdentityAttack,
            bool isInsult,
            bool isThreat,
            bool isSexualExplicit,
            string adviceMessage)
        {
            this.message = message;
            this.isToxic = isToxic;
            this.isServerelyToxic = isServerelyToxic;
            this.isObscene = isObscene;
            this.isIdentityAttack = isIdentityAttack;
            this.isInsult = isInsult;
            this.isThreat = isThreat;
            this.isSexualExplicit = isSexualExplicit;
            this.adviceMessage = adviceMessage;
        }
    }

}
