using UnityEngine;

public class Parallax : MonoBehaviour
{

    private float lenght, startPos;
    private Transform cam;

    public float efeitoParallax;

    void Start(){
        startPos = transform.position.x;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;   /*Pegando a largura do sprite*/
        cam = Camera.main.transform;   /*Pegando o transform da câmera principal*/
    }

    void Update(){

        float restarPosition = cam.transform.position.x * (1 - efeitoParallax);
        float distancia = cam.transform.position.x * efeitoParallax;
        transform.position = new Vector3(startPos + distancia, transform.position.y, transform.position.z);

        if(restarPosition > startPos + lenght)
            startPos += lenght;
        else if (restarPosition < startPos - lenght)
            startPos -= lenght;

    }
}
