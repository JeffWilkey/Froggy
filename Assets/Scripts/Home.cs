using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    Collider2D _collider2D;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetOccupied(bool occupied)
    {
        _spriteRenderer.enabled = occupied;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SetOccupied(true);
        }
    }
}
