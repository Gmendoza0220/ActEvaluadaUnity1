using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script para movimiento del jugador
public class Jugador : MonoBehaviour
{
    // Atributos para la clase

    // Variable para indicar velocidad
    public float velocidad = 10f;

    // Variable para indicar la fuerza del salto
    private float fuerzaSalto = 20f;
    // Variable auxiliar para saber si se quiere saltar
    private bool realizarSalto = false;
    // Variable boolean que indica si está en colision con el suelo
    private bool enSuelo = false;

    // Variable de RigidBody
    private Rigidbody2D rb;

    void Start()
    {
        // Inicializar instancia de RigidBody
        rb = GetComponent<Rigidbody2D>();
    }

    // Metodo FixedUpdate, el cual no se ejecuta en cada frame, sino cuando ocurre uno en especifico
    void FixedUpdate()
    {
        // Aqui se realizará la accion de salto en base a la validacion en Update()
        if (realizarSalto)
        {
            // Cuando la variable para realizar salto sea true, se generará el salto con el uso de vectores
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
            // Las variables bool seran falsas
            enSuelo = false;
            realizarSalto = false;
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

        // Aqui se realiza la validacion de teclas y variables necesarias para realizar el salto
        if (Input.GetKey(KeyCode.Space) && enSuelo)
        {
            // Cuando la condicion se cumpla, la variable para validar el salto será true
            realizarSalto = true;
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
}
