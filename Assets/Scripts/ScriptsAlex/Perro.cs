using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perro : MonoBehaviour
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
        // Movemos el perro hacia el objetivo actual con velocidad constante
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Cuando la plataforma está muy cerca del punto de destino (menos de 0.1 unidades),
        // cambiamos el objetivo al otro punto para que se mueva de vuelta
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            // Si el objetivo actual es A, cambiamos a B; si es B, cambiamos a A
            target = (target == puntoA.position) ? puntoB.position : puntoA.position;


            // Invertir el sprite en el eje X
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }
}
