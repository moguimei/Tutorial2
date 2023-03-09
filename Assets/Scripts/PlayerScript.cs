using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public Text score;

    public Text winText;

    public Text lives;

    private int scoreValue = 0;

    private int livesValue = 3;
    
    private bool moved;

    private bool isGround;

    public AudioClip musicClipOne;

    public AudioClip musicClipTwo;

    public AudioSource musicSource;
    private bool gameOver;
    KeyCode quitKey = KeyCode.Escape;

    Animator anim;
    private bool isMoving;
    private bool facingRight = true;
    private bool won;
    private bool isJump;

    

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Coins: " + scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();

        musicSource.clip = musicClipOne;
        musicSource.Play();
        musicSource.loop = true;

        anim.SetInteger("State", 0);

        gameOver = false;
        moved = false;
        isGround = true;
        isJump = false;
        isMoving = false;
        
        
        
    }
    void Update()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");

        if(Input.GetKeyDown(quitKey)) {
            Application.Quit();
 
            Debug.Log("Quit Game.");
        }

        if (Input.GetKeyDown(KeyCode.W)){
            isGround = false;
            isJump = true;
            //anim.SetInteger("State", 2);
        }
        if (Input.GetKeyUp(KeyCode.W)){
            isJump = false;
            //anim.SetInteger("State", 2);
        }
        
        if (isGround == true){
            if (Input.GetKeyDown(KeyCode.A)){  
                isMoving = true;    
            }
            if (Input.GetKeyDown(KeyCode.D)){
                isMoving = true;
            }

        }
        if (Input.GetKeyUp(KeyCode.A)){
            isMoving = false;
        }
         if (Input.GetKeyUp(KeyCode.D)){
            isMoving = false;
        }

        if (isJump == true){
            anim.SetInteger("State", 2);
        }
        
        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));  
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Coins: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);

            if (gameOver == false){

                if (scoreValue == 8)
                {
                    winText.text = "You Win! Game by Allison Li";
                    gameOver = true;
                    musicSource.clip = musicClipTwo;
                    musicSource.Play();
                    musicSource.loop = true;
                }
            }
        }
        if (moved == false)
        {
            if (scoreValue == 4)
            {
                moved = true;
                livesValue = 3;
                lives.text = "Lives: " + livesValue.ToString();
                transform.position = new Vector3(55f, 2.15f, 0f);
            }
        }

        if(collision.collider.tag == "Enemy")
        {
            if (gameOver == false){
                livesValue -= 1;
                lives.text = "Lives: " + livesValue.ToString();
                Destroy(collision.collider.gameObject);

                if (livesValue == 0){
                    rd2d.constraints = RigidbodyConstraints2D.FreezeAll;
                    winText.text = "You lose! Game by Allison Li";
                    gameOver = true;
            }
            }
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (isMoving == false){
            anim.SetInteger("State", 0); 
            }
            if (isMoving == true){
            anim.SetInteger("State", 1);
            }
            isJump = false;
            if (isGround == false)
            {
                rd2d.AddForce(new Vector2(0, 6), ForceMode2D.Impulse); 
                 //the 3 in this line of code is the player's "jumpforce," and you change that number to get different jump behaviors.  You can also create a public variable for it and then edit it in the inspector.
                 isGround = true; 
            }
            
        }
    }
    void Flip()
   {
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
   }
}
