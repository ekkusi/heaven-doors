using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace HeavenDoors {
    public class ChatDiscriminationPreventor
    {
        private const string API_URL = "https://localhost:5000";
        private float detectionThreshold;
        private HttpClient httpClient;
        public ChatDiscriminationPreventor(float detectionThreshhold = 0.8f) {
            this.detectionThreshold = detectionThreshhold;
            this.httpClient = new HttpClient();
        }

        public async Task<DiscriminationResult> CheckHatespeech(string message) {
            var result = await httpClient.GetAsync(API_URL + "/detech-hate-speech");
            return new DiscriminationResult("This text contains hate speech. You are a fucking idiot", true);
        }
    }

    public struct DiscriminationResult {
        public string message;
        public bool containsHatespeech;
        public DiscriminationResult(string message, bool containsHatespeech) {
            this.message = message;
            this.containsHatespeech = containsHatespeech;
        }
    }
}


