using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    // Instancia única del GameController (Singleton)
    private static GameController instance = null;

    // Método estático para obtener la instancia de GameController
    public static GameController GetInstance() { return instance; }

    [SerializeField] private GameObject juegoUI; // Referencia a la interfaz de usuario
    //[SerializeField] private GameObject limiteIzquierdo; // Objeto que define el límite izquierdo
    //[SerializeField] private GameObject limiteDerecho;   // Objeto que define el límite derecho
    [SerializeField] private GameObject player;          // Referencia al jugador
    [SerializeField] private int maxVidas;               // Número máximo de vidas
    [SerializeField] private AudioClip audioGameOver;    // Sonido al perder todas las vidas
    [SerializeField] private AudioClip audioVictoria;    // Sonido al ganar el nivel

    private string MenuNiveles = "MenuNiveles";
    private AudioSource audioSource; // Componente de audio del GameController
    private Text vidas;              // Texto de la UI que muestra las vidas
    private Text puntos;             // Texto de la UI que muestra los puntos
    private GameObject gameOver;     // UI de Game Over
    private GameObject victory;      // UI de Victory
    private int nVidas = 0;          // Número actual de vidas
    private int nPuntos = 0;         // Puntos actuales
    private float limIzq;            // Límite izquierdo del nivel
    private float limDer;            // Límite derecho del nivel

    void Awake()
    {
        // Implementación del patrón Singleton
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject); // Si ya existe una instancia, destruir esta
        }
        else
        {
            instance = this; // Si no existe, esta instancia se convierte en la única
        }

        // Inicialización de componentes y variables
        audioSource = GetComponent<AudioSource>();
        nVidas = maxVidas;

        /*
        // Obtener los límites del nivel usando los bordes de los objetos
        limIzq = limiteIzquierdo.GetComponent<SpriteRenderer>().bounds.min.x;
        limDer = limiteDerecho.GetComponent<SpriteRenderer>().bounds.max.x;
        */

        // Obtener referencias a elementos de la UI
        vidas = juegoUI.transform.Find("Vidas").gameObject.GetComponent<Text>();
        puntos = juegoUI.transform.Find("Puntos").gameObject.GetComponent<Text>();
        gameOver = juegoUI.transform.Find("ImgGameOver").gameObject;
        victory = juegoUI.transform.Find("ImgVictory").gameObject;


        // Inicializar la UI con los valores actuales
        // Verificar si se extrayeron correctamente los elementos de la UI


        if(victory == null)
        {
            Debug.Log("No se encontró el elemento Victory");
        } else
        {
            victory.SetActive(false);
        }
        if (gameOver == null)
        {
            Debug.Log("No se encontró el elemento GameOver");
        }
        else
        {
            gameOver.SetActive(false); // Ocultar la pantalla de Game Over
        }

        if (vidas == null)
        {
            Debug.Log("No se encontró el elemento Vidas");
        }
        else
        {
            vidas.text = nVidas.ToString();
        }

        if (puntos == null)
        {
            Debug.Log("No se encontró el elemento Puntos");
        }
        else
        {
            puntos.text = nPuntos.ToString();
        }


    }

    // Devuelve la referencia del jugador
    public GameObject GetPlayer()
    {
        return player;
    }

    // Devuelve el límite izquierdo del nivel
    public float GetLimiteIzquierdo()
    {
        return limIzq;
    }

    // Devuelve el límite derecho del nivel
    public float GetLimiteDerecho()
    {
        return limDer;
    }

    // Método para reducir una vida al jugador
    public int RestaVidas()
    {
        try
        {
            if (nVidas > 0)
            {
                nVidas--; // Restar una vida

                // Si las vidas llegan a 0, reproducir sonido de Game Over
                if (nVidas == 0)
                {
                    //audioSource.PlayOneShot(audioGameOver);
                    StartCoroutine(CambiarEscenaDespuesDeSonido(audioGameOver, MenuNiveles));
                    gameOver.SetActive(true); // Mostramos el mensaje de game over
                }

                // Actualizar el texto de la UI
                vidas.text = nVidas.ToString();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e); // Imprimir error en consola en caso de fallo
        }

        return nVidas; // Retornar el número de vidas restantes
    }

    // Método para aumentar los puntos del jugador
    public void SumaPuntos(int n)
    {
        try
        {
            nPuntos += n; // Sumar los puntos
            Debug.Log("Puntos:" + nPuntos);
            puntos.text = nPuntos.ToString(); // Actualizar el texto en la UI

            if(nPuntos >= 30)
            {
                victory.SetActive(true);
                FinalizarNivel();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e); // Imprimir error en consola en caso de fallo
        }
    }

    public void FinalizarNivel()
    {
        StartCoroutine(CambiarEscenaDespuesDeSonido(audioVictoria, MenuNiveles));
    }

    // Permite cargar una escena luego de reproducir un sonido
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
}
