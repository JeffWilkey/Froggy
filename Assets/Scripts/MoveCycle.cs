using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCycle : MonoBehaviour
{
    public GameObject sprite;
    public float speed = 5f;
    public Dir direction;
    public enum Dir { Left, Right };
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private float _edgeLeft = -6f;
    private float _edgeRight = 7f;
    void Start()
    {
        var lane = gameObject.GetComponentInParent<Lane>();

        _rb = gameObject.GetComponent<Rigidbody2D>();
        _spriteRenderer = sprite.GetComponent<SpriteRenderer>();

        if (lane && lane.speed != 0f)
            speed = lane.speed;
        
       
        PositionAndApplyInitialVelocity(lane);
    }
    
    void Update()
    {
        if (direction == Dir.Left)
        {
            if(transform.position.x < _edgeLeft - _spriteRenderer.size.x)
            {
                transform.position = new Vector3(_edgeRight + _spriteRenderer.size.x, transform.position.y, 0);
            }
        }
        else if (direction == Dir.Right)
        {
            
            if (transform.position.x > _edgeRight + _spriteRenderer.size.x)
            {
                transform.position = new Vector3(_edgeLeft - _spriteRenderer.size.x, _rb.position.y, 0);
            }
        }
    }

    private void FixedUpdate()
    {
        
    }

    private void PositionAndApplyInitialVelocity(Lane lane)
    {
        if (lane)
         _rb.position = new Vector3(transform.position.x, lane.gameObject.transform.position.y, 0);

        if (Dir.Left == direction)
            _rb.velocity = new Vector2(-speed, 0);
        else if (Dir.Right == direction)
            _rb.velocity = new Vector2(speed, 0);
    }
}
