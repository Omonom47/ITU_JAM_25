using System;
using TMPro;
using UnityEngine;

public class UnitsToCome : MonoBehaviour
{
    [SerializeField] private GameObject _ui;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private IntVariable _countQueuedUp;

    private void Update()
    {
        if (_countQueuedUp.Value == 0)
        {
            _ui.SetActive(false);
        }
        else
        {
            _ui.SetActive(true);
            _text.text = $"x{_countQueuedUp.Value}";
        }
    }
}
