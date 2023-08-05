using UnityEngine;
using UnityEngine.SceneManagement;

public class Cam : MonoBehaviour {
    public static Transform player=null;
    private Vector3 seguindo;
    private float posicaoInicialX = 0, posicaoFinalX = 100;

    private float distanciaInicialPersonagem = 8.5f;     /*Esta vari�vel define em que posi��o o personagem estar� na c�mera*/

    private void Start() {
        string nomeCena = SceneManager.GetActiveScene().name;
    }

    void FixedUpdate() {
        if(player != null) {
            /*Fazendo a movimenta��o da c�mera*/
            if (player.position.x > posicaoInicialX && player.position.x <= posicaoFinalX) {
                seguindo = new Vector3(player.position.x + distanciaInicialPersonagem, transform.position.y, transform.position.z);    /*Esta vari�vel guardar� a posi��o do personagem apenas no eixo x*/
                transform.position = Vector3.Lerp(transform.position, seguindo, 10 * Time.deltaTime);     /*Aqui � definido o que a camer� ir� seguir e com qual suavidade*/
            }
        }
    }
}
