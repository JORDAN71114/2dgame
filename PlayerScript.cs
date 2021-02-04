using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Diagnostics;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
	//gives my Player the ability to interact with other game elements using physics
	private Rigidbody2D myRigidbody;
	//this is set to public so I can interact with it in the inspector.  It will give the Player variable speed.
	private Animator myAnimator;
	public float movementSpeed;
	//can be set to true or false to change the Players facing direction
	private bool facingRight;
	public bool imAlive;
    public GameObject menu;
	public GameObject reset; //this is the reset button in the canvas
	[SerializeField]
	private Transform[] groundpoints; //creates something to collide with the ground
	[SerializeField]
	private float groundRadius; //creates the size of the colliders
	[SerializeField]
	private LayerMask WhatIsGround; //defines what is ground
	private bool isGrounded; //can be set to true or false based on our position
	private bool jump; //can be set to true or false when we press the space key
	[SerializeField]
	private float jumpforce; //allows us to determine the magnitude of the jump
	public GameObject nextlevel; //this is the next level button in the canvas
								 //health slider vars
	private Slider healthBar;
	public float health = 10f;
	private float healthBurn = 4f;
	// Use this for initialization
	void Start()
	{   //initial value to set the Player facing right
		facingRight = true;
		//associates the Rigidbody component of the Player with a variable we can use later on
		myRigidbody = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<Animator>();
		imAlive = true;
		reset.SetActive(false); //at the beginning of the game, the reset button is invisible
     
		nextlevel.SetActive(false); //at the beginning of the game, the nextlevel button is invisible
									//health slider variables
		healthBar = GameObject.Find("health slider").GetComponent<Slider>();
		healthBar.minValue = 0f;
		healthBar.maxValue = health;
		healthBar.value = healthBar.maxValue;

	}

	//Update is called once per frame. 
	void Update()
	{
		HandleInput();
	}
	//Fixed Update locks in speed and performance regardless of console performance and quality*/
	void FixedUpdate()
	{
		//access the keyboard controls and move left and right
		float horizontal = Input.GetAxis("Horizontal");
		//just to see what is being reported by the keyboard on the console
		//Debug.Log (horizontal);
		isGrounded = IsGrounded();
		//calling the function in the game 
		//controls Player on the x and y axis
		if (imAlive)
		{

			HandleMovement(horizontal);
			//controls player facing direction
			Flip(horizontal);
		}
		if (health <= 0)
		{
			myAnimator.SetBool("dead", true); // trigger death animation
			imAlive = false;
		}

	}

	private void HandleInput()
	{

		if (Input.GetKeyDown(KeyCode.Space))
		{
			jump = true;
			UnityEngine.Debug.Log("I'm jumping");
			myAnimator.SetBool("jumping", true);
		}

	}
	private void Flip(float horizontal)
	{
		//logical test to make sure that we are changing his facing direction
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
		{
			facingRight = !facingRight; //this sets the value of facingRight to its opposite
			Vector3 theScale = transform.localScale; //this accesses the local player scale component
			theScale.x *= -1;  //multiplying the x value of scale by -1 allows us to get either 1 or -1 as the result
			transform.localScale = theScale; //this reports the new value to the player's scale component
		}

	}
	private void HandleMovement(float horizontal)
	{
		if (isGrounded && jump)
		{
			isGrounded = false;
			jump = false;
			myRigidbody.AddForce(new Vector2(0, jumpforce));
		}
		myRigidbody.velocity = new Vector2(horizontal * movementSpeed, myRigidbody.velocity.y);
		myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
	}
	private bool IsGrounded()
	{
		if (myRigidbody.velocity.y <= 0) //if player is not moving vertically, test each of Player's groundpoints for collision
		{
			foreach (Transform point in groundpoints)
			{
				Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, WhatIsGround);
				for (int i = 0; i < colliders.Length; i++)
				{
					if (colliders[i].gameObject != gameObject)
					{
						return true;
					}
				}
			}
		}
		return false;
	}
	//this function will be called when player collides with an enemy
	public void UpdateHealth()
	{
		if (health > 1)
		{
			//if the health bar has life left..
			health -= healthBurn; //current value of health minus 2f
			healthBar.value = health;  //update the interface slider
		}
		else
		{
			imdead(); //if no life left, run this function which kills
			reset.SetActive(true);   //this makes the button visible
		}
	}


	public void imdead()
    {
		myAnimator.SetBool("dead", true);
		imAlive = false;   //kills the player
		reset.SetActive(true);   //this makes the button visible
		menu.SetActive(true);
		health = 0; //current value of health minus 2f
		healthBar.value = health;  //update the interface slider
	}
    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag == "Ground")//if i collide with the ground
        {

            myAnimator.SetBool("jumping", false); // don't do a jumping animation.

        }
        if (target.gameObject.tag == "deadly") //if he collides with an enemy that instakills
            
        {
			imdead();
		
        }
		if (target.gameObject.tag == "damage" || target.gameObject.tag == "rat") //if he collides with an enemy that doesn't kill

		{
			UpdateHealth();

		}
	}


}
