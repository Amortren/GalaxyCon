using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Ship : MonoBehaviour
{
    
    [SerializeField] 
    private Rigidbody2D rb;
    [SerializeField] 
    private GameObject sprite;
    private Vector2 movePosition;
    private GameObject aim;
    public enum Status
    {
        Player,
        Enemy
    }
    public Status shipStatus;

    public void SetShip(GameObject planetForAttack, Planet startPlanet)
    {
        if (startPlanet.planetTeam == Planet.Team.Player)
        {
            shipStatus = Status.Player;
        }
        else
        {
            shipStatus = Status.Enemy;
        }

        aim = planetForAttack;
        switch (shipStatus)
        {
            case Status.Player:
                sprite.GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            case Status.Enemy:
                sprite.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            
        }
    }

    private void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        
        movePosition = GameObject.FindObjectOfType<Controller>().GetComponent<Controller>().GetLastAttackPosition();

        Move();
    }
    void FixedUpdate()
    {
        Move();
    }
    void Move()
    {

        Vector2 vector = new Vector2(aim.transform.position.x - transform.position.x, aim.transform.position.y - transform.position.y);
        rb.AddForce(vector);


    }
  

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Planet>() && collision.gameObject == aim)
        {
            if (collision.gameObject.GetComponent<Planet>().planetTeam.ToString() != shipStatus.ToString())
            {
                collision.gameObject.GetComponent<Planet>().TakeDamage(this.shipStatus);
            }
            else
            {
                collision.gameObject.GetComponent<Planet>().TakeComrads();
            }
                

            Destroy(this.gameObject);
        }
    }

}
