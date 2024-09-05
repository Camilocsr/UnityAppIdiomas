using UnityEngine;

public class ActivarObjetoFrenteACamara : MonoBehaviour
{
    [SerializeField] private ClienteFtp clienteFtp;
    [SerializeField] private OVRCameraRig[] camarasOculus;
    [SerializeField] private OVRHand[] hands;
    [SerializeField] private GameObject objetoAActivar;
    [SerializeField] private GameObject[] _prefabCiudades;
    [SerializeField] private GameObject[] _prefabPaneles;

    [SerializeField] private bool panelActivado = false;

    [SerializeField] private float distanciaDesdeCamara = 2f;

    private bool objetoActivado; // Variable para controlar si el objeto ha sido activado

    void Start()
    {
        objetoAActivar.SetActive(false);
        objetoActivado = false;
    }

    void Update()
{
    foreach (var hand in hands) // Recorre todas las manos en el array
    {
        if (hand.GetFingerIsPinching(OVRHand.HandFinger.Thumb) && hand.GetFingerIsPinching(OVRHand.HandFinger.Ring))
        {
            if (!objetoActivado)
            {
                if (camarasOculus != null && camarasOculus.Length > 0)
                {
                    // Buscar la cámara activa
                    OVRCameraRig camaraActiva = null;
                    foreach (var camara in camarasOculus)
                    {
                        if (camara.gameObject.activeSelf)
                        {
                            camaraActiva = camara;
                            break;
                        }
                    }

                    if (camaraActiva != null)
                    {
                        // Calcula la posición frente a la cámara activa
                        Transform camaraTransform = camaraActiva.centerEyeAnchor;
                        Vector3 posicionFrenteACamara = camaraTransform.position + camaraTransform.forward * distanciaDesdeCamara;

                        // Establece la posición del objeto frente a la cámara activa
                        objetoAActivar.transform.position = posicionFrenteACamara;
                        objetoAActivar.SetActive(true);
                        objetoActivado = true;
                    }
                    else
                    {
                        Debug.LogWarning("No se encontró ninguna cámara Oculus activa en el array.");
                    }
                }
                else
                {
                    Debug.LogWarning("No se han asignado cámaras de Oculus en el inspector.");
                }
            }
            else
            {
                objetoAActivar.SetActive(false);
                objetoActivado = false;
            }
        }
    }
}


    // Método para activar el objeto frente a una cámara específica en el array
    public void ActivarObjetoFrenteACamaraOculus(int indiceCamara)
    {
        if (indiceCamara >= 0 && indiceCamara < camarasOculus.Length && objetoAActivar != null)
        {
            Transform camaraTransform = camarasOculus[indiceCamara].centerEyeAnchor;
            Vector3 posicionFrenteACamara = camaraTransform.position + camaraTransform.forward * 2f;
            objetoAActivar.SetActive(true);
            objetoAActivar.transform.position = posicionFrenteACamara;
        }
        else
        {
            Debug.LogWarning("El índice de la cámara Oculus es inválido o el objeto a activar no está asignado.");
        }
    }

    private void DesactivarTodosPrefabsCiudades()
    {
        foreach (GameObject ciudad in _prefabCiudades)
        {
            ciudad.SetActive(false);
        }
    }

    private void DesactivarTodosPrefabsPaneles()
    {
        foreach (GameObject panel in _prefabPaneles)
        {
            panel.SetActive(false);
        }
    }

    public void ActivarPrefabCiudad(string nombreCiudad)
    {
        DesactivarTodosPrefabsCiudades();

        foreach (GameObject ciudad in _prefabCiudades)
        {
            if (ciudad.name == nombreCiudad)
            {
                ciudad.SetActive(true);
                break;
            }
        }

        Debug.Log("Prefab de ciudad activado: " + nombreCiudad);
    }

    public void ActivarPrefabPaneles(string nombreCiudad)
    {
        DesactivarTodosPrefabsPaneles();

        foreach (GameObject panel in _prefabPaneles)
        {
            if (panel.name == nombreCiudad)
            {
                panel.SetActive(true);
                break;
            }
        }

        Debug.Log("Prefab de ciudad activado: " + nombreCiudad);
    }
}