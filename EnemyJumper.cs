using UnityEngine;
using System.Collections;

public class EnemyJumper : MonoBehaviour
{

    public float forceY = 300f;
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;


    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }
    // Use this for initialization
    void Start()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        
        yield return new WaitForSeconds(Random.Range(2, 4)); //controls the pause between jumps, adds random factor
        forceY = Random.Range(250, 550);         //controls the magnitude of the jump
        myRigidbody.AddForce(new Vector2(0, forceY)); //applies the jump to the enemy
        myAnimator.SetBool("attack", true);        //allows the attack animation to play
    
  
        yield return new WaitForSeconds(1.5f);     //length of time to play the attack animation
        myAnimator.SetBool("attack", false);     //back to the idle state   
        StartCoroutine(Attack());
    }

    /*
    void OnTriggerEnter2D(Collider2D target)
    {

        if (target.tag == "bullet")
        {
            Destroy(gameObject);
            Destroy(target.gameObject);
        }
    }
    */
}