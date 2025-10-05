using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class IntText : MonoBehaviour
    {
        [SerializeField] private IntVariable _intVariable;
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

        private void Update()
        {
            _textMeshProUGUI.text = _intVariable.Value.ToString();
        }
    }
}