using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    BoxCollider2D _boxCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetOccupied(bool occupied)
    {
        if (_spriteRenderer)
            _spriteRenderer.enabled = occupied;
        if (_boxCollider2D)
            _boxCollider2D.enabled = !occupied;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SetOccupied(true);
            FindObjectOfType<GameManager>().HomeOccupied();
        }
    }
}
