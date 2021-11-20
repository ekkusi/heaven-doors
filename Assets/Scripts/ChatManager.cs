using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using ChatDescrimintionPreventorProj;
using UnityEngine;
using UnityEngine.UI;

namespace HeavenDoors
{
    public class ChatManager : MonoBehaviour
    {
        private List<string> PLAYER_NAMES = new List<string> {
            "Junior",
            "Dilly Dally",
            "Frauline",
            "Bubble Butt",
            "Frau Frau",
            "Miss Piggy",
            "Dreamey",
            "Bean",
            "Boomer",
            "Cello",
            "Thunder Thighs",
            "Butterbuns"
        };
        private Text inputText;
        [SerializeField]
        private InputField inputField;
        [SerializeField]
        private GameObject chatContainer;
        private VerticalLayoutGroup chatLayoutGroup;
        [SerializeField]
        private GameObject messagePrefab;
        // [SerializeField]
        // private Color sendButtonEnabledColor;
        // [SerializeField]
        // private Color sendButtonDisabledColor;
        [SerializeField]
        private Button sendButton;
        private string previousPlayerName;
        private int messageCount = 0;
        ChatDiscriminationPreventor discriminationPreventor;

        private void Start()
        {
            inputText = inputField.transform.Find("Text").GetComponent<Text>();
            chatLayoutGroup = chatContainer.GetComponent<VerticalLayoutGroup>();
            foreach (Transform child in chatContainer.transform)
            {
                Destroy(child.gameObject);
            }
            discriminationPreventor = new ChatDiscriminationPreventor();
            string profanitySentence = "You are fucking gay";
            Debug.Log(profanitySentence);
            var list = discriminationPreventor.DetectProfanitiesSync(profanitySentence);
            foreach (string profanity in list)
            {
                Debug.Log(profanity);
            }
        }

        private void Update()
        {
            if (inputText.text.Length == 0 && sendButton.interactable)
            {
                sendButton.interactable = false;
            }
            if (inputText.text.Length > 0 && !sendButton.interactable)
            {
                sendButton.interactable = true;
            }
            if (Input.GetKeyDown(KeyCode.Return) && inputText.text.Length > 0)
            {
                CreateChatMessage(inputText.text);
                inputField.SetTextWithoutNotify("");
            }
        }


        private void UpdateCanvas()
        {
            Canvas.ForceUpdateCanvases();
            chatLayoutGroup.enabled = false;
            chatLayoutGroup.enabled = true;
        }

        public void SendMessageClicked()
        {
            if (inputText.text.Length > 0)
            {
                CreateChatMessage(inputText.text);
                inputField.SetTextWithoutNotify("");
            }
        }

        private void CreateChatMessage(string message)
        {
            GameObject messageObj = Instantiate(messagePrefab, Vector3.zero, Quaternion.identity);
            messageObj.transform.SetParent(chatContainer.transform, false);
            string playerName = PLAYER_NAMES[Random.Range(0, PLAYER_NAMES.Count)];
            while (previousPlayerName == playerName)
            {
                playerName = PLAYER_NAMES[Random.Range(0, PLAYER_NAMES.Count)];
            }
            previousPlayerName = playerName;
            ChatMessageManager messageManager = messageObj.GetComponent<ChatMessageManager>();
            messageManager.InitializeMessage(playerName, message);
            messageCount++;
            StartCoroutine(CheckAndAlertDiscriminationAsync(messageManager, message).AsIEnumerator());
            UpdateCanvas();
        }

        private async Task CheckAndAlertDiscriminationAsync(ChatMessageManager messageManager, string message)
        {
            Debug.Log("Started to check hate speech for message: " + message);
            object response = await discriminationPreventor.CheckHatespeech(message);
            Debug.Log("Hate speech detectino over");
            float range = Random.Range(0f, 1f);
            if (range > 0.7)
            {
                messageManager.ActivateAlert("Some instructions for better communication through this cool message system:)");
            }
        }

        private IEnumerator AlertAfterTime(ChatMessageManager messageManager, float time)
        {
            yield return new WaitForSeconds(time);

            messageManager.ActivateAlert("Some instructions for better communication through this cool message system:)");
        }
    }


}

