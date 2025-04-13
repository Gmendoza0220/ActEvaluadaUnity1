using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script para la Camara Principal
public class MainCamera : MonoBehaviour
{
    // Atributos
    private GameObject player;      // variable que hace referencia al jugador
    public Vector3 offset;          // variable que almacena la distancia entre la camara y el jugador

    void Start()
    {
        // Obtener instancia del jugador buscando por tag
        player = GameObject.FindGameObjectWithTag("Player");

        // Si se encuentra, se calcula la distancia inicial entre la c�mara y el jugador
        if (player != null)
        {
            offset = transform.position - player.transform.position;
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        // C�lculo de dimensiones visibles de la c�mara
        // Reciclado de codigo del profesor
        float verticalHeightSeen = Camera.main.orthographicSize * 2.0f;
        float verticalWidthSeen = verticalHeightSeen * Camera.main.aspect;
        float dx = verticalWidthSeen / 2.0f;

        // Nueva posici�n basada en el jugador y el offset
        Vector3 nuevaPosicion = player.transform.position + offset;

        // Variable opcional: mantener la c�mara fija en Y
        // nuevaPosicion.y = transform.position.y;

        // Versi�n simple: solo seguir al jugador sin l�mites
        transform.position = nuevaPosicion;

        // Si quisieras usar l�mites laterales, podr�as hacer algo como:
        // if (newPos.x - dx > limIzq && newPos.x + dx < limDer)
        //     transform.position = newPos;
    }
}


