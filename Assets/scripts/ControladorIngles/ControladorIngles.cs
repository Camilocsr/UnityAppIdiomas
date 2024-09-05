using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ControladorIngles : MonoBehaviour
{
    public ClienteFtp clienteFtp;
    public OVRCameraRig oculusCameraRig;
    [SerializeField] private Transform camaraOculus;
    [SerializeField] private Vector3[] posicionesDeCamaraOculus;
    [SerializeField] private GameObject[] _prefabCiudades;
    [SerializeField] private GameObject[] _prefabPaneles;

    private bool objetoActivo = false;

    static string nombre = "";

    [SerializeField] private Button[] _buttons;
    [SerializeField] private AnimatorOverrideLayerWeigth _animatorOverrideLayerWeigth;

    private void Start()
    {
        oculusCameraRig = FindObjectOfType<OVRCameraRig>();

        foreach (Button button in _buttons)
        {
            button.onClick.AddListener(() => ActivarAnimacion(button.name));
        }
    }

    private void ActivarAnimacion(string nombreAnimacion)
    {
        _animatorOverrideLayerWeigth.SetOverrideLayerActive(true); // Activar la animación
    }

    private void Update()
    {
        if (!objetoActivo)
        {
            ActivarObjeto(nombre);
        }

        if (objetoActivo)
        {
            if (oculusCameraRig != null)
            {
                // Obtiene la posición y dirección de la cámara derecha
                Transform camTransform = oculusCameraRig.rightEyeAnchor;
                Vector3 camForward = camTransform.forward;

                // Calcula la posición frente a la cámara
                Vector3 objetoPosition = camTransform.position + camForward * 2f;

                // Mira hacia la cámara
                GameObject objetoActivar = GameObject.Find(nombre);
                if (objetoActivar != null)
                {
                    // Calcula la dirección opuesta a la cámara
                    Vector3 oppositeDirection = -camForward;

                    // Calcula la rotación necesaria para mirar en la dirección opuesta
                    Quaternion targetRotation = Quaternion.LookRotation(oppositeDirection);

                    // Aplica una rotación adicional de 180 grados en el eje Y
                    Quaternion finalRotation = targetRotation * Quaternion.Euler(0, 180, 0);

                    // Establece la rotación y posición del objeto
                    objetoActivar.transform.rotation = finalRotation;
                    objetoActivar.transform.position = objetoPosition;
                }
                else
                {
                    Debug.LogWarning("No se encontró el objeto con el nombre especificado: " + nombre);
                }
            }
            else
            {
                Debug.LogWarning("No se encontró el OVRCameraRig en la escena.");
            }
        }
    }

    public void ActivarObjeto(string nombreObjeto)
    {
        GameObject objetoEncontrado = GameObject.Find(nombreObjeto);
        if (objetoEncontrado != null)
        {
            objetoActivo = true;
        }
        else
        {
            Debug.LogWarning("No se encontró un objeto con el nombre especificado: " + nombreObjeto);
        }
    }

    public async void cambiarIdioma(string stringidioma)
    {
        if (stringidioma == "Spanish")
        {
            await clienteFtp.EnviarMensaje(stringidioma);
        }
        else if (stringidioma == "English")
        {
            await clienteFtp.EnviarMensaje(stringidioma);
        }
        else if (stringidioma == "French")
        {
            await clienteFtp.EnviarMensaje(stringidioma);
        }
    }

    private void DesactivarTodosPrefabsCiudades()
    {
        foreach (GameObject ciudad in _prefabCiudades)
        {
            ciudad.SetActive(false);
        }
    }

    public void ActivarPrefabCiudad(string nombreCiudad)
    {
        DesactivarTodosPrefabsCiudades();

        for (int i = 0; i < _prefabCiudades.Length; i++)
        {
            GameObject ciudad = _prefabCiudades[i];
            if (ciudad.name == nombreCiudad)
            {
                ciudad.SetActive(true);
                Debug.Log("Prefab de ciudad activado: " + nombreCiudad);

                if (nombreCiudad == "Taxi")
                {
                    if (posicionesDeCamaraOculus.Length >= 2)
                    {
                        // Establecer la posición de la cámara en el mundo global
                        camaraOculus.transform.position = posicionesDeCamaraOculus[1];
                    }
                    else
                    {
                        Debug.LogError("¡No hay suficientes posiciones de cámara en el array para la ciudad activada!");
                    }
                }
                else if (nombreCiudad == "Assets_escenario_Agencia")
                {
                    if (posicionesDeCamaraOculus.Length >= 2)
                    {
                        // Establecer la posición de la cámara en el mundo global
                        camaraOculus.transform.position = posicionesDeCamaraOculus[0];
                    }
                    else
                    {
                        Debug.LogError("¡No hay suficientes posiciones de cámara en el array para la ciudad activada!");
                    }
                }
            }
        }
    }

    public void ActivarPrefabPaneles(string nombreCiudad)
    {
        GameObject ciudadActivada = null;

        foreach (GameObject ciudad in _prefabPaneles)
        {
            if (ciudad.name == nombreCiudad)
            {
                bool estadoActual = ciudad.activeSelf;
                ciudad.SetActive(!estadoActual);
                ciudadActivada = ciudad;

                if (!estadoActual)
                {
                    Debug.Log("Prefab de ciudad activado: " + nombreCiudad);
                    nombre = nombreCiudad;
                }
                else
                {
                    Debug.Log("Prefab de ciudad desactivado: " + nombreCiudad);
                }
            }
            else
            {
                ciudad.SetActive(false);
            }
        }
    }
}
