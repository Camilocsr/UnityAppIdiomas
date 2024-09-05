using System.Collections;
using UnityEngine;

public class ActivarPanelAlfinalDeAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public GameObject objetoAActivarAlFinal;

    private void OnEnable()
    {
        
        if (gameObject.CompareTag("Taxi"))
        {
            if (audioSource != null && audioClip != null)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
                StartCoroutine(EsperarFinAudio());

                Debug.Log("Ejecutando audio");
            }
            else
            {
                Debug.LogWarning("El audio source o el clip de audio no están asignados.");
            }
        }
    }

    private IEnumerator EsperarFinAudio()
    {
        
        yield return new WaitForSeconds(audioClip.length);
        
        if (objetoAActivarAlFinal != null)
        {
            objetoAActivarAlFinal.SetActive(true);
        }
        else
        {
            Debug.LogWarning("El objeto a activar al final del audio no está asignado.");
        }
    }
}