using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HeavenDoors
{
    public class TagManager : MonoBehaviour
    {
        [SerializeField]
        private Text tagText;
        [SerializeField]
        private HorizontalLayoutGroup layoutGroup;
        public void SetTagText(string tagMessage)
        {
            tagText.text = tagMessage;
            UpdateCanvas();
        }
        private void UpdateCanvas()
        {
            Canvas.ForceUpdateCanvases();
            layoutGroup.enabled = false;
            layoutGroup.enabled = true;
        }
    }
}

