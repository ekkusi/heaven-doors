using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HeavenDoors
{
    public class ChatMessageManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject alertMessage;
        [SerializeField]
        private GameObject alertImageObj;
        [SerializeField]
        private HorizontalLayoutGroup alertAndMessageGroup;
        [SerializeField]
        private Text senderText;
        [SerializeField]
        private Text messageText;

        public void InitializeMessage(string senderName, string message)
        {
            senderText.text = senderName;
            messageText.text = message;
            DeactivateAlert();
            UpdateCanvas();
        }
        private void UpdateCanvas()
        {
            Canvas.ForceUpdateCanvases();
            alertAndMessageGroup.enabled = false;
            alertAndMessageGroup.enabled = true;
        }
        public void ActivateAlert(string message)
        {
            alertMessage.SetActive(true);
            alertImageObj.SetActive(true);
            Text alertMessageText = alertMessage.transform.Find("Text").GetComponent<Text>();
            alertMessageText.text = message;
            UpdateCanvas();
        }

        public void DeactivateAlert()
        {
            alertMessage.SetActive(false);
            alertImageObj.SetActive(false);
            UpdateCanvas();
        }
    }
}

