using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opossum : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    public float speed;
    bool isright;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Flip
        if (Mathf.Abs(rb.velocity.x) <= 0.0001f)
        {
            isright = !isright;
            sr.flipX = !sr.flipX;
        }

        if (isright)
            rb.velocity = new Vector2(Time.deltaTime * speed, rb.velocity.y);
        else
            rb.velocity = new Vector2(Time.deltaTime * -speed, rb.velocity.y);
    }

    void FixedUpdate()
    {
        //Flip
        //if (Mathf.Abs(rb.velocity.x) <= 0.0001f)
        //{
        //    isright = !isright;
        //    sr.flipX = !sr.flipX;
        //}

        //if (isright)
        //    rb.velocity = new Vector2(Time.fixedDeltaTime * speed, rb.velocity.y);
        //else
        //    rb.velocity = new Vector2(Time.fixedDeltaTime * -speed, rb.velocity.y);
    }
}
