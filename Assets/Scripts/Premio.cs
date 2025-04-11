using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Premio : MonoBehaviour
{
    // Indica cuando la moneda aun no a sido recogida
    private bool vivo = true;

    // Evento OnTrigger que detecta las colisiones tipo Trigger
    void OnTriggerEnter2D(Collider2D col)
    {
        // Se obtiene el objeto con el que se está colisionando
        GameObject obj = col.gameObject;

        // Verifica que el tag del objeto sea el del player
        if (vivo && obj.tag == "Player")
        {
            // Indica que ya no está habilitada la moneda
            vivo = false;

            // Se obtiene una instancia del GameController
            GameController gc = GameController.GetInstance();

            // Se suma un punto en la partida
            gc.SumaPuntos(1);

            // Se desactiva el renderizado del sprite de la moneda
            GetComponent<SpriteRenderer>().enabled = false;

            // Se reproduce el sonido de la moneda
            GetComponent<AudioSource>().Play();

            // Se elimina el objeto de la moneda
            Destroy(gameObject, 1.0f);
        }
    }
}
