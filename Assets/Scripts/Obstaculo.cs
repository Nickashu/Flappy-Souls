using System.Collections;
using UnityEngine;

public class Obstaculo : MonoBehaviour{

    public float velocidade;

    void Update(){
        if (!Main_Character.morreu) {    /*Se o personagem não tiver morrido*/
            transform.Translate(Vector3.left * velocidade * Time.deltaTime);
            if (gameObject.transform.position.x <= GameController.distanciaDestroy) {
                Destroy(gameObject);
            }
        }
    }
}
