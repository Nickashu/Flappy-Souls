using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject cam, obstaculo, moeda, objetoPersonagens, txtNumMoedasMenu;
    public Transform ground, background;
    public static float distanciaSpawn, distanciaDestroy;

    private float limiteSuperiorObstaculo = 2.08f, limiteInferiorObstaculo = -4.2f, limiteSuperiorMoeda=6, limiteInferiorMoeda=-4, tempoTransicaoTelas=0.2f, posicaoInicialCamera, intervaloReposicaoCenario=20.04f;
    private float posicaoTrocaPersonagemDireita = -6.6f, posicaoTrocaPersonagemEsquerda = 5.5f, posicaoCentral=-1.4f;
    private int cont = 0, contObstaculos=0;
    private int indexPersonagemAtivo = 0, indexNovoPersonagem = 0;
    private bool isTrocandoPersonagensDireita = false, isTrocandoPersonagensEsquerda = false;

    private void Start() {
        Transform[] filhos = objetoPersonagens.GetComponentsInChildren<Transform>(true);
        if (SceneManager.GetActiveScene().name.Contains("main")) {       /*Se estiver na cena do jogo*/
            posicaoInicialCamera = cam.transform.position.x;
            for(int i=0; i<filhos.Length; i++) {
                if (i == Configs.indexpersonagemSelecionado) {
                    filhos[i].gameObject.SetActive(true);
                    Cam.player = filhos[i].gameObject.transform;
                    break;
                }
            }
            txtNumMoedasMenu.GetComponent<TextMeshProUGUI>().text = Configs.numMoedas.ToString();     /*Mostrando o número de moedas totais*/
        }
        if (SceneManager.GetActiveScene().name.Contains("characters")) {
            for (int i = 0; i < filhos.Length; i++) {
                if (i == Configs.indexpersonagemSelecionado) {
                    filhos[i].gameObject.SetActive(true);
                    Vector3 novaPosicao = new Vector3(posicaoCentral+1.5f, filhos[i].gameObject.transform.position.y, filhos[i].gameObject.transform.position.z);
                    filhos[i].transform.position = novaPosicao;    /*Posicionando o personagem selecionado no centro da tela*/
                    break;
                }
            }
        }
        if (SceneManager.GetActiveScene().name.Contains("settings")) {
            for (int i = 0; i < filhos.Length; i++) {
                if (i == Configs.indexpersonagemSelecionado) {
                    filhos[i].gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    void Update() {
        if (SceneManager.GetActiveScene().name.Contains("main")) {    /*Se estiver na cena do jogo*/
            distanciaDestroy = cam.GetComponent<Transform>().position.x - 17;
            distanciaSpawn = cam.GetComponent<Transform>().position.x + 13;
            if (!Main_Character.morreu && Main_Character.comecouJogo) {
                if (cont <= 2)
                    cont++;
                StartCoroutine(spawnarObstaculo());
            }

            if (cam.transform.position.x - intervaloReposicaoCenario >= posicaoInicialCamera) {    /*Aqui eu verifico o momento em que a câmera atinge um ponto limite para reposicionar o cenário*/
                /*Movendo o fundo e o chão*/
                Vector3 novaPosicao = new Vector3(ground.transform.position.x + intervaloReposicaoCenario, ground.transform.position.y, ground.transform.position.z);
                ground.transform.position = novaPosicao;
                background.transform.position = novaPosicao;
                posicaoInicialCamera = cam.transform.position.x;
            }
        }
        else if (SceneManager.GetActiveScene().name.Contains("characters")) {    /*Se estiver na cena de troca de personagem*/
            if(isTrocandoPersonagensDireita || isTrocandoPersonagensEsquerda) {
                Transform[] filhos = objetoPersonagens.GetComponentsInChildren<Transform>(true);
                if (isTrocandoPersonagensDireita) {
                    filhos[indexPersonagemAtivo].gameObject.transform.Translate(Vector3.right * 3 * Time.deltaTime);
                    filhos[indexNovoPersonagem].gameObject.transform.Translate(Vector3.right * 3 * Time.deltaTime);

                    if (filhos[indexNovoPersonagem].gameObject.transform.position.x >= posicaoCentral + 1.3f) {    /*Quando o novo personagem chegar no centro da tela*/
                        filhos[indexNovoPersonagem].gameObject.GetComponent<Animator>().SetBool("isCorrendo", false);
                        filhos[indexPersonagemAtivo].gameObject.SetActive(false);
                        isTrocandoPersonagensEsquerda = false;
                        isTrocandoPersonagensDireita = false;
                        Configs.indexpersonagemSelecionado = indexNovoPersonagem;     /*Definindo o novo personagem no arquivo de configurações globais*/
                    }
                }
                else if (isTrocandoPersonagensEsquerda) {
                    filhos[indexPersonagemAtivo].gameObject.transform.Translate(Vector3.left * 3 * Time.deltaTime);
                    filhos[indexNovoPersonagem].gameObject.transform.Translate(Vector3.left * 3 * Time.deltaTime);

                    if (filhos[indexNovoPersonagem].gameObject.transform.position.x <= posicaoCentral + 1.3f) {    /*Quando o novo personagem chegar no centro da tela*/
                        filhos[indexNovoPersonagem].gameObject.GetComponent<Animator>().SetBool("isCorrendo", false);
                        filhos[indexPersonagemAtivo].gameObject.SetActive(false);
                        isTrocandoPersonagensEsquerda = false;
                        isTrocandoPersonagensDireita = false;
                        Configs.indexpersonagemSelecionado = indexNovoPersonagem;
                    }
                }
            }
        }
    }

    private IEnumerator spawnarObstaculo() {
        if(cont > 1)     /*Verificando se é a primeira vez que estou chamando*/
            yield return new WaitForSeconds(4);
        GameObject objetoCopia = Instantiate(obstaculo);
        Vector3 posicao = new Vector3(distanciaSpawn, Random.Range(limiteInferiorObstaculo, limiteSuperiorObstaculo), obstaculo.transform.position.z);
        objetoCopia.transform.position = posicao;
        objetoCopia.SetActive(true);
        contObstaculos++;
        if(contObstaculos == 2) {     /*A cada 3 obstáculos, será spawnada uma moeda*/
            GameObject moedaCopia = Instantiate(moeda);
            Vector3 posicaoMoeda = new Vector3(objetoCopia.transform.position.x + 10, Random.Range(limiteInferiorMoeda, limiteSuperiorMoeda), moeda.transform.position.z);
            moedaCopia.transform.position = posicaoMoeda;
            moedaCopia.SetActive(true);
            contObstaculos = 0;
        }
        StopAllCoroutines();    //Para impedir que sejam spawnados vários objetos
    }

    public void quitGame() {
        Application.Quit();    /*Saindo do jogo*/
    }

    public void telaConfiguracoes() {
        tocarSomBotão();
        StartCoroutine(carregarTelaConfiguracoes());
    }
    public IEnumerator carregarTelaConfiguracoes() {
        yield return new WaitForSeconds(tempoTransicaoTelas);
        Transicao_Fases.tela = 3;
        Transicao_Fases.transicao = true;
    }

    public void telaPersonagens() {
        tocarSomBotão();
        Debug.Log("apertou!");
        StartCoroutine(carregarTelaPersonagens());
    }
    public IEnumerator carregarTelaPersonagens() {
        yield return new WaitForSeconds(tempoTransicaoTelas);
        Transicao_Fases.tela = 2;
        Transicao_Fases.transicao = true;
    }

    public void telaMenu() {
        if (!isTrocandoPersonagensDireita && !isTrocandoPersonagensEsquerda) {
            tocarSomBotão();
            StartCoroutine(carregarTelaMenu());
        }
    }
    public IEnumerator carregarTelaMenu() {
        yield return new WaitForSeconds(tempoTransicaoTelas);
        Transicao_Fases.tela = 1;
        Transicao_Fases.transicao = true;
    }

    public void trocarPersonagemDireita() {    /*Esta função será chamada na tela de troca de personagens*/
        if (!isTrocandoPersonagensDireita && !isTrocandoPersonagensEsquerda) {
            Transform[] filhos = objetoPersonagens.GetComponentsInChildren<Transform>(true);
            for (int i = 1; i < filhos.Length; i++) {
                if (filhos[i].gameObject.GetComponent<SpriteRenderer>().flipX == true)
                    filhos[i].gameObject.GetComponent<SpriteRenderer>().flipX = false;      /*Girando o personagem se ele estiver olhando para a esquerda*/

                if (filhos[i].gameObject.activeSelf) {    /*Se o personagem estiver ativo*/
                    indexPersonagemAtivo = i;
                    if (i < filhos.Length - 1)
                        indexNovoPersonagem = i + 1;
                    else
                        indexNovoPersonagem = 1;
                }
                else {
                    Vector3 novaPosicao = new Vector3(posicaoTrocaPersonagemDireita, filhos[i].position.y, filhos[i].position.z);
                    filhos[i].gameObject.transform.position = novaPosicao;
                }
            }
            filhos[indexNovoPersonagem].gameObject.SetActive(true);
            filhos[indexPersonagemAtivo].gameObject.GetComponent<Animator>().SetBool("isCorrendo", true);
            filhos[indexNovoPersonagem].gameObject.GetComponent<Animator>().SetBool("isCorrendo", true);
            isTrocandoPersonagensDireita = true;
        }
    }
    public void trocarPersonagemEsquerda() {    /*Esta função será chamada na tela de troca de personagens*/
        if (!isTrocandoPersonagensDireita && !isTrocandoPersonagensEsquerda) {
            Transform[] filhos = objetoPersonagens.GetComponentsInChildren<Transform>(true);
            for (int i = filhos.Length - 1; i >= 1; i--) {
                if (filhos[i].gameObject.GetComponent<SpriteRenderer>().flipX == false)
                    filhos[i].gameObject.GetComponent<SpriteRenderer>().flipX = true;      /*Girando o personagem se ele estiver olhando para a direita*/

                if (filhos[i].gameObject.activeSelf) {    /*Se o personagem estiver ativo*/
                    indexPersonagemAtivo = i;
                    if (i > 1)
                        indexNovoPersonagem = i - 1;
                    else
                        indexNovoPersonagem = filhos.Length - 1;
                }
                else {
                    Vector3 novaPosicao = new Vector3(posicaoTrocaPersonagemEsquerda, filhos[i].position.y, filhos[i].position.z);
                    filhos[i].gameObject.transform.position = novaPosicao;
                }
            }
            filhos[indexNovoPersonagem].gameObject.SetActive(true);
            filhos[indexPersonagemAtivo].gameObject.GetComponent<Animator>().SetBool("isCorrendo", true);
            filhos[indexNovoPersonagem].gameObject.GetComponent<Animator>().SetBool("isCorrendo", true);
            isTrocandoPersonagensEsquerda = true;
        }
    }

    private void tocarSomBotão() {
        /*Aqui será o som do botão*/
        Debug.Log("som");
    }
}
