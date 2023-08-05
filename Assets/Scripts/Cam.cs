using UnityEngine;
using UnityEngine.SceneManagement;

public class Cam : MonoBehaviour {
    public static Transform player=null;
    private Vector3 seguindo;
    private float posicaoInicialX = 0, posicaoFinalX = 100;

    private float distanciaInicialPersonagem = 8.5f;     /*Esta variável define em que posição o personagem estará na câmera*/

    private void Start() {
        string nomeCena = SceneManager.GetActiveScene().name;
    }

    void FixedUpdate() {
        if(player != null) {
            /*Fazendo a movimentação da câmera*/
            if (player.position.x > posicaoInicialX && player.position.x <= posicaoFinalX) {
                seguindo = new Vector3(player.position.x + distanciaInicialPersonagem, transform.position.y, transform.position.z);    /*Esta variável guardará a posição do personagem apenas no eixo x*/
                transform.position = Vector3.Lerp(transform.position, seguindo, 10 * Time.deltaTime);     /*Aqui é definido o que a camerá irá seguir e com qual suavidade*/
            }
        }
    }
}
