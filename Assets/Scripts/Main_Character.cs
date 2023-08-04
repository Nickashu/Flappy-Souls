using UnityEngine;

public class Main_Character : MonoBehaviour{

    private GameObject personagem;
    private Animator anim;
    private Rigidbody2D rig;
    private int contToques = 0;

    public GameObject canvas;
    public float forcaPulo, gravidade, velocidade;
    public static bool morreu = false, comecouJogo=false;

    void Awake(){
        personagem = GetComponent<GameObject>();
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }

    void Update(){
        if (!morreu) {
            if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Space)) {
                if (contToques == 0) {    /*Na primeira vez que a pessoa der o input*/
                    canvas.SetActive(false);
                    comecouJogo = true;
                    contToques++;
                }
                rig.gravityScale = 0;
                rig.AddForce(new Vector2(0, forcaPulo), ForceMode2D.Impulse);    /*Adicionando fora ao rigidbody para fazer o personagem pular*/
                anim.SetBool("isPulando", true);
                anim.SetBool("isCaindo", false);
            }
            if ((Input.GetKeyUp(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Space)) || (Input.GetKeyUp(KeyCode.Space) && !Input.GetKey(KeyCode.Mouse0))) {
                rig.gravityScale = gravidade;
                anim.SetBool("isPulando", false);
                anim.SetBool("isCaindo", true);
            }

            if (contToques > 0) {
                transform.Translate(Vector3.right * velocidade * Time.deltaTime);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D colisor) {
        if(colisor.tag == "obstaculo") {     /*Verificando se o personagem conseguiu passar por um obstáculo*/
            Debug.Log("Passou obstaculo!");
        }
    }

    private void OnCollisionEnter2D(Collision2D colisor) {
        if(colisor.gameObject.tag == "cano" || colisor.gameObject.tag == "ground") {     /*Verificando a morte do personagem*/
            if(contToques > 0) {
                Debug.Log("Morreu!");
                comecouJogo = false;
                morreu = true;
            }
        }
    }
}
