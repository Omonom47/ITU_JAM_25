using UnityEngine;

public class Blink : MonoBehaviour
{
   
    public float max = 1f;
    public float speed = 5.0f;
    
    private SpriteRenderer _renderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;

        _renderer.color = new Color(1f, 1f, 1f, Mathf.PingPong(Time.time * speed, max));
    }
}
