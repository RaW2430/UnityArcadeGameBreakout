using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    private float minX = -7.5f;
    private float maxX = 7.5f;
    private Vector3 targetPosition;
    private bool isCollidingWithWall = false;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();    
    }

    // Update is called once per frame
    void Update()
    {
        float halfWidth = spriteRenderer.bounds.size.x / 2; 
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float clampedX = Mathf.Clamp(mousePosition.x, minX + halfWidth, maxX - halfWidth);  
        if (!isCollidingWithWall)
        {
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
        }
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("wall"))
    //    {
    //        Debug.Log("wall!");
    //        isCollidingWithWall = true;
    //    }
    //}
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("wall"))
    //    {
    //        Debug.Log("no wall!");
    //        isCollidingWithWall = false;
    //    }
    //}
}
