using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lee.Scripts
{

    public class BaseUI : MonoBehaviour
    {
        protected Dictionary<string, RectTransform> rectTransform;
        protected Dictionary<string, Button> buttons;
        protected Dictionary<string, TMP_Text> texts;
        protected virtual void Awake()
        {
            BindChildren();
        }
        
        protected virtual void OnDisable() {}

        private void BindChildren()
        {
            rectTransform = new Dictionary<string, RectTransform>();
            buttons = new Dictionary<string, Button>();
            texts = new Dictionary<string, TMP_Text>();

            RectTransform[] children = GetComponentsInChildren<RectTransform>();
            foreach (RectTransform child in children)
            {
                string key = child.gameObject.name;

                if (rectTransform.ContainsKey(key))
                    continue;

                rectTransform.Add(key, child);

                Button button = child.GetComponent<Button>();
                if (button != null)
                    buttons.Add(key, button);

                TMP_Text text = child.GetComponent<TMP_Text>();
                if (text != null)
                    texts.Add(key, text);
            }
        }
    }

}

