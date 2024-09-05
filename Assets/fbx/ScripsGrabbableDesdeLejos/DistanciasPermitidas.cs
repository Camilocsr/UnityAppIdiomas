using Oculus.Interaction;
using UnityEngine;
using TMPro;
using Oculus.Interaction.DistanceReticles;
using System.Collections;
using Oculus.Interaction.Input;
using System.Runtime.CompilerServices;

public class DistanciasPermitidas : MonoBehaviour
{
  [SerializeField]
  private EjecucionAlIniciar ejecucionAlIniciar;

  [SerializeField]
  private AudioTrigger audioTriger;

  [SerializeField]
  private float distanciaPermitida = 5.1f;

  private float distanciaPermitidaPanless = 5.1f;

  [SerializeField]
  private SnapInteractor[] snapInteractors;

  [SerializeField]
  private Camera camaraOculusQust;

  [SerializeField]
  private Hand RightHand;

  [SerializeField]
  private Hand LeftHand;

  [SerializeField]
  private TMP_Text textMeshPro;

  [SerializeField]
  private GameObject[] gameObjects;

  [SerializeField]
  private GameObject gameObjectInicioPreguntaObjeto;

  private int AudiosCorrectos = 0;
  private int AudiosIncorrectos = 0;

  private bool ActivacionNombresPanel = false;

  void Start()
  {
    ObjetosPull(gameObjectInicioPreguntaObjeto);
  }


  void Update()
  {
    Debug.Log("La distancia permitida es: " + distanciaPermitida);

    Vector3 centro = camaraOculusQust.transform.position;
    float anguloInicial = 0f;
    float anguloTotal = 360f;
    float separacionAngular = anguloTotal / snapInteractors.Length;
    float radio = distanciaPermitida;

    for (int i = 0; i < snapInteractors.Length; i++)
    {
      float anguloActual = anguloInicial + (separacionAngular * i);
      float x = centro.x + radio * Mathf.Cos(anguloActual * Mathf.Deg2Rad);
      float z = centro.z + radio * Mathf.Sin(anguloActual * Mathf.Deg2Rad);
      Vector3 posicion = new Vector3(x, centro.y, z);
      snapInteractors[i].transform.position = posicion;
      snapInteractors[i].DistanceThreshold = distanciaPermitida;
    }
  }
  public void activacion()
  {
    Vector3 spawnPosition = camaraOculusQust.transform.position + camaraOculusQust.transform.forward * distanciaPermitidaPanless;
    Vector3 directionToCamera = (camaraOculusQust.transform.position - spawnPosition).normalized;

    for (int i = 0; i < gameObjects.Length; i++)
    {

      gameObjects[i].SetActive(true);


      gameObjects[i].transform.position = spawnPosition;


      Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
      gameObjects[i].transform.rotation = targetRotation;
    }


    ActivacionNombresPanel = true;
  }


  public void Desactivacion()
  {
    for (int i = 0; i < gameObjects.Length; i++)
    {
      gameObjects[i].SetActive(false);
    }
    ActivacionNombresPanel = false;
  }

  public void objetos(GameObject objeto)
  {
    StartCoroutine(NombresObjetos(objeto));
  }

  public void ObjetosPull(GameObject objeto)
  {
    StartCoroutine(NombresObjetosPull(objeto));
  }

  private IEnumerator NombresObjetos(GameObject objeto)
  {
    audioTriger.PlayAudio();
    AudioSource audioSource = audioTriger.GetComponent<AudioSource>();
    yield return new WaitUntil(() => !audioSource.isPlaying);


    string nombreObjeto = objeto.name;
    textMeshPro.text = nombreObjeto;

    if (ActivacionNombresPanel)
    {
      Desactivacion();
    }
    else
    {
      activacion();
    }
  }


  private IEnumerator NombresObjetosPull(GameObject Objeto)
  {
    // Verificar si la lista de índices reproducidos está llena
    if (ejecucionAlIniciar.indicesReproducidos.Count == ejecucionAlIniciar.AudiosJuegoDeObjetos.Length)
    {
      // Si la lista está llena, simplemente terminar el método sin hacer nada
      yield break;
    }

    audioTriger.PlayAudio();
    AudioSource audioSource = audioTriger.GetComponent<AudioSource>();
    yield return new WaitUntil(() => !audioSource.isPlaying);
    Desactivacion();

    string nombreObjeto = Objeto.name;
    string nombreAudio = ejecucionAlIniciar.GetRandomAudioClipName();

    if (nombreObjeto == nombreAudio)
    {
      Debug.Log("Correcto: " + nombreObjeto);
      AudiosCorrectos++;
    }
    else
    {
      Debug.Log("Incorrecto: " + nombreObjeto);
      AudiosIncorrectos++;
    }

    activacion();

    textMeshPro.text = "Correctas " + AudiosIncorrectos + "\n Incorrectas : " + AudiosCorrectos;

    ejecucionAlIniciar.PlayAudiosRandom();
  }
}