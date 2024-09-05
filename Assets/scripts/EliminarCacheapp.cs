using UnityEngine;

public class EliminarCacheapp : MonoBehaviour
{
    void Start()
    {
        // Eliminar preferencias guardadas relacionadas con la cámara
        PlayerPrefs.DeleteKey("CamaraPosicionX");
        PlayerPrefs.DeleteKey("CamaraPosicionY");
        PlayerPrefs.DeleteKey("CamaraPosicionZ");

        // Restablecer la posición de la cámara a un valor predeterminado (por ejemplo, [0, 0, 0])
        Transform camaraTransform = Camera.main.transform; // Obtener el transform de la cámara principal
        camaraTransform.position = Vector3.zero; // Establecer la posición de la cámara en [0, 0, 0]
    }
}