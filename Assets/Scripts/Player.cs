using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Player : MonoBehaviour
{
    // Configuración de velocidad
    [Header("Configuración de Velocidad")]
    [SerializeField] private float hSpeed = 7f;  // Velocidad horizontal
    [SerializeField] private float vSpeed = 7f;  // Velocidad de salto

    // Sonidos del personaje
    [Header("Sonidos")]
    [SerializeField] private AudioClip audioJump;
    [SerializeField] private AudioClip audioRun;
    [SerializeField] private AudioClip audioMuere;

    // Componentes y Variables
    private GameController gc;
    private Rigidbody2D rb2D;
    private AudioSource audioSource;

    // Variables a utilizar
    private bool vivo = true;
    private bool enSuelo = false;
    private Vector3 posicionInicial;
    private float limiteIzquierdo;
    private float limiteDerecho;

    // Método que se ejecuta al iniciar el Script
    void Start()
    {
        InicializarComponentes();
    }

    // Maneja las actualizaciones del jugador en torno a las física
    void FixedUpdate()
    {
        if (!vivo) return;
        ProcesarSalto();
    }

    // Maneja las actualizaciones del jugador en torno a los fotogramas
    void Update()
    {
        if (!vivo) return;
        ProcesarMovimiento();
    }

    // Método para inicializar los componentes que se utilizarán
    private void InicializarComponentes()
    {
        gc = GameController.GetInstance();
        rb2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        posicionInicial = transform.position;
        limiteIzquierdo = gc.GetLimiteIzquierdo();
        limiteDerecho = gc.GetLimiteDerecho();
    }

    // Método que nos permite procesar 
    private void ProcesarSalto()
    {
        if (Input.GetKey(KeyCode.Space) && enSuelo)
        {
            enSuelo = false;
            rb2D.velocity = Vector2.up * vSpeed;
            ReproducirSonido(audioJump);
        }
    }

    // Método que permite procesar el movimiento del personaje mediante los inputs recibidos
    private void ProcesarMovimiento()
    {
        float movimiento = Input.GetAxis("Horizontal");

        if (movimiento != 0)
        {
            MoverJugador(movimiento);
        }
        else if (enSuelo)
        {
            DetenerSonidoSiEsNecesario();
        }
    }

    // Método que aplica los movimientos al personaje
    private void MoverJugador(float direccion)
    {
        float desplazamiento = direccion * hSpeed * Time.deltaTime;
        float nuevaPosX = transform.position.x + desplazamiento;


        // En caso que no tengamos limites en el escenario
        transform.Translate(desplazamiento, 0, 0); 

        if (enSuelo && !audioSource.isPlaying)
        {
            ReproducirSonido(audioRun);
        }

        /*
        if (EstaDentroDeLimites(nuevaPosX))
        {
            //transform.Translate(desplazamiento, 0, 0);

            if (enSuelo && !audioSource.isPlaying)
            {
                ReproducirSonido(audioRun);
            }
        }
        */
    }


    private bool EstaDentroDeLimites(float x)
    {
        float anchoSprite = GetComponent<SpriteRenderer>().bounds.extents.x;
        return (x - anchoSprite > limiteIzquierdo && x + anchoSprite < limiteDerecho);
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

    // Método OnTriggerEnter que maneja los distintos tipos de colisiones
    // trigger presentes en mi videojuego
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!vivo) return;

        switch (col.gameObject.tag)
        {
            case "Suelo": // Si se colisiona con un elemento con tag "Suelo"
                enSuelo = true;
                break;

            case "Muerte": // Si se colisiona con un elemento con tag "Muerte"
                ManejarMuerte();
                break;
        }
    }

    // Método para llamar a la instancia GameController y indicarle que se perdió una vida
    private void ManejarMuerte()
    {
        vivo = false;
        audioSource.Stop();

        // Se ejecuta mientras la cantidad de vidas sea mayor a 0
        if (gc.RestaVidas() > 0)
        {
            ReproducirSonido(audioMuere);
            // Método que ejecuta una acción en segundo plano para
            // esperar el reinicio del personaje tras la muerte
            StartCoroutine(ReiniciarTrasMuerte());
        }
    }

    // Método de espera que reiniciará al jugador luego de que el sonido de muerte finalice.
    private IEnumerator ReiniciarTrasMuerte()
    {
        while (audioSource.isPlaying)
        {
            yield return new WaitForSeconds(1f);
        }

        transform.position = posicionInicial;
        vivo = true;
    }
}
