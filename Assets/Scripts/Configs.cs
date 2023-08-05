using System.Collections.Generic;
using UnityEngine;

public class Configs : MonoBehaviour {

    public static bool primeiraTela = true;
    public static int dificuldade=1, indexpersonagemSelecionado=1;

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

}
