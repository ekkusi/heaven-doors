using System.Collections;
using System.Collections.Generic;
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
        [SerializeField]
        private GameObject messagePrefab;

        private void Start()
        {
            inputText = inputField.transform.Find("Text").GetComponent<Text>();
            foreach (Transform child in chatContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) && inputText.text.Length > 0)
            {
                CreateChatMessage(inputText.text);
                inputField.SetTextWithoutNotify("");
            }

        }

        private void CreateChatMessage(string message)
        {
            GameObject messageObj = Instantiate(messagePrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("Pos first: " + messageObj.transform.position);
            messageObj.transform.SetParent(chatContainer.transform);
            Text senderText = messageObj.transform.Find("SenderName").GetComponent<Text>();
            Text messageText = messageObj.transform.Find("Message").GetComponent<Text>();
            senderText.text = PLAYER_NAMES[Random.Range(0, PLAYER_NAMES.Count)] + ":";
            messageText.text = message;
            messageObj.transform.localScale = Vector3.one;
            messageObj.transform.position = Vector3.zero;
            Debug.Log("Pos end: " + messageObj.transform.position);
        }
    }
}

