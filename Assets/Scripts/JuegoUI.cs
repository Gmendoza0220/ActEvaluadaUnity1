using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JuegoUI : MonoBehaviour
{
    [SerializeField] AudioClip PresionarBoton; // Sonido de presionar el botón
    [SerializeField] AudioSource audioSource; // Reproductor de Sonidos
    string escenaMenuPrincipal = "MenuNiveles";


    public void Volver() // Carga el menú principal
    {
        print("Botón Volver");

        StartCoroutine(CambiarEscenaDespuesDeSonido(PresionarBoton, escenaMenuPrincipal));
    }

    private IEnumerator CambiarEscenaDespuesDeSonido(AudioClip clip, string nombreEscena)
    {

        if (clip != null)
        {
            audioSource.PlayOneShot(clip); // Reproduce el sonido
            Debug.Log("Esperando " + clip.length + " segundos antes de cambiar de escena...");
            yield return new WaitForSeconds(clip.length); // Generará un delay definido por la duración del sonido
        }

        // Cargará la escena luego de reproducirse el sonido
        SceneManager.LoadScene(nombreEscena);
    }

    /*
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    */
}
