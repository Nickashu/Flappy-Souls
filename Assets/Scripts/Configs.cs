using System.Collections.Generic;
using UnityEngine;

public class Configs : MonoBehaviour {

    public static bool primeiraTela = true;
    public static int dificuldade=1, indexpersonagemSelecionado=3, numMoedas=0, highScore=0;

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

}
