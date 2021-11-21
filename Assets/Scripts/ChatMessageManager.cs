using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HeavenDoors
{
    public class ChatMessageManager : MonoBehaviour
    {
        [SerializeField]
        private VerticalLayoutGroup alertContainer;
        [SerializeField]
        private Text alertText;
        [SerializeField]
        private Image alertImage;
        [SerializeField]
        private HorizontalLayoutGroup alertTagsContainer;
        [SerializeField]
        private GameObject alertImageObj;
        [SerializeField]
        private HorizontalLayoutGroup alertAndMessageGroup;
        [SerializeField]
        private Text senderText;
        [SerializeField]
        private Text messageText;
        [SerializeField]
        private GameObject tagPrefab;
        [SerializeField]
        private Color positiveColor;
        [SerializeField]
        private Color youColor;

        public void InitializeMessage(string senderName, string message, bool isYou)
        {
            senderText.text = senderName;
            messageText.text = message;
            if (isYou)
            {
                senderText.color = youColor;
            }
            DeactivateAlert();
            ClearTags();
            UpdateCanvas();
        }
        private void UpdateCanvas()
        {
            Canvas.ForceUpdateCanvases();
            alertAndMessageGroup.enabled = false;
            alertAndMessageGroup.enabled = true;
            alertContainer.enabled = false;
            alertContainer.enabled = true;
            alertTagsContainer.enabled = false;
            alertTagsContainer.enabled = true;
        }
        public void ActivateAlert(string message, List<string> tags)
        {
            alertContainer.gameObject.SetActive(true);
            alertImageObj.SetActive(true);
            alertText.text = message;
            if (tags.Count == 0)
            {
                alertTagsContainer.gameObject.SetActive(false);
            }
            foreach (string tag in tags)
            {
                GameObject tagObj = Instantiate(tagPrefab);
                tagObj.transform.SetParent(alertTagsContainer.transform, false);
                TagManager tagManager = tagObj.GetComponent<TagManager>();
                tagManager.SetTagText(tag);
            }
            UpdateCanvas();
        }
        public void ActivatePositiveAlert(string message)
        {
            alertContainer.gameObject.SetActive(true);
            alertImageObj.SetActive(false);
            alertText.text = message;
            alertImage.color = positiveColor;
            UpdateCanvas();
        }

        public void ClearTags()
        {
            foreach (Transform tag in alertTagsContainer.transform)
            {
                GameObject.Destroy(tag.gameObject);
            }
        }

        public void DeactivateAlert()
        {
            alertContainer.gameObject.SetActive(false);
            alertImageObj.SetActive(false);
            UpdateCanvas();
        }
    }
}

