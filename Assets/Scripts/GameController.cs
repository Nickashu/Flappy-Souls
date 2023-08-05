using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject cam, obstaculo;
    public Transform ground, background;
    public static float distanciaSpawn, distanciaDestroy;

    private float limiteSuperiorObstaculo = 2.08f, limiteInferiorObstaculo = -4.2f, tempoTransicaoTelas=0.3f, posicaoInicialCamera, intervaloReposicaoCenario=20.04f;
    private int cont = 0;

    private void Start() {
        if (SceneManager.GetActiveScene().name.Contains("main"))    /*Se estiver na cena do jogo*/
            posicaoInicialCamera = cam.transform.position.x;
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
    }

    private IEnumerator spawnarObstaculo() {
        if(cont > 1)     /*Verificando se é a primeira vez que estou chamando*/
            yield return new WaitForSeconds(4);
        GameObject objetoCopia = obstaculo;
        Vector3 posicao = new Vector3(distanciaSpawn, Random.Range(limiteInferiorObstaculo, limiteSuperiorObstaculo), obstaculo.transform.position.z);
        objetoCopia.transform.position = posicao;
        objetoCopia.SetActive(true);
        Instantiate(objetoCopia);
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
        tocarSomBotão();
        StartCoroutine(carregarTelaMenu());
    }
    public IEnumerator carregarTelaMenu() {
        yield return new WaitForSeconds(tempoTransicaoTelas);
        Transicao_Fases.tela = 1;
        Transicao_Fases.transicao = true;
    }

    private void tocarSomBotão() {
        /*Aqui será o som do botão*/
        Debug.Log("som");
    }
}
