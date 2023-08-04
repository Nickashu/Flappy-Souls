using UnityEngine;
using UnityEngine.SceneManagement;

public class Cam : MonoBehaviour {
    private Transform player;
    private Vector3 seguindo;
    private float posicaoInicialX = 0, posicaoFinalX = 100;

    private float distanciaInicialPersonagem = 8.5f;     /*Esta variável define em que posição o personagem estará na câmera*/

    private void Start() {
        string nomeCena = SceneManager.GetActiveScene().name;
        player = GameObject.FindGameObjectWithTag("player").transform;    /*Aqui eu procuro o gameobject com a tag "player"*/
    }

    void FixedUpdate() {
        /*Fazendo a movimentação da câmera*/
        if (player.position.x > posicaoInicialX && player.position.x <= posicaoFinalX) {
            seguindo = new Vector3(player.position.x + distanciaInicialPersonagem, transform.position.y, transform.position.z);    /*Esta variável guardará a posição do personagem apenas no eixo x*/
            transform.position = Vector3.Lerp(transform.position, seguindo, 10 * Time.deltaTime);     /*Aqui é definido o que a camerá irá seguir e com qual suavidade*/
        }
    }
}
