using UnityEngine;

public class SelectPersonagem : MonoBehaviour
{
    public AudioSource somPasso;

    private void tocarSomPasso() {
        GameController.tocarSom(somPasso);
    }
}
