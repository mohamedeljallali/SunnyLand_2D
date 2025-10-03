using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour
{
    Transform player;
    SpriteRenderer enemysprite;
    Transform endpos;
    float eaglehight = 2;
    Vector3 startpos;
    [SerializeField] float speed;


    void Start()
    {
        enemysprite = GetComponentInChildren<SpriteRenderer>();
        StartCoroutine(EagleAnimation());
        startpos = transform.position;
        player = GameObject.Find("player").GetComponent<Transform>();     
                 //GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    }

    void Update()
    {
        //Enemy flip
        if (player.position.x > transform.position.x)
            enemysprite.flipX = true;
        else 
            enemysprite.flipX = false;
    }

    public IEnumerator EagleAnimation()
    {
        Vector3 endpos = new Vector3(transform.position.x, transform.position.y + eaglehight, startpos.z);

        bool isflight = true;
        float value = 0;

        while (true)
        {
            yield return null;

            if (isflight)
                transform.position = Vector3.Lerp(startpos, endpos, value);
            else 
                transform.position = Vector3.Lerp(endpos, startpos, value);

            value = value + speed * Time.deltaTime;

            if(value > 1)
            {
                value = 0;
                isflight = !isflight;
            }

        }

    }

}
