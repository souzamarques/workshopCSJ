using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Attributes")]
    public float health;
    public float velocidade;
    public float jumpForce;
    public float atkRadius;
    public float recoveryTime;

    bool isJumping;
    bool isAttacking;
    bool isDead;

    float recoveryCount;

    [Header("Components")]
    public Rigidbody2D rig;
    public Animator anim;
    public Transform firePoint;
    public LayerMask enemyLayer;
    public Image healthBar;
    public GameController gc;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip sfx;

    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        if(isDead == false){
            Jump();
            OnAttack();
        }
    }

    void Jump(){

        // condição irá checar se a tecla "espaço" está sendo pressionada
        if(Input.GetButtonDown("Jump")){
            if(isJumping == false){
                anim.SetInteger("transition", 2);
                rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isJumping = true;
            }
        }

    }

    void OnAttack(){
        if(Input.GetButtonDown("Fire1")){
            isAttacking = true;
            anim.SetInteger("transition", 3);
            audioSource.PlayOneShot(sfx);
            
            Collider2D hit = Physics2D.OverlapCircle(firePoint.position, atkRadius, enemyLayer);

            if(hit != null){
                hit.GetComponent<FlightEnemy>().OnHit();
            }

            StartCoroutine(OnAttacking());
        }
    }

    IEnumerator OnAttacking(){
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected(){
        Gizmos.DrawWireSphere(firePoint.position, atkRadius);
    }

    public void OnHit(float damage){
        recoveryCount += Time.deltaTime;

        if(recoveryCount >= recoveryTime && isDead == false){
            anim.SetTrigger("hit");
            health -= damage;

            healthBar.fillAmount = health / 100;

            GameOver();

            recoveryCount = 0f;
        }
    }

    void GameOver(){
        if(health <= 0){
            anim.SetTrigger("die");
            isDead = true;
            gc.ShowGameOver();
        }
    }

    // é chamado pela física do jogo ("FixedUpdate" método padrão da Unity como "Update" e "Start")
    void FixedUpdate(){
        if(isDead == false){
            OnMove();
        }
    }

    void OnMove(){
        float direcao = Input.GetAxis("Horizontal"); // armazena direcao horizontal
        
        rig.velocity = new Vector2(direcao * velocidade, rig.velocity.y); // move o Player na direcao do Input

        if(direcao > 0 && isJumping == false && isAttacking == false){
            transform.eulerAngles = new Vector2(0, 0); // Rotacionar para a direita (passa o valor "0, 0" para o rotation do player)
            anim.SetInteger("transition", 1);
        }

        if(direcao < 0 && isJumping == false && isAttacking == false){
            transform.eulerAngles = new Vector2(0, 180); // Rotacionar para a esquerda (Lembrar Plano Cartesiano e passa o valor "0, 180" para o rotation do player)
            anim.SetInteger("transition", 1);
        }

        if(direcao == 0 && isJumping == false && isAttacking == false){
            anim.SetInteger("transition", 0);
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        // se a condição for atendida, o Player estará tocando o chão (layer "Ground")
        if(collision.gameObject.layer == 8){
            isJumping = false;
        }
    }
}