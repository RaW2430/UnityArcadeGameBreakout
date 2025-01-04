using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class BallController : MonoBehaviour
{
    //UI
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI youWinText;
    public Button restartButton;
    //score
    private Dictionary<string, int> scoreMap = new Dictionary<string, int>() {
        {"red0", 600},
        {"red1", 500},
        {"orange", 400},
        {"yellow", 200},
        {"green", 300},
        {"azure", 100}
    };

    //paddle
    public GameObject paddle;
    private float paddleWidth;

    //ball
    private Vector3 initialPosition = new Vector3(0f, -1.5f, 0f); 
    private float initialSpeed = 7f;
    //private float minSpeed = 2f;
    //private float maxSpeed = 20f;
    private Rigidbody2D rb;

    //speed
    private bool hasBrokenThroughHighest = false;
    private bool hasBrokenThroughMiddle = false;

    //audio
    [System.Serializable]
    public class HitSound
    {
        public string hitType;
        public AudioClip sound;
    }
    [Header("audio setting")]
    public HitSound[] hitSounds;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(Random.Range(-1f, 1f), -1f).normalized * initialSpeed;
        //rb.velocity = new Vector2(0f, -1f).normalized * initialSpeed;
        paddleWidth = paddle.GetComponent<SpriteRenderer>().bounds.size.x;
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(int.Parse(scoreText.text) == 45000)
        {
            youWinText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            rb.velocity = new Vector2(0f, 0f);
            paddle.GetComponent<PaddleController>().enabled = false;  
            //Time.timeScale = 0;
            //Debug.Log("You Win!");
        }
        if (transform.position.y < -5.5f && int.Parse(hpText.text) != 0)
        {
            hpText.text = (int.Parse(hpText.text) - 1).ToString();
            if (int.Parse(hpText.text) == 0)
            {
                gameOverText.gameObject.SetActive(true);
                restartButton.gameObject.SetActive(true);
                rb.velocity = new Vector2(0f, 0f);
                paddle.GetComponent<PaddleController>().enabled = false;
                // Debug.Log("Game Over!");
            }
            else if(int.Parse(hpText.text) > 0) 
            {
                Vector3 newSacle = paddle.transform.localScale;
                newSacle.x = paddleWidth;
                paddle.transform.localScale = newSacle;
                hasBrokenThroughHighest = false;
                hasBrokenThroughMiddle = false;
                transform.position = initialPosition;   
                rb.velocity = new Vector2(Random.Range(-1f, 1f), -1f).normalized * initialSpeed;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if ball hits the brick
        if (collision.gameObject.CompareTag("brick"))
        {
            //Debug.Log("brick!");
            string brickColor = collision.gameObject.GetComponent<BrickController>().brickColor;
            scoreText.text = (int.Parse(scoreText.text) + scoreMap[brickColor]).ToString("D8");
            //change the speed of the ball and the width of the paddle based on the color
            //note: change only once per life
            if(!hasBrokenThroughHighest && brickColor == "red0")
            {
                hasBrokenThroughHighest = true;
                ChangeSpeedAndWidth();
                if (!hasBrokenThroughMiddle)
                {
                    hasBrokenThroughMiddle = true;
                    ChangeSpeedAndWidth();
                }
            }
            else if(!hasBrokenThroughMiddle && brickColor == "green")
            {
                hasBrokenThroughMiddle = true;
                ChangeSpeedAndWidth();
            }

            //play audio clip
            foreach(HitSound hitSound in hitSounds)
            {
                if (hitSound.hitType == brickColor)
                {
                    PlaySound(hitSound.sound);
                }
            }

            Destroy(collision.gameObject);
        }
        //if ball hits the paddle
        if(collision.gameObject.CompareTag("paddle"))
        {
            foreach (HitSound hitSound in hitSounds)
            {
                if (hitSound.hitType == "paddle")
                {
                    PlaySound(hitSound.sound);
                }
            }

            //Handle ball's collison with the paddle
            ContactPoint2D contact = collision.GetContact(0);
            Vector2 paddleCenter = collision.collider.bounds.center;
            float collisionPoint = contact.point.x - paddleCenter.x; 
            Vector2 ballVelocity = rb.velocity;
            //Debug.Log("v:"+ ballVelocity);
            
            if (ballVelocity.y > 0) {
                //ball is moving towards the bottom-left of the screen
                if (ballVelocity.x < 0)
                {
                    if(collisionPoint > 0)
                    {
                        //rb.velocity = new Vector2(-ballVelocity.x, -ballVelocity.y).normalized * rb.velocity.magnitude;
                        //rb.velocity = -rb.velocity;
                        rb.velocity = new Vector2(-ballVelocity.x + Random.Range(-1f, 1f), ballVelocity.y);
                    }
                }
                //ball is moving towards the bottom-right of the screen
                else if (ballVelocity.x > 0)
                {
                    if (collisionPoint < 0)
                    {
                        //rb.velocity = new Vector2(-ballVelocity.x, -ballVelocity.y).normalized * rb.velocity.magnitude;
                        //rb.velocity = -rb.velocity;
                        rb.velocity = new Vector2(-ballVelocity.x + Random.Range(-1f, 1f), ballVelocity.y);
                    }
                }   
            }

            //if ((Mathf.Abs(ballVelocity.x) / (paddleWidth / 2)) > 0.4 )
            //{
            //    rb.velocity = new Vector2(ballVelocity.x  * 1.5f, ballVelocity.y).normalized * rb.velocity.magnitude;
            //}

            //prevent vertical bounce
            if (Mathf.Abs(ballVelocity.x) < 1f)
            {
                rb.velocity = new Vector2((ballVelocity.x + 0.1f) * 3, ballVelocity.y);
            }
        }
        //if ball hits the wall
        if (collision.gameObject.CompareTag("wall"))
        {
            foreach (HitSound hitSound in hitSounds)
            {
                if (hitSound.hitType == "wall")
                {
                    PlaySound(hitSound.sound);
                }
            }
        }
    }
    private void ChangeSpeedAndWidth()
    {
        rb.velocity *= -1.2f;
        Vector3 newSacle = paddle.transform.localScale;
        newSacle.x = newSacle.x / 2;
        paddle.transform.localScale = newSacle;
    }
    private void PlaySound(AudioClip clip)
    {
        if(clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("AudioClip is missing!");
        }
    }
}
