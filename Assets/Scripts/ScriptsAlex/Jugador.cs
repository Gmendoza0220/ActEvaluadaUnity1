using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script para movimiento del jugador
public class Jugador : MonoBehaviour
{
    // Atributos para la clase
    public AudioClip sonidoMorir;
    public AudioClip sonidoCaminar;
    public AudioClip sonidoSaltar;

    // Variable para indicar velocidad
    public float velocidad = 10f;

    // Boleano para definir si el jugador está vivo o muerto
    private bool vivo = true;

    // Variable para indicar la fuerza del salto
    private float fuerzaSalto = 20f;
    // Variable auxiliar para saber si se quiere saltar
    private bool realizarSalto = false;
    // Variable boolean que indica si está en colision con el suelo
    private bool enSuelo = false;

    // Variable de posición
    private Vector3 posicionInicial;

    // Variable de RigidBody
    private Rigidbody2D rb;

    // Variable GameController
    private GameController gc;

    // Variable AudioSource
    private AudioSource audioSource;

    // Variable de Animator
    private Animator animator;


    void Start()
    {
        // Inicializar instancia de RigidBody
        rb = GetComponent<Rigidbody2D>();

        // Obtenemos un game controller
        gc = GameController.GetInstance();

        // Se obtiene la posición inicial
        posicionInicial = transform.position;

        // Se obtiene el audioSource
        audioSource = GetComponent<AudioSource>();

        // Se obtiene instancia de animator
        animator = GetComponent<Animator>();

    }

    // Metodo FixedUpdate, el cual no se ejecuta en cada frame, sino cuando ocurre uno en especifico
    void FixedUpdate()
    {
        // Si el jugador no está vivo, no se puede saltar
        if (!vivo) return;

        // Aqui se realizará la accion de salto en base a la validacion en Update()
        if (realizarSalto)
        {
            // 
            // Reproducimos el sonido al saltar
            ReproducirSonido(sonidoSaltar);

            // Cuando la variable para realizar salto sea true, se generará el salto con el uso de vectores
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);

            // Las variables bool seran falsas
            enSuelo = false;
            realizarSalto = false;

            // Activamos animación de salto
            animator.SetBool("isJumping", true);
        }
        /**
        // Para realizar el salto, se debe presionar la tecla Espacio
        // El salto solo ocurrirá cuando se aprete esta tecla y la variable enSuelo sea true
        // (Osea solo cuando el jugador esté tocando el suelo)
        if (Input.GetKey(KeyCode.Space) && enSuelo)
        {
            // Al momento de saltar, la variable volverá a ser falsa
            enSuelo = false;
            // Se usara el vector inicializado en el metodo Start
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
        }
        **/
    }

    void Update()
    {

        // Si el jugador no está vivo, no se pueden realizar movimientos
        if (!vivo) return;

        /**
        // Con esto, defino que el movimiento de derecha a izquierda lo hago con las teclas A y D
        float velocidadX = Input.GetAxis("Horizontal")*Time.deltaTime*velocidad;

        // creamos un vector que almacena la posicion que tengamos
        Vector3 posicion = transform.position;

        // ahora realizamos el movimiento
        transform.position = new Vector3(velocidadX + posicion.x, posicion.y, posicion.z);
        **/
        // Con esto, defino que el movimiento de derecha a izquierda lo hago con las teclas A y D
        float movimientoX = Input.GetAxis("Horizontal");

        // Ahora para el movimiento, utilizo el metodo "velocity" de RigidBody
        rb.velocity = new Vector2(movimientoX * velocidad, rb.velocity.y);

        // Animación de movimiento (ej. correr o idle)
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        // Flip visual (mirar a la izquierda o derecha)
        if (rb.velocity.x > 0.1f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (rb.velocity.x < -0.1f)
            transform.localScale = new Vector3(-1, 1, 1);

        // Aqui se realiza la validacion de teclas y variables necesarias para realizar el salto
        if (Input.GetKey(KeyCode.Space) && enSuelo)
        {
            // Cuando la condicion se cumpla, la variable para validar el salto será true
            realizarSalto = true;
        }

    }

    // Método que permite reproducir cualquier clip de sonido
    private void ReproducirSonido(AudioClip clip)
    {
        if (clip != null) audioSource.PlayOneShot(clip);
    }

    private void DetenerSonidoSiEsNecesario()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    // Metodo onCollisionrEnter2D para detectar cuanto entremos en colision con objetos
    // que utilizen Collider2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Ahora detectaremos aquellos objetos cuyo tag sea "suelo"
        // En caso de detectarse, la variable boolean cambiara a true
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true;

            animator.SetBool("isJumping", false); // Ya no está en el aire
        }
    }
    // Metodo onCollisionrEnter2D para detectar cuanto dejemos de entrar en colision con objetos
    // que utilizen Collider2D
    private void OnCollisionExit2D(Collision2D collision)
    {
        // En caso de dejar de detectarse, la variable boolean cambiara a false
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = false;
        }
    }

    // Método OnTriggerEnter que maneja los distintos tipos de colisiones
    // trigger presentes en mi videojuego
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!vivo) return;

        switch (col.gameObject.tag)
        {
            case "Muerte": // Si se colisiona con un elemento con tag "Muerte"
                ManejarMuerte();
                break;
        }
    }

    // Método para llamar a la instancia GameController y indicarle que se perdió una vida
    private void ManejarMuerte()
    {
        // Uso de animator para animacion de Muerte
        animator.SetTrigger("Die");
        animator.Play("Death", 0, 0f); // Reproduce la animación desde el inicio

        vivo = false;
        audioSource.Stop();

        // Experimento: Detener musica de fondo
        gc.DetenerMusicaFondo();

        // Se ejecuta mientras la cantidad de vidas sea mayor a 0
        if (gc.RestaVidas() > 0)
        {
            ReproducirSonido(sonidoMorir);
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
        animator.Play("Idle", 0, 0f); // Reproduce la animación Idle desde el principio
        vivo = true;

        // Experimento: Volver a iniciar musica fondo
        gc.ReproducirMusicaFondo();
    }
}
