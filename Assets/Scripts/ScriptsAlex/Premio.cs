using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Premio : MonoBehaviour
{
    // Atributos

    // Define que el objeto existe
    private bool activo = true;

    // Este metodo detecta cuando algo se "cruza" con el
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Obtener instancias de objetos en el nivel
        GameObject objeto = collision.gameObject;

        // Si el premio esta activo y el objeto con el que
        // colisiona tiene el tag "Player"
        if (activo && objeto.tag == "Player")
        {
            // Dejara de estar disponible
            activo = false;

            // El sprite desaparecerá
            GetComponent<SpriteRenderer>().enabled = false;
            // Reproducirá el audio que tenga asociado
            GetComponent<AudioSource>().Play();

            // Se destruira el objeto despues de 1 segundo
            Destroy(gameObject, 1.0f);
        }
    }
}
