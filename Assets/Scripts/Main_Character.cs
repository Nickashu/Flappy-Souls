using System.Collections;
using UnityEngine;

public class Main_Character : MonoBehaviour{

    private GameObject personagem;
    private Animator anim;
    private Vector3 direcao;
    private Rigidbody2D rig;

    public float forcaPulo, gravidade;

    void Awake(){
        personagem = GetComponent<GameObject>();
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
        if(Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Space)){
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
    }
    /*
    private IEnumerator pulo() {
        rig.gravityScale = 0;
        rig.mass = 0.5f;
        rig.AddForce(new Vector2(0, forcaPulo), ForceMode2D.Impulse);
        anim.SetBool("isPulando", true);
        anim.SetBool("isCaindo", false);
        yield return new WaitForSeconds(0.5f);
        rig.gravityScale = 1;
        rig.mass = 10;
        anim.SetBool("isPulando", false);
        anim.SetBool("isCaindo", true);
    }
*/
}
