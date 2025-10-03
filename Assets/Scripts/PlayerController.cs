using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using Unity.VisualScripting;
using JetBrains.Annotations;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D player;
    SpriteRenderer mysprite;
    Animator anim;
    public Menu menu;
    
    [SerializeField] float speed;
    [SerializeField] float jump;
    [SerializeField] int score;
    [SerializeField] Transform holder;
    [SerializeField] Transform collectedSound;
    [SerializeField] Transform jumpSound;
    [SerializeField] Transform enemySound;

    bool crouch = false;
    int maxHealth = 5;
    int playerHealth;
    public bool isjump = false;
    TextMeshProUGUI healthText;
    TextMeshProUGUI scoreText;

    float min = -11.84f;
    Vector2 tm;

    void Start()
    {
        player = gameObject.GetComponent<Rigidbody2D>();
        mysprite = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        playerHealth = maxHealth;
        score = 0;
        healthText = holder.Find("TxtHealth").GetComponent<TextMeshProUGUI>();
        scoreText = holder.Find("TxtScore").GetComponent<TextMeshProUGUI>();
        healthText.text = playerHealth + "/" + maxHealth;
        scoreText.text = "Score : " + score;

        //menu = gameObject.GetComponent<Menu>();

        
        

    }

    void Update()
    {
        /*Vector3 pos = transform.position;
        float n = Input.GetAxisRaw("Horizontal");

        if(n > 0)
        {
            player.velocity = new Vector2(speed, player.velocity.y);
            
        }

        else if(n < 0)
        {
            player.velocity = new Vector2(-speed, player.velocity.y);
        }

        else player.velocity = new Vector2(0, player.velocity.y);

       
        Zone limit

        if (pos.x < min ) pos.x = min;
        if (pos.x > max ) pos.x = max;
        transform.position = pos;*/

        /*if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(speed, 0));
            mysprite.flipX = false;
        }

        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-speed,0));
            mysprite.flipX = true;
        }
        */

        float horizontalmove = Input.GetAxisRaw("Horizontal");


        //Flip
        if (horizontalmove > 0) mysprite.flipX = false;
        else if (horizontalmove < 0) mysprite.flipX = true;


        //Jump
        if ( (Input.GetButtonDown("Jump") || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))  &&  isjump == false )
        {
            player.velocity = new Vector2(player.velocity.x, jump);
            isjump = true;
            anim.SetBool("Isjump", true);
            JumpSound(gameObject.transform.position);

        }
        if (Mathf.Abs(player.velocity.y) < 0.01f)
        {
            isjump = false;
            anim.SetBool("Isjump", false);
        }


        //Crouch
        anim.SetBool("Iscrouch", crouch);
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouch = true;
           
            speed = 3;
        }
            
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            crouch = false;

            speed = 8;
        }


        //Animation change
        anim.SetFloat("Speed",Mathf.Abs(player.velocity.x));


        //Menu
        if (Input.GetKeyDown(KeyCode.Escape))
                menu.Pause();
                

    }
    


    void FixedUpdate() //Player Control 
    {
        float horizontalmove = Input.GetAxisRaw("Horizontal");

        tm = transform.position;

        //Player Control
        player.velocity = new Vector2(speed * horizontalmove, player.velocity.y);
        if (tm.x <= min)
            tm.x = min;
        transform.position = tm;

    }


    void SoundOn(Vector3 itempos)
    {
        Transform obj = Instantiate(collectedSound, itempos, new Quaternion());
        obj.gameObject.SetActive(true);
        Destroy(obj.gameObject, obj.GetComponent<AudioSource>().clip.length);
    }

    void JumpSound(Vector3 jumping)
    {
        Transform jp = Instantiate(jumpSound, jumping, new Quaternion());
        jp.gameObject.SetActive(true);
        Destroy(jp.gameObject, jp.GetComponent<AudioSource>().clip.length);
    }

    void EnemySound(Vector3 killing)
    {
        Transform en = Instantiate(enemySound, killing, new Quaternion());
        en.gameObject.SetActive(true);
        Destroy(en.gameObject, en.GetComponent<AudioSource>().clip.length);
    }



    private void OnTriggerEnter2D(Collider2D collision)     //Collision off (Eagle)
    {
        Animator C = collision.gameObject.GetComponent<Animator>();

        Animator E = collision.gameObject.GetComponentInChildren<Animator>();           //Enemy Animator
        Eagle ES = collision.gameObject.GetComponent<Eagle>();                           //Eagle Script
        Opossum O = collision.gameObject.GetComponentInChildren<Opossum>();             //Opossum Script
        CircleCollider2D CC = collision.gameObject.GetComponent<CircleCollider2D>();    //Collider Component to use Trigger
        BoxCollider2D BC = collision.gameObject.GetComponent<BoxCollider2D>();

        //Enemy destroy, Health down
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (player.velocity.y < 0 && isjump)
            {
                CC.isTrigger = true;
                if(collision.gameObject.CompareTag("Eagle"))
                    ES.StopAllCoroutines();
                if(collision.gameObject.CompareTag("Opossum"))
                    O.speed = 0;
                E.SetBool("Kill", true);
                EnemySound(enemySound.transform.position);
                Destroy(collision.gameObject, 0.5f);
            }
        }


        //Gem Effect
        if (collision.CompareTag("Gem"))
        {
            C.SetTrigger("Collected");
            score +=100;
            //Debug.Log("Score = " + score);
            scoreText.text = "Score : " + score;
            SoundOn(collision.transform.position);
            Destroy(collision.gameObject, 0.5f);
        }


        //Cherry Effect
        else if (collision.CompareTag("Cherry"))
        {
            C.SetTrigger("Collected");
            score += 50;
            //Debug.Log("Score = " + score);
            scoreText.text = "Score : " + score;
            SoundOn(collision.transform.position);
            Destroy(collision.gameObject, 0.5f);
        }


        //Heart Effect
        else if (collision.CompareTag("Heart"))
        {
            if(playerHealth < maxHealth)
                playerHealth++;
            C.SetTrigger("Collected");
            healthText.text = playerHealth + "/" + maxHealth;
            SoundOn(collision.transform.position);
            Destroy(collision.gameObject, 0.5f);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Animator E = collision.gameObject.GetComponentInChildren<Animator>();           //Enemy Animator
        Eagle ES = collision.gameObject.GetComponent<Eagle>();                           //Eagle Script
        Opossum O = collision.gameObject.GetComponentInChildren<Opossum>();             //Opossum Script
        CircleCollider2D CC = collision.gameObject.GetComponentInChildren<CircleCollider2D>();    //Collider Component to use Trigger
        BoxCollider2D BC = collision.gameObject.GetComponent<BoxCollider2D>();


        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {

                if (playerHealth > 0)
                {
                    playerHealth--;
                    healthText.text = playerHealth + "/" + maxHealth;
                }

                //Player Hurt
                StartCoroutine(PlayerHurt());

        }
        
    }


    IEnumerator PlayerHurt()
    {
        //float timer = animation["player_hurt"].clip.length;
        //yield return new WaitForSeconds(0.5f);
        
        anim.Play("player_hurt");
        /*if (!mysprite) player.velocity = new Vector2(speed * -Input.GetAxisRaw("Horizontal"), 2f);
        if (mysprite) player.velocity = new Vector2(speed * Input.GetAxisRaw("Horizontal"), 2f);*/
        yield return new WaitForSeconds(0.5f);
        anim.Play("player_idle");
    }
}
