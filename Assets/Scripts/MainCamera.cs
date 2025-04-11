using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private GameObject player; // Instancia de jugador
    private float limIzq; // Variable que almacena el limite Izquierdo
    private float limDer; // Variable que almacena el limite Derecho
    private Vector3 offset; // Desplazamiento entre la cámara y el jugador

    /*
        player: Objeto del jugador que la cámara sigue.
        limIzq y limDer: Definen los límites de movimiento de la cámara.
        offset: Mantiene la cámara a una distancia fija del jugador.
     */

    void Start()
    {
        // Se obtiene la instancia del GameController
        GameController gc = GameController.GetInstance();

        // Se obtiene una instancia del personaje
        player = gc.GetPlayer();

        // Se obtiene la diferencia de distancia entre el player y la cámara
        // para que la cámara no esté completamente fija al personaje.
        offset = transform.position - player.transform.position;

        // Se obtienen los límites del escenario
        limIzq = gc.GetLimiteIzquierdo();
        limDer = gc.GetLimiteDerecho();
    }

    void LateUpdate()
    {
        // Altura de a cámara
        float verticalHeightSeen = Camera.main.orthographicSize * 2.0f;

        // Ancho de la cámara
        float verticalWidthSeen = verticalHeightSeen * Camera.main.aspect;

        // Distancia horizontal desde el centro hasta un borde
        float dx = verticalWidthSeen / 2.0f;

        // Calcula la nueva poscición de la cámara
        Vector3 newPos = player.transform.position + offset;
        // Mantiene la altura fija (solo se mueve en x)
        newPos.y = transform.position.y;

        // Verifica si la cámara está dentro de los límites
        if (newPos.x - dx > limIzq && newPos.x + dx < limDer)
        {
            transform.position = newPos; // Actualiza la posición de la cámara
        }
    }
}
