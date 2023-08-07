using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Classes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class Personagem {
    private string nome { get; set; }
    private int preco { get; set; }
    private bool isComprado { get; set; }

    public Personagem(string nome, int preco, bool isComprado) {
        this.nome = nome;
        this.preco = preco;
        this.isComprado = isComprado;
    }
}
