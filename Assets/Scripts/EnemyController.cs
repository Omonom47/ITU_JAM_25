using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject _cursorObject;

    private RectTransform _rectTransform;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rectTransform = _cursorObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        _rectTransform.position += Vector3.one;
    }
}
