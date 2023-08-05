using UnityEngine;

public class Configs : MonoBehaviour {

    public static bool primeiraTela = true;

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

}
