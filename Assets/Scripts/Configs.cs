using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Configs : MonoBehaviour {

    public static bool primeiraTela = true;

    /*O que precisará ser salvo*/
    public static int dificuldade=1, indexPersonagemSelecionado=1, numMoedas=0, highScoreFacil=0, highScorePadrao = 0, highScoreHardcore = 0;
    public static float volume = 1;
    public static Dictionary<string, bool> isCompradoPersonagens = new Dictionary<string, bool>() {
        {"frog", false}, {"pinkGuy", true}, {"virtualGuy", false}, {"maskDude", false}
    };


    public static string[] personagens = { "frog", "pinkGuy", "virtualGuy", "maskDude" };    /*Esta lista precisa estar na mesma ordem em que os objetos estão ordenados*/
    public static string[] dificuldades = { "Fácil", "Padrão", "Hardcore" };

    public static Dictionary<string, string> nomesPersonagens = new Dictionary<string, string>() {
        {"frog", "Renan"}, {"pinkGuy", "Fabrício"}, {"virtualGuy", "Heitor"}, {"maskDude", "Douglas"}
    };
    public static Dictionary<string, int> precosPersonagens = new Dictionary<string, int>() {
        {"frog", 500}, {"pinkGuy", 50}, {"virtualGuy", 200}, {"maskDude", 100}
    };

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    public static void SaveData() {
        string path = Application.persistentDataPath + "/configs.bin";
        ConfigsData data = new ConfigsData(dificuldade, indexPersonagemSelecionado, numMoedas, highScoreFacil, highScorePadrao, highScoreHardcore, isCompradoPersonagens, volume);
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Create);    /*Se o arquivo não existir, será criado. Caso contrário, sera sobrescrito*/

        formatter.Serialize(fileStream, data);   /*Salvando os dados no arquivo*/
        fileStream.Close();
    }

    public static ConfigsData LoadData() {
        string path = Application.persistentDataPath + "/configs.bin";
        if (File.Exists(path)) {   /*Se o arquivo existir*/
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);

            ConfigsData data = formatter.Deserialize(fileStream) as ConfigsData;
            fileStream.Close();

            return data;
        }

        return null;
    }

    public static void ResetData() {
        dificuldade = 1;
        indexPersonagemSelecionado = 1;
        numMoedas = 0; 
        highScoreFacil = 0;
        highScorePadrao = 0; 
        highScoreHardcore = 0;
        isCompradoPersonagens = new Dictionary<string, bool>() {
            {"frog", false}, {"pinkGuy", true}, {"virtualGuy", false}, {"maskDude", false}
        };

        Debug.Log("Dados zerados");
        SaveData();
        GameController.LoadInicial();
    }
}


[System.Serializable]
public class ConfigsData {
    public int dificuldade = 1, indexPersonagemSelecionado = 1, numMoedas = 0, highScoreFacil = 0, highScorePadrao = 0, highScoreHardcore = 0;
    public float volume;
    public Dictionary<string, bool> isCompradoPersonagens = new Dictionary<string, bool>() {
        {"frog", false}, {"pinkGuy", true}, {"virtualGuy", false}, {"maskDude", false}
    };

    public ConfigsData(int dificuldade, int indexPersonagemSelecionado, int numMoedas, int highScoreFacil, int highScorePadrao, int highScoreHardcore, Dictionary<string, bool> isCompradoPersonagens, float volume) {
        this.dificuldade = dificuldade;
        this.indexPersonagemSelecionado = indexPersonagemSelecionado;
        this.numMoedas = numMoedas;
        this.highScoreFacil = highScoreFacil;
        this.highScorePadrao = highScorePadrao;
        this.highScoreHardcore = highScoreHardcore;
        this.isCompradoPersonagens = isCompradoPersonagens;
        this.volume = volume;
    }
}
