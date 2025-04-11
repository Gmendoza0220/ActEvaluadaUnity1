using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] string escenaSelectorNiveles; // Menú de selecciíon de niveles
    [SerializeField] string escenaCreditos; // Menú de créditos
    [SerializeField] AudioClip PresionarBoton; // Sonido de presionar el botón
    [SerializeField] AudioSource audioSource; // Reproductor de Sonidos


    public void Iniciar() // Carga el menú de niveles
    {
        print("Botón Inicial");
        StartCoroutine(CambiarEscenaDespuesDeSonido(PresionarBoton, escenaSelectorNiveles));
    }

    public void Creditos() // Carga el apartado de creditos
    {
        print("Botón Creditos");
        StartCoroutine(CambiarEscenaDespuesDeSonido(PresionarBoton, escenaCreditos));
    }

    private IEnumerator CambiarEscenaDespuesDeSonido(AudioClip clip, string nombreEscena)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
            Debug.Log("Esperando " + clip.length + " segundos antes de cambiar de escena...");
            yield return new WaitForSeconds(clip.length);
        }

        SceneManager.LoadScene(nombreEscena);
    }

    // Método que permite reproducir cualquier clip de sonido
    private void ReproducirSonido(AudioClip clip)
    {
        if (clip != null) audioSource.PlayOneShot(clip);
    }


    // Método que permite detener cualquier sonido si es necesario
    private void DetenerSonidoSiEsNecesario()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

}
