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
        private string otherPlayerName;
        private int messageCount;
        ChatDiscriminationPreventor discriminationPreventor;

        private void Start()
        {
            chatLayoutGroup = chatContainer.GetComponent<VerticalLayoutGroup>();
            foreach (Transform child in chatContainer.transform)
            {
                Destroy(child.gameObject);
            }
            discriminationPreventor = new ChatDiscriminationPreventor();
            otherPlayerName = "DragonSlayer666";
        }

        private void Update()
        {
            if (inputField.text.Length == 0 && sendButton.interactable)
            {
                sendButton.interactable = false;
            }
            if (inputField.text.Length > 0 && !sendButton.interactable)
            {
                sendButton.interactable = true;
            }
            if (Input.GetKeyDown(KeyCode.Return) && inputField.text.Length > 0)
            {
                CreateChatMessage(inputField.text);
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
            if (inputField.text.Length > 0)
            {
                CreateChatMessage(inputField.text);
                inputField.SetTextWithoutNotify("");
            }
        }

        private void CreateChatMessage(string message)
        {
            GameObject messageObj = Instantiate(messagePrefab, Vector3.zero, Quaternion.identity);
            messageObj.transform.SetParent(chatContainer.transform, false);

            bool isYou = messageCount % 2 != 0;
            string playerName = "You";
            if (!isYou)
            {
                playerName = otherPlayerName;
            }
            ChatMessageManager messageManager = messageObj.GetComponent<ChatMessageManager>();
            messageManager.InitializeMessage(playerName, message, isYou);
            messageCount++;
            if (isYou)
            {
                StartCoroutine(CheckAndAlertDiscriminationAsync(messageManager, message).AsIEnumerator());
            }
            UpdateCanvas();
        }

        private async Task CheckAndAlertDiscriminationAsync(ChatMessageManager messageManager, string message)
        {
            DiscriminationResult result = await discriminationPreventor.CheckHatespeech(message);
            if (result.isPositive)
            {
                messageManager.ActivatePositiveAlert(result.positiveMessage);
            }
            else if (result.isToxic)
            {
                messageManager.ActivateAlert(result.adviceMessage, result.types);
            }
            UpdateCanvas();
        }

    }
}

