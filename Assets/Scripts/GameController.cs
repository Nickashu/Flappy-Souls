using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject cam, obstaculo;
    public static float distanciaSpawn, distanciaDestroy;

    private float limiteSuperiorObstaculo = 2.08f, limiteInferiorObstaculo = -4.2f;
    private int cont = 0;

    void Update() {
        distanciaDestroy = cam.GetComponent<Transform>().position.x - 17;
        distanciaSpawn = cam.GetComponent<Transform>().position.x + 13;
        if (!Main_Character.morreu && Main_Character.comecouJogo) {
            if (cont <= 2)
                cont++;
            StartCoroutine(spawnarObstaculo());
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
}
