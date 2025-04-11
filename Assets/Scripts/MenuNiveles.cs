using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNiveles : MonoBehaviour
{
    [SerializeField] string escenaMenuPrincipal; // Men� principal
    [SerializeField] string escenaNivel_1; // Escena nivel 1
    [SerializeField] string escenaNivel_2; // Escena nivel 2
    [SerializeField] AudioClip PresionarBoton; // Men� de cr�ditos
    [SerializeField] AudioSource audioSource; // Men� de cr�ditos


    public void Volver() // Carga el men� de niveles
    {
        print("Bot�n Volver");
        StartCoroutine(CambiarEscenaDespuesDeSonido(PresionarBoton, escenaMenuPrincipal));
    }

    public void Nivel_1() // Carga el apartado de creditos
    {
        print("Bot�n Nivel 1");
        StartCoroutine(CambiarEscenaDespuesDeSonido(PresionarBoton, escenaNivel_1));
    }

    public void Nivel_2() // Carga el apartado de creditos
    {
        print("Bot�n Nivel 2");
        StartCoroutine(CambiarEscenaDespuesDeSonido(PresionarBoton, escenaNivel_2));
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

    // M�todo que permite reproducir cualquier clip de sonido
    private void ReproducirSonido(AudioClip clip)
    {
        if (clip != null) audioSource.PlayOneShot(clip);
    }


    // M�todo que permite detener cualquier sonido si es necesario
    private void DetenerSonidoSiEsNecesario()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
