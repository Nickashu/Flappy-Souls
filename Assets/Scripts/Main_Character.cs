using System.Collections;
using UnityEngine;

public class Main_Character : MonoBehaviour{

    private GameObject personagem;
    private Animator anim;
    private Rigidbody2D rig;

    public float forcaPulo = 1f;

    void Awake(){
        personagem = GetComponent<GameObject>();
        anim= GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
        if(Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space)){
            if (!anim.GetBool("isPulando")) {
                Debug.Log("Apertou!");
                StartCoroutine(pulo());
            }
        }
    }

    private IEnumerator pulo() {
        anim.SetBool("isPulando", true);
        anim.SetBool("isCaindo", false);
        rig.AddForce(transform.up * forcaPulo, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1);
        anim.SetBool("isPulando", false);
        anim.SetBool("isCaindo", true);
    }
}
