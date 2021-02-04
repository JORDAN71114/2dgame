using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class horizontalEnemyShooter : MonoBehaviour
{

    [SerializeField]
    private GameObject bullet;
    // Use this for initialization
    void Start()
    {
        //this calls our attack function to start shooting the bullets right from the start of the game.
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(2f);
        //this attaches the bullet to the position of the shooter.
        
        Instantiate(bullet, transform.position, transform.rotation);
        //this begins the coroutine
        StartCoroutine(Attack());
    }

}
