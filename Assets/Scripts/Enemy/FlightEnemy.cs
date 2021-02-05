using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightEnemy : MonoBehaviour{
    // Start is called before the first frame update
    public float health;
    public float damage;

    public float speed;
    float initialSpeed;
    public bool isRight;
    public float stopDistance;

    public Rigidbody2D rig;
    public Animator anim;

    bool isDead;

    Transform Player;

    void Start(){
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        initialSpeed = speed;
    }

    // Update is called once per frame
    void Update(){
        float distance = Vector2.Distance(transform.position, Player.position);
        float playerPos = transform.position.x - Player.position.x;

        if(playerPos > 0){
            isRight = false;
        } else{
            isRight = true;
        }

        if(distance <= stopDistance){ // executado quando inimigo visualiza o Player
            speed = 0f;
            Player.GetComponent<Player>().OnHit(damage);
        } else{
            speed = initialSpeed;
        }
    }

    void FixedUpdate(){
        if(isDead == false){
            if(isRight){
                rig.velocity = new Vector2(speed, rig.velocity.y);
                transform.eulerAngles = new Vector2(0, 0);
            } else{
                rig.velocity = new Vector2(-speed, rig.velocity.y);
                transform.eulerAngles = new Vector2(0, 180);
            }
        } else{
            speed = 0f;
        }
    }

    public void OnHit(){ 
        health--;

        if(health <= 0){
            isDead = true;
            speed = 0f;
            anim.SetTrigger("death");
            Destroy(gameObject, 1f);
        } else{
            anim.SetTrigger("hit");
        }       
    }
}