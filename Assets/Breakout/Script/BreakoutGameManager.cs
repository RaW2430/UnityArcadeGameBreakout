using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakoutGameManager : MonoBehaviour
{
    public GameObject brick;
    //public GameObject Bricks;
    public GameObject paddle;
    public GameObject ball;
    private Vector3 leftUpPos = new Vector3(-7.1f, 1f, 0f);
    private int rows = 6;
    private int columns = 15;
    private List<Color> brickColors =  new List<Color> { 
        Color.red,
        new Color(1f, 0.33f, 0f),
        new Color(1f, 0.67f, 0f),
        new Color(1f, 1f, 0f),
        Color.green,
        new Color(0f, 1f, 1f),
    };
    // Start is called before the first frame update
    void Start()
    {
        //used to initiate the bricks
        //SpriteRenderer spriteRenderer = brick.GetComponent<SpriteRenderer>();
        //float brickWidth = spriteRenderer.bounds.size.x + 0f;
        //float brickHeight = spriteRenderer.bounds.size.y + 0f;
        //for(int i=0; i<rows; i++)
        //{
        //    for(int j=0; j<columns; j++)
        //    {
        //        Vector3 position = new Vector3(leftUpPos.x + j * brickWidth, 
        //            leftUpPos.y - i * brickHeight, 0);
        //        Quaternion rotation = Quaternion.identity;
        //        spriteRenderer.color = brickColors[i];
        //        GameObject newObject = Instantiate(brick, position, rotation);
        //    }
        //}   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
