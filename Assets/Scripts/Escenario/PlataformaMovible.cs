using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaMovible : MonoBehaviour
{

    public Transform puntoA;
    public Transform puntoB;
    public float speed = 2f;

    // Variable privada para saber hacia qué punto se está moviendo actualmente
    private Vector3 target;

    void Start()
    {
        // Al iniciar el juego, establecemos que el primer destino será el punto B
        target = puntoB.position;
    }

    void Update()
    {
        // Movemos la plataforma hacia el objetivo actual con velocidad constante
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Cuando la plataforma está muy cerca del punto de destino (menos de 0.1 unidades),
        // cambiamos el objetivo al otro punto para que se mueva de vuelta
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            // Si el objetivo actual es A, cambiamos a B; si es B, cambiamos a A
            target = (target == puntoA.position) ? puntoB.position : puntoA.position;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform); // hace al jugador hijo de la plataforma
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null); // suelta al jugador
        }
    }

    /* OLD
    // Este método hace que el jugador "se pegue" a la plataforma al pararse sobre ella,
    // de modo que se mueva con la plataforma sin problemas.
    void OnCollisionEnter(Collision collision)
    {
        // Verificamos si el objeto que tocó la plataforma tiene el tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Hacemos que el jugador se convierta en hijo de la plataforma
            // Esto hace que se mueva junto con ella
            collision.transform.parent = transform;
        }
    }

    // Este método se llama cuando el jugador deja de tocar la plataforma
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Eliminamos la relación de padre para que el jugador vuelva a moverse libremente
            collision.transform.parent = null;
        }
    }
    */
}
