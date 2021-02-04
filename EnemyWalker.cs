using UnityEngine;
using System.Collections;

public class EnemyWalker : MonoBehaviour
{

	//these will be used to help the enemy change direction when it encounters the edge of the ground
	private Rigidbody2D myRigidbody;
	[SerializeField]
	private Transform startPos, endPos;
	//this will test for the ground
	private bool collision;
	//these allow the enemy to walk on the ground
	public float speed = 2f;

	private bool connection;
	void Awake()
	{
		myRigidbody = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		Move();
		ChangeDirection();
	}

	//this moves the walker along the x axis at the speed variable setting
	void Move()
	{
		myRigidbody.velocity = new Vector2(transform.localScale.x, 0) * speed;
	}
	void OnCollisionEnter2D(Collision2D target)
	{
		if (target.gameObject.tag == "rat" || target.gameObject.tag == "Player") //if rat collides with a rat
        {
			connection = true;
        }
		
	}
	void ChangeDirection()

	{
		//this monitors the contact between the walker and a layer called Ground.
		collision = Physics2D.Linecast(startPos.position, endPos.position, 1 << LayerMask.NameToLayer("Ground"));

		//if there is no collision between the walker and the ground, then this changes the walker's direction
		if (!collision || connection)
		{
			Vector3 temp = transform.localScale;
			if (temp.x == 1)
			{
				temp.x = -1f;
			}
			else
			{
				temp.x = 1f;
			}
			transform.localScale = temp;
			connection = false;
		}
		
	}	
}

