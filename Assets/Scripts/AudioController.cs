using System.Collections;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource musicaMenu, musicaJogo, musicaAmbiente;
    public static AudioController instancia;
    private bool podeTocar=false, isTocando=false;

    public static int INDEX_MUSICA_JOGO = 1, INDEX_MUSICA_MENU = 2, INDEX_MUSICA_AMBIENTE = 3;

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
        /*Para evitar o bug do travamento ao começar o jogo*/
        tocarMusicaRapido(INDEX_MUSICA_JOGO);
        tocarMusicaRapido(INDEX_MUSICA_MENU);
        tocarMusicaRapido(INDEX_MUSICA_AMBIENTE);
        if(instancia == null)
            instancia = this;
    }

    private void Update() {
        if (Configs.primeiraTela && podeTocar && !isTocando) {
            tocarMusica(3);
            isTocando = true;
        }
    }

    public void tocarMusicaRapido(int numTrack) {    /*Para evitar bugs de travamento*/
        StartCoroutine(tocarMusicaRapidoEnumerator(numTrack));
    }
    public void tocarMusica(int numTrack) {
        StartCoroutine(tocarMusicaFade(numTrack));
    }
    public void pararMusica(int numTrack) {
        StartCoroutine(pararMusicaFade(numTrack));
    }

    private IEnumerator tocarMusicaFade(int numTrack) {
        float timeToFade = 0.5f, timeElapsed = 0, volumeDesejado=1;
        AudioSource audio = null;
        if (numTrack == 1) {
            audio = musicaJogo;
            volumeDesejado = 0.8f;
            timeToFade = 1;
        }
        else if (numTrack == 2) {
            audio = musicaMenu;
            volumeDesejado = 0.8f;
        }
        else if (numTrack == 3) {
            audio = musicaAmbiente;
            volumeDesejado = 0.4f;
            timeToFade = 1;
        }
        audio.Play();
        while (timeElapsed < timeToFade) {
            audio.volume = Mathf.Lerp(0, volumeDesejado, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator pararMusicaFade(int numTrack) {
        float timeToFade = 0.5f, timeElapsed = 0;
        AudioSource audio = null;
        if (numTrack == 1)
            audio = musicaJogo;
        else if (numTrack == 2)
            audio = musicaMenu;
        else if (numTrack == 3)
            audio = musicaAmbiente;

        while (timeElapsed < timeToFade) {
            audio.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        audio.Stop();
    }

    private IEnumerator tocarMusicaRapidoEnumerator(int numTrack) {
        AudioSource audio = null;
        if (numTrack == 1)
            audio = musicaJogo;
        else if (numTrack == 2)
            audio = musicaMenu;
        else if (numTrack == 3)
            audio = musicaAmbiente;
        audio.volume = 0;
        audio.Play();
        yield return new WaitForSeconds(0.2f);
        audio.Stop();
        podeTocar = true;
    }
}