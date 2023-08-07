using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject cam, objObstaculos, moeda, objetoPersonagens, txtNumMoedasMenu, txtNomePersonagem;
    private GameObject obstaculo;
    public TMP_Dropdown dropDownDificuldade;
    public Transform ground, background;
    public static float distanciaSpawn, distanciaDestroy;

    private float limiteSuperiorObstaculo = 2.08f, limiteInferiorObstaculo = -4.2f, limiteSuperiorMoeda=6.2f, limiteInferiorMoeda=-4.2f, tempoTransicaoTelas=0.2f, posicaoInicialCamera, intervaloReposicaoCenario=20.04f, tempoSpawnObstaculos=4;
    private float posicaoTrocaPersonagemDireita = -6.6f, posicaoTrocaPersonagemEsquerda = 5.5f, posicaoCentral=-1.4f;
    private int cont = 0, contObstaculos=0;
    private int indexPersonagemAtivo = 0, indexNovoPersonagem = 0, numPersonagens=0;
    private bool isTrocandoPersonagensDireita = false, isTrocandoPersonagensEsquerda = false, isComprandoPersonagem = false;

    private void Start() {
        numPersonagens = objetoPersonagens.transform.childCount;

        if (SceneManager.GetActiveScene().name.Contains("main")) {       /*Se estiver na cena do jogo*/
            posicaoInicialCamera = cam.transform.position.x;
            for(int i = 0; i< numPersonagens; i++) {
                if (i == Configs.indexPersonagemSelecionado) {
                    objetoPersonagens.transform.GetChild(i).gameObject.SetActive(true);
                    Cam.player = objetoPersonagens.transform.GetChild(i).gameObject.transform;
                    break;
                }
            }
            /*Definindo as configurações de acordo com a dificuldade*/
            if (Configs.dificuldade == 0) {
                limiteInferiorObstaculo = -4;
                limiteSuperiorObstaculo = 1.7f;
                obstaculo = objObstaculos.transform.GetChild(0).gameObject;
            }
            else if (Configs.dificuldade == 1) {
                limiteInferiorObstaculo = -4.74f;
                limiteSuperiorObstaculo = 1.6f;
                obstaculo = objObstaculos.transform.GetChild(1).gameObject;
            }
            else {
                limiteInferiorObstaculo = -5.6f;
                limiteSuperiorObstaculo = 1.7f;
                obstaculo = objObstaculos.transform.GetChild(2).gameObject;
            }

            atualizarNumMoedas();
        }
        if (SceneManager.GetActiveScene().name.Contains("characters")) {
            for (int i = 0; i < numPersonagens; i++) {
                if (i == Configs.indexPersonagemSelecionado) {
                    objetoPersonagens.transform.GetChild(i).gameObject.SetActive(true);
                    Vector3 novaPosicao = new Vector3(posicaoCentral+1.3f, objetoPersonagens.transform.GetChild(i).gameObject.transform.position.y, objetoPersonagens.transform.GetChild(i).gameObject.transform.position.z);
                    objetoPersonagens.transform.GetChild(i).transform.position = novaPosicao;    /*Posicionando o personagem selecionado no centro da tela*/
                    atualizarNomePersonagem(i);
                }

                if (Configs.isCompradoPersonagens[Configs.personagens[i]] == false) {     /*Verificando se o personagem não foi comprado*/
                    objetoPersonagens.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                    objetoPersonagens.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Configs.precosPersonagens[Configs.personagens[i]].ToString();      /*Recuperando o preço do personagem*/
                }
                else {     /*Se o personagem foi comprado, vou remover o preço dele*/
                    objetoPersonagens.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
            }

            atualizarNumMoedas();
        }
        if (SceneManager.GetActiveScene().name.Contains("settings")) {
            dropDownDificuldade.SetValueWithoutNotify(Configs.dificuldade);

            for (int i = 0; i < numPersonagens; i++) {
                if (i == Configs.indexPersonagemSelecionado) {
                    objetoPersonagens.transform.GetChild(i).gameObject.SetActive(true);
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
                atualizarNomePersonagem(-1);   /*Apagando o nome do personagem*/
                if (isTrocandoPersonagensDireita) {
                    objetoPersonagens.transform.GetChild(indexPersonagemAtivo).gameObject.transform.Translate(Vector3.right * 3 * Time.deltaTime);
                    objetoPersonagens.transform.GetChild(indexNovoPersonagem).gameObject.transform.Translate(Vector3.right * 3 * Time.deltaTime);

                    if (objetoPersonagens.transform.GetChild(indexNovoPersonagem).gameObject.transform.position.x >= posicaoCentral + 1.3f) {    /*Quando o novo personagem chegar no centro da tela*/
                        objetoPersonagens.transform.GetChild(indexNovoPersonagem).gameObject.GetComponent<Animator>().SetBool("isCorrendo", false);
                        objetoPersonagens.transform.GetChild(indexPersonagemAtivo).gameObject.SetActive(false);
                        isTrocandoPersonagensEsquerda = false;
                        isTrocandoPersonagensDireita = false;
                        atualizarNomePersonagem(indexNovoPersonagem);   /*Atualizando o nome do personagem*/
                        if (Configs.isCompradoPersonagens[Configs.personagens[indexNovoPersonagem]] == true) {     /*Verificando se o personagem foi comprado*/
                            Configs.indexPersonagemSelecionado = indexNovoPersonagem;     /*Definindo o novo personagem no arquivo de configurações globais*/
                        }
                    }
                }
                else if (isTrocandoPersonagensEsquerda) {
                    objetoPersonagens.transform.GetChild(indexPersonagemAtivo).gameObject.transform.Translate(Vector3.left * 3 * Time.deltaTime);
                    objetoPersonagens.transform.GetChild(indexNovoPersonagem).gameObject.transform.Translate(Vector3.left * 3 * Time.deltaTime);

                    if (objetoPersonagens.transform.GetChild(indexNovoPersonagem).gameObject.transform.position.x <= posicaoCentral + 1.3f) {    /*Quando o novo personagem chegar no centro da tela*/
                        objetoPersonagens.transform.GetChild(indexNovoPersonagem).gameObject.GetComponent<Animator>().SetBool("isCorrendo", false);
                        objetoPersonagens.transform.GetChild(indexPersonagemAtivo).gameObject.SetActive(false);
                        isTrocandoPersonagensEsquerda = false;
                        isTrocandoPersonagensDireita = false;
                        atualizarNomePersonagem(indexNovoPersonagem);   /*Atualizando o nome do personagem*/
                        if (Configs.isCompradoPersonagens[Configs.personagens[indexNovoPersonagem]] == true) {     /*Verificando se o personagem foi comprado*/
                            Configs.indexPersonagemSelecionado = indexNovoPersonagem;     /*Definindo o novo personagem no arquivo de configurações globais*/
                        }
                    }
                }
            }
        }
    }

    private IEnumerator spawnarObstaculo() {
        if(cont > 1)     /*Verificando se é a primeira vez que estou chamando*/
            yield return new WaitForSeconds(tempoSpawnObstaculos);
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


    /*Funções de botões*/
    public void trocarPersonagemDireita() {    /*Esta função será chamada na tela de troca de personagens*/
        if (!isTrocandoPersonagensDireita && !isTrocandoPersonagensEsquerda) {
            for (int i = 0; i < numPersonagens; i++) {
                if (objetoPersonagens.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().flipX == true)
                    objetoPersonagens.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().flipX = false;      /*Girando o personagem se ele estiver olhando para a esquerda*/

                if (objetoPersonagens.transform.GetChild(i).gameObject.activeSelf) {    /*Se o personagem estiver ativo*/
                    indexPersonagemAtivo = i;
                    if (i < numPersonagens - 1)
                        indexNovoPersonagem = i + 1;
                    else
                        indexNovoPersonagem = 0;
                }
                else {
                    Vector3 novaPosicao = new Vector3(posicaoTrocaPersonagemDireita, objetoPersonagens.transform.GetChild(i).position.y, objetoPersonagens.transform.GetChild(i).position.z);
                    objetoPersonagens.transform.GetChild(i).gameObject.transform.position = novaPosicao;
                }
            }
            objetoPersonagens.transform.GetChild(indexNovoPersonagem).gameObject.SetActive(true);
            objetoPersonagens.transform.GetChild(indexPersonagemAtivo).gameObject.GetComponent<Animator>().SetBool("isCorrendo", true);
            objetoPersonagens.transform.GetChild(indexNovoPersonagem).gameObject.GetComponent<Animator>().SetBool("isCorrendo", true);
            isTrocandoPersonagensDireita = true;
        }
    }
    public void trocarPersonagemEsquerda() {    /*Esta função será chamada na tela de troca de personagens*/
        if (!isTrocandoPersonagensDireita && !isTrocandoPersonagensEsquerda) {
            for (int i = numPersonagens - 1; i >= 0; i--) {
                if (objetoPersonagens.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().flipX == false)
                    objetoPersonagens.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().flipX = true;      /*Girando o personagem se ele estiver olhando para a direita*/

                if (objetoPersonagens.transform.GetChild(i).gameObject.activeSelf) {    /*Se o personagem estiver ativo*/
                    indexPersonagemAtivo = i;
                    if (i > 0)
                        indexNovoPersonagem = i - 1;
                    else
                        indexNovoPersonagem = numPersonagens - 1;
                }
                else {
                    Vector3 novaPosicao = new Vector3(posicaoTrocaPersonagemEsquerda, objetoPersonagens.transform.GetChild(i).position.y, objetoPersonagens.transform.GetChild(i).position.z);
                    objetoPersonagens.transform.GetChild(i).gameObject.transform.position = novaPosicao;
                }
            }
            objetoPersonagens.transform.GetChild(indexNovoPersonagem).gameObject.SetActive(true);
            objetoPersonagens.transform.GetChild(indexPersonagemAtivo).gameObject.GetComponent<Animator>().SetBool("isCorrendo", true);
            objetoPersonagens.transform.GetChild(indexNovoPersonagem).gameObject.GetComponent<Animator>().SetBool("isCorrendo", true);
            isTrocandoPersonagensEsquerda = true;
        }
    }

    public void clicaComprar() {
        int indexPersonagemComprado=0;
        for (int i = 0; i < numPersonagens; i++) {
            if (objetoPersonagens.transform.GetChild(i).gameObject.activeSelf) {     /*Pegando o objeto do personagem que foi comprado*/
                indexPersonagemComprado = i;
                break;
            }
        }
        if(Configs.numMoedas >= Configs.precosPersonagens[Configs.personagens[indexPersonagemComprado]]) {      /*Se o dinheiro for suficiente*/
            tocarSomBotão();    /*Aqui será tocado o som do botão de compra bem sucedida*/
            objetoPersonagens.transform.GetChild(indexPersonagemComprado).transform.GetChild(0).gameObject.SetActive(false);    /*Escondendo o preço*/
            Debug.Log("Compra efetuada!");

            Configs.numMoedas -= Configs.precosPersonagens[Configs.personagens[indexPersonagemComprado]];
            atualizarNumMoedas();
            isComprandoPersonagem = true;
            StartCoroutine(comprarPersonagem(indexPersonagemComprado));
        }
        else {      /*Se o dinheiro for insuficiente*/
            Debug.Log("Dinheiro insuficiente!");
            tocarSomBotão();    /*Aqui será tocado o som do botão de compra mal sucedida*/
        }
    }

    public IEnumerator comprarPersonagem(int indexPersonagem) {
        yield return new WaitForSeconds(1);
        objetoPersonagens.transform.GetChild(indexPersonagem).GetComponent<SpriteRenderer>().color = Color.white;
        Configs.isCompradoPersonagens[Configs.personagens[indexPersonagem]] = true;
        Configs.indexPersonagemSelecionado = indexPersonagem;       /*Definindo o novo personagem no arquivo de configurações globais*/
        isComprandoPersonagem = false;
        StopAllCoroutines();
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
        if (!isTrocandoPersonagensDireita && !isTrocandoPersonagensEsquerda && !isComprandoPersonagem) {
            tocarSomBotão();
            StartCoroutine(carregarTelaMenu());
        }
    }
    public IEnumerator carregarTelaMenu() {
        yield return new WaitForSeconds(tempoTransicaoTelas);
        Transicao_Fases.tela = 1;
        Transicao_Fases.transicao = true;
    }

    public void trocarDificuldade(int valor) {
        Configs.dificuldade = valor;    /*Alterando a dificuldade do jogo*/
    }




    /*Funções auxiliares*/
    private void atualizarNumMoedas() {
        txtNumMoedasMenu.GetComponent<TextMeshProUGUI>().text = Configs.numMoedas.ToString();
    }

    private void atualizarNomePersonagem(int indexPersonagem) {
        if(indexPersonagem == -1)
            txtNomePersonagem.GetComponent<TextMeshProUGUI>().text = "";
        else
            txtNomePersonagem.GetComponent<TextMeshProUGUI>().text = Configs.nomesPersonagens[Configs.personagens[indexPersonagem]];
    }

    private void tocarSomBotão() {
        /*Aqui será o som do botão*/
        Debug.Log("som");
    }
}
