using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transicao_Fases : MonoBehaviour {
    private Animator anim;
    private float tempoTransicao = 0.4f;
    public static bool transicao = false;
    public static int tela=1;    /*1 - Menu; 2 - Personagens; 3 - Configurações*/

    private void Start() {
        anim = gameObject.GetComponent<Animator>();
        if (!Configs.primeiraTela) {           /*Para não acontecer a transição logo no início do jogo*/
            anim.SetBool("aparecerTela", true);
        }
    }
    private void Update() {
        if (transicao) {
            carregarProximaCena();
            transicao = false;
        }
    }

    public void carregarProximaCena() {
        StartCoroutine(carregarLevel(tela));    /*Chamando a co-rotina que carrega a cena*/
    }

    private IEnumerator carregarLevel(int index) {
        Configs.primeiraTela = false;
        index -= 1;
        anim.SetBool("escurecerTela", true);     /*Ativando a animação de transição entre telas*/
        yield return new WaitForSeconds(tempoTransicao);
        SceneManager.LoadScene(index);
    }
}