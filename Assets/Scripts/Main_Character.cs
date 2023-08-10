using System.Collections;
using TMPro;
using UnityEngine;

public class Main_Character : MonoBehaviour{

    private Animator anim;
    private Rigidbody2D rig;
    private int contToques = 0, score=0, moedas=0, pontosMoeda=1, highScore=0;
    private bool ultrapassouRecorde = false;

    public GameObject canvas, objetoBotoes, telaMorte, menuMorte, txtScore;
    public float forcaPulo, gravidade, velocidade;
    public static bool morreu = false, comecouJogo=false;

    public AudioController instanciaAudioController;
    public AudioSource somMorte, somImpulso, somMoeda, somPonto;

    void Awake(){
        instanciaAudioController = AudioController.instancia;

        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        txtScore.gameObject.SetActive(false);    /*Escondendo o score*/
        morreu = false;
        comecouJogo = false;

        /*Definindo as configurações de acordo com a dificuldade*/
        if (Configs.dificuldade == 0) {
            pontosMoeda = 1;
            highScore = Configs.highScoreFacil;   /*Pegando o high score do modo fácil*/
        }
        else if (Configs.dificuldade == 1) {
            pontosMoeda = 3;
            highScore = Configs.highScorePadrao;   /*Pegando o high score do modo padrão*/
        }
        else {
            pontosMoeda = 10;
            highScore = Configs.highScoreHardcore;   /*Pegando o high score do modo hardcore*/
        }
        gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+1";
    }

    void Update(){
        if (!morreu) {
            if (!comecouJogo) {
                if (Input.GetKeyDown(KeyCode.Alpha9))
                    Configs.ResetData();   /*Apagando todos os dados salvos*/
                if (Input.GetKeyDown(KeyCode.Alpha7))
                    GameController.tocarSom(somImpulso);
            }

            if(Input.GetKeyDown(KeyCode.Space)) {     /*Primeiro pulo com espaço*/
                if (contToques == 0) {    /*No começo do jogo (primeira vez que a pessoa der o input)*/
                    GameController.tocarSom(somImpulso);
                    instanciaAudioController.pararMusica(AudioController.INDEX_MUSICA_AMBIENTE);
                    instanciaAudioController.tocarMusica(AudioController.INDEX_MUSICA_JOGO);
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
                        GameController.tocarSom(somImpulso);
                        instanciaAudioController.pararMusica(AudioController.INDEX_MUSICA_AMBIENTE);
                        instanciaAudioController.tocarMusica(AudioController.INDEX_MUSICA_JOGO);
                        canvas.SetActive(false);
                        comecouJogo = true;
                        contToques++;
                    }
                }
            }

            if (contToques > 0) {    /*Depois do pulo inicial*/
                txtScore.SetActive(true);
                txtScore.gameObject.GetComponent<TextMeshProUGUI>().text = score.ToString();   /*Atualizando o score*/
                if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0)) {
                    anim.SetBool("isPulando", true);
                    anim.SetBool("isCaindo", false);
                    rig.gravityScale = 0;
                    rig.AddForce(new Vector2(0, forcaPulo), ForceMode2D.Impulse);    /*Adicionando fora ao rigidbody para fazer o personagem pular*/
                }
                if ((Input.GetKeyUp(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Space)) || (Input.GetKeyUp(KeyCode.Space) && !Input.GetKey(KeyCode.Mouse0))) {
                    anim.SetBool("isPulando", false);
                    anim.SetBool("isCaindo", true);
                    rig.gravityScale = gravidade;
                }

                transform.Translate(Vector3.right * velocidade * Time.deltaTime);   /*Para mover o personagem para a direita*/
            }
        }
    }

    private void OnTriggerExit2D(Collider2D colisor) {
        if(colisor.tag == "obstaculo" && !morreu) {     /*Verificando se o personagem conseguiu passar por um obstáculo*/
            score++;
            moedas++;
            GameController.tocarSom(somPonto);
            gameObject.transform.GetChild(0).gameObject.SetActive(true);     /*Ativando o canvas do personagem e fazendo aparecer o indicador de pontuação*/
            StartCoroutine(desativarCanvasPersonagem());
        }
    }

    private IEnumerator desativarCanvasPersonagem() {
        yield return new WaitForSeconds(1);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);     /*Desativando o canvas do personagem depois de 1 segundo*/
    }

    private void OnTriggerEnter2D(Collider2D colisor) {
        if (colisor.tag == "moeda") {      /*Verificando se o jogador pegou uma moeda extra*/
            moedas += pontosMoeda;
            GameController.tocarSom(somMoeda);
            Destroy(colisor.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D colisor) {
        if((colisor.gameObject.tag == "cano" || colisor.gameObject.tag == "ground") && !morreu) {     /*Verificando a morte do personagem*/
            if(contToques > 0) {
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 5;
                GameController.tocarSom(somMorte);
                instanciaAudioController.pararMusica(AudioController.INDEX_MUSICA_JOGO);

                if (score > highScore) {
                    ultrapassouRecorde = true;
                    if (Configs.dificuldade == 0)
                        Configs.highScoreFacil = score;
                    else if (Configs.dificuldade == 1)
                        Configs.highScorePadrao = score;
                    else
                        Configs.highScoreHardcore = score;
                }
                Configs.numMoedas += moedas;

                StartCoroutine(aparecerMenuMorte());
                anim.SetBool("morreu", true);
                telaMorte.SetActive(true);
                comecouJogo = false;
                morreu = true;

                Configs.SaveData();
            }
        }
    }

    private IEnumerator aparecerMenuMorte() {
        Transform[] filhosMenuMorte = menuMorte.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < filhosMenuMorte.Length; i++) {
            if (filhosMenuMorte[i].gameObject.tag == "score")
                filhosMenuMorte[i].gameObject.GetComponent<TextMeshProUGUI>().text = score.ToString();

            if (filhosMenuMorte[i].gameObject.tag == "numMoedas")
                filhosMenuMorte[i].gameObject.GetComponent<TextMeshProUGUI>().text = "+" + moedas.ToString();

            if (filhosMenuMorte[i].gameObject.tag == "txtNovoRecorde") {
                if (ultrapassouRecorde)
                    filhosMenuMorte[i].gameObject.SetActive(true);
            }

            if (filhosMenuMorte[i].gameObject.tag == "highScore") {
                if (ultrapassouRecorde)
                    filhosMenuMorte[i].gameObject.GetComponent<TextMeshProUGUI>().text = " " + score.ToString() + "(Modo " + Configs.dificuldades[Configs.dificuldade] + ")";
                else
                    filhosMenuMorte[i].gameObject.GetComponent<TextMeshProUGUI>().text = " " + highScore + "(Modo " + Configs.dificuldades[Configs.dificuldade] + ")";
            }
        }
        yield return new WaitForSeconds(1.5f);
        menuMorte.SetActive(true);
    }
}
