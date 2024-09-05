// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class Streaming : MonoBehaviour
// {
//   [SerializeField] ClienteFtp clienteFtp;
//   [SerializeField] Slider ZoomCamara;
//   [SerializeField] private Camera camera;
//   [SerializeField] private RawImage rawImage;
//   [SerializeField] private TMP_Text TextoEspañol;
//   [SerializeField] private TMP_Text TextoIngles;
//   [SerializeField] private ObjectData[] objetosAgrabar;

//   private bool isStreaming = true;

//   void Start()
//   {
//     if (camera == null)
//     {
//       Debug.LogError("No se ha asignado ninguna cámara.");
//       return;
//     }

//     if (rawImage == null)
//     {
//       Debug.LogError("No se ha asignado ningún RawImage.");
//       return;
//     }

//     camera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
//     rawImage.texture = camera.targetTexture;
//   }

//   void Update()
//   {
//     if (!isStreaming)
//     {
//       TextoEspañol.text = "Español";
//       TextoIngles.text = "Inglés";
//       return;
//     }

//     bool objetoDetectado = false;
//     Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
//     RaycastHit hit;
//     if (Physics.Raycast(ray, out hit))
//     {
//       foreach (ObjectData objeto in objetosAgrabar)
//       {
//         if (hit.collider.gameObject == objeto.objeto)
//         {
//           TextoEspañol.text = objeto.textoEspañol;
//           TextoIngles.text = objeto.textoIngles;
//           objetoDetectado = true;
//           break;
//         }
//       }
//     }

//     if (!objetoDetectado)
//     {
//       TextoEspañol.text = "Español";
//       TextoIngles.text = "Inglés";
//     }

//     if (ZoomCamara != null)
//     {
//       float maxFieldOfView = 90f;
//       float zoomValue = ZoomCamara.value / 100f;
//       camera.fieldOfView = Mathf.Lerp(60f, maxFieldOfView, zoomValue);
//     }

//   }

//   public void ToggleStreaming()
//   {
//     if (!isStreaming)
//     {
//       StartStreaming();
//     }
//     else
//     {
//       StopStreaming();
//     }
//   }

//   private void StartStreaming()
//   {
//     camera.enabled = true;
//     isStreaming = true;
//   }

//   private void StopStreaming()
//   {
//     camera.enabled = false;
//     isStreaming = false;
//   }

//   [System.Serializable]
//   public class ObjectData
//   {
//     public GameObject objeto;
//     public string textoEspañol;
//     public string textoIngles;
//   }
// }


using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Streaming : MonoBehaviour
{
  [SerializeField] ClienteFtp clienteFtp;
  [SerializeField] Slider ZoomSlider;
  [SerializeField] private Camera camera;
  [SerializeField] private RawImage rawImage;
  [SerializeField] private TMP_Text TextoEspañol;
  [SerializeField] private TMP_Text TextoIngles;
  [SerializeField] private ObjectData[] objetosAgrabar;

  private bool isStreaming = true;

  void Start()
  {
    if (camera == null)
    {
      Debug.LogError("No se ha asignado ninguna cámara.");
      return;
    }

    if (rawImage == null)
    {
      Debug.LogError("No se ha asignado ningún RawImage.");
      return;
    }

    camera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
    rawImage.texture = camera.targetTexture;
  }

  void Update()
  {
    if (!isStreaming)
    {
      TextoEspañol.text = "Español";
      TextoIngles.text = "Inglés";
      return;
    }

    bool objetoDetectado = false;
    Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
    RaycastHit hit;
    if (Physics.Raycast(ray, out hit))
    {
      foreach (ObjectData objeto in objetosAgrabar)
      {
        if (hit.collider.gameObject == objeto.objeto)
        {
          TextoEspañol.text = objeto.textoEspañol;
          TextoIngles.text = objeto.textoIngles;
          objetoDetectado = true;
          break;
        }
      }
    }

    if (!objetoDetectado)
    {
      TextoEspañol.text = "Español";
      TextoIngles.text = "Inglés";
    }

    if (ZoomSlider != null)
    {
      float maxZoomValue = 100f;
      float zoomValue = ZoomSlider.value;
      float normalizedZoomValue = zoomValue / maxZoomValue;
      float maxFieldOfView = 90f;
      float minFieldOfView = 60f;
      camera.fieldOfView = Mathf.Lerp(minFieldOfView, maxFieldOfView, normalizedZoomValue);
    }

  }
  public void ToggleStreaming()
  {
    if (!isStreaming)
    {
      StartStreaming();
    }
    else
    {
      StopStreaming();
    }
  }
  public void ReproducirAudio()
  {
    Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
    RaycastHit hit;
    if (Physics.Raycast(ray, out hit))
    {
      foreach (ObjectData objeto in objetosAgrabar)
      {
        if (hit.collider.gameObject == objeto.objeto)
        {
          if (objeto.audioClip != null)
          {
            AudioSource.PlayClipAtPoint(objeto.audioClip, hit.point);
          }
          else
          {
            Debug.LogWarning("El objeto no tiene un audio clip asignado.");
          }
          break;
        }
      }
    }
  }
  private void StartStreaming()
  {
    camera.enabled = true;
    isStreaming = true;
  }

  private void StopStreaming()
  {
    camera.enabled = false;
    isStreaming = false;
  }

  [System.Serializable]
  public class ObjectData
  {
    public GameObject objeto;
    public string textoEspañol;
    public string textoIngles;
    public AudioClip audioClip;
  }
}