using System.Collections.Generic;
using UnityEngine;

public class Configs : MonoBehaviour {

    public static bool primeiraTela = true;
    public static int dificuldade=1, indexPersonagemSelecionado=1, numMoedas=1000, highScoreFacil=0, highScorePadrao = 0, highScoreHardcore = 0;

    public static string[] personagens = { "frog", "pinkGuy", "virtualGuy", "maskDude" };    /*Esta lista precisa estar na mesma ordem em que os objetos estão ordenados*/
    public static string[] dificuldades = { "Fácil", "Padrão", "Hardcore" };

    public static Dictionary<string, string> nomesPersonagens = new Dictionary<string, string>() {
        {"frog", "Sapo Ninja"}, {"pinkGuy", "Cara Rosa"}, {"virtualGuy", "Ramon"}, {"maskDude", "Felipe"}
    };
    public static Dictionary<string, int> precosPersonagens = new Dictionary<string, int>() {
        {"frog", 100}, {"pinkGuy", 50}, {"virtualGuy", 50}, {"maskDude", 60}
    };
    public static Dictionary<string, bool> isCompradoPersonagens = new Dictionary<string, bool>() {
        {"frog", false}, {"pinkGuy", true}, {"virtualGuy", false}, {"maskDude", false}
    };

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

}
