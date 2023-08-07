using System.Collections;
using TMPro;
using UnityEngine;

public class Main_Character : MonoBehaviour{

    private Animator anim;
    private Rigidbody2D rig;
    private int contToques = 0, score=0, moedas=0;

    public GameObject canvas, objetoBotoes, telaMorte, menuMorte;
    public float forcaPulo, gravidade, velocidade;
    public static bool morreu = false, comecouJogo=false;

    void Awake(){
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        morreu = false;
        comecouJogo = false;
    }

    void Update(){
        if (!morreu) {
            if(Input.GetKeyDown(KeyCode.Space)) {     /*Primeiro pulo com espaço*/
                if (contToques == 0) {    /*No começo do jogo (primeira vez que a pessoa der o input)*/
                    canvas.SetActive(false);
                    comecouJogo = true;
                    contToques++;
                }
            }
            if (Input.GetKeyDown(KeyCode.Mouse0)) {     /*Primeiro pulo com o mouse*/
                if (contToques == 0) {    /*No começo do jogo (primeira vez que a pessoa der o input)*/
                    Vector3 mousePosition = Input.mousePosition;
                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                    Vector2 worldPosition2D = new Vector2(worldPosition.x, worldPosition.y);
                    if (!objetoBotoes.GetComponent<Collider2D>().OverlapPoint(worldPosition2D)) {    /*Verifica se o clique ocorreu sobre o objeto que bloqueia o clique nos botões*/
                        Debug.Log("Não clicou no objeto!");
                        canvas.SetActive(false);
                        comecouJogo = true;
                        contToques++;
                    }
                }
            }

            if (contToques > 0) {    /*Depois do pulo inicial*/
                if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0)) {
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

                transform.Translate(Vector3.right * velocidade * Time.deltaTime);   /*Para mover o personagem para a direita*/
            }
        }
    }

    private void OnTriggerExit2D(Collider2D colisor) {
        if(colisor.tag == "obstaculo") {     /*Verificando se o personagem conseguiu passar por um obstáculo*/
            Debug.Log("Passou obstaculo!");
            score++;
            moedas++;
        }
    }

    private void OnTriggerEnter2D(Collider2D colisor) {
        if (colisor.tag == "moeda") {      /*Verificando se o jogador pegou uma moeda extra*/
            Debug.Log("Pegou moeda!");
            if (Configs.dificuldade == 0)
                moedas++;
            else if (Configs.dificuldade == 1)
                moedas += 2;
            else
                moedas += 3;
            Destroy(colisor.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D colisor) {
        if(colisor.gameObject.tag == "cano" || colisor.gameObject.tag == "ground") {     /*Verificando a morte do personagem*/
            if(contToques > 0) {
                Debug.Log("Morreu!");
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 5;
                StartCoroutine(aparecerMenuMorte());
                anim.SetBool("morreu", true);
                telaMorte.SetActive(true);
                comecouJogo = false;
                morreu = true;
                Configs.numMoedas += moedas;
            }
        }
    }

    private IEnumerator aparecerMenuMorte() {
        Transform[] filhosMenuMorte = menuMorte.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < filhosMenuMorte.Length; i++) {
            if (filhosMenuMorte[i].gameObject.tag == "score") {
                filhosMenuMorte[i].gameObject.GetComponent<TextMeshProUGUI>().text = score.ToString();
            }
            if (filhosMenuMorte[i].gameObject.tag == "numMoedas") {
                filhosMenuMorte[i].gameObject.GetComponent<TextMeshProUGUI>().text = "+" + moedas.ToString();
            }
            if (filhosMenuMorte[i].gameObject.tag == "txtNovoRecorde") {
                if (score > Configs.highScore) {
                    filhosMenuMorte[i].gameObject.SetActive(true);
                    Configs.highScore = score;
                }
            }
        }
        yield return new WaitForSeconds(1.5f);
        menuMorte.SetActive(true);
    }
}
