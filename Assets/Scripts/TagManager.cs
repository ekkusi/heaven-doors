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
        public void SetTagText(string tagMessage)
        {
            tagText.text = tagMessage;
        }
    }
}

