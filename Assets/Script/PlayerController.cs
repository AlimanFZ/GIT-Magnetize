using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    
    private UIControllerScript uiControl;
    private Rigidbody2D rb2D;
    public float moveSpeed = 5f;
    public float pullForce = 100f;
    public float rotateSpeed = 360f;
    private GameObject closestTower;
    private GameObject hookedTower;
    private bool isPulled = false;
    private AudioSource myAudio;
    private bool isCrashed = false;
    private Vector2 startPosition;
    private bool angular;
    public TowerControl turnTower;
    public TowerControl2 turnTower2;


    // Start is called before the first frame update
    void Start()
    {
        rb2D = this.gameObject.GetComponent<Rigidbody2D>();
        uiControl = GameObject.Find("Canvas").GetComponent<UIControllerScript>();
        myAudio = this.gameObject.GetComponent<AudioSource>();
        startPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        
        rb2D.velocity = -transform.up * moveSpeed;
        if ((turnTower.turn == true && !isPulled) || (turnTower2.turn == true && !isPulled) || (Input.GetKeyDown(KeyCode.Z) && !isPulled))
        {
            
                if (closestTower != null && hookedTower == null)
                {
                    hookedTower = closestTower;
                }
                if (hookedTower)
                {
                    angular = false;
                    float distance = Vector2.Distance(transform.position, hookedTower.transform.position);

                    Vector3 pullDirection = (hookedTower.transform.position - transform.position).normalized;

                    float newPullForce = Mathf.Clamp(pullForce / distance, 20, 50);
                    rb2D.AddForce(pullDirection * newPullForce);

                    rb2D.angularVelocity = -rotateSpeed / distance;
                    isPulled = true;
                }
            
        }

        if (turnTower.turn == false && turnTower2.turn == false && Input.GetKeyUp(KeyCode.Z))
        {
            isPulled = false;
            angular = true;
        }

        if (isCrashed)
        {
            if (!myAudio.isPlaying)
            {
                restartPosition();
            }
        }
        else
        {
            rb2D.velocity = -transform.up * moveSpeed;
            if(angular)
            {
                rb2D.angularVelocity = 0f;
            }
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Tower")
        {
            if (!isCrashed)
            {
                myAudio.Play();
                rb2D.velocity = new Vector3(0f, 0f, 0f);
                rb2D.angularVelocity = 0f;
                isCrashed = true;
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            Debug.Log("Levelclear!");
            uiControl.endGame();
        }
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tower")
        {
            closestTower = collision.gameObject;


            //Change tower color back to green as indicator
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (isPulled) return;

        if (collision.gameObject.tag == "Tower")
        {
            closestTower = null;
            

            //Change tower color back to normal
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
    public void restartPosition()
    {
        
        this.transform.position = startPosition;

        
        this.transform.rotation = Quaternion.Euler(0f, 0f, 90f);

       
        isCrashed = false;

        if (closestTower)
        {
            closestTower.GetComponent<SpriteRenderer>().color = Color.white;
            closestTower = null;
        }
        
    }
    

}
