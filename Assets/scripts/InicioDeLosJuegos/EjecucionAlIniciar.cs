using UnityEngine;
using System.Collections.Generic;

public class EjecucionAlIniciar : MonoBehaviour
{
    [SerializeField]
    private AudioClip AudioDeBienvenida;
    public AudioClip[] AudiosJuegoDeObjetos;

    [SerializeField]
    private AudioSource audioSource;

    public List<int> indicesReproducidos = new List<int>();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = AudioDeBienvenida;

        Invoke("ReproducirAudio", 0.1f);

    }

    void ReproducirAudio()
    {
        audioSource.Play();
    }

    public void PlayAudiosRandom()
    {
        if (AudiosJuegoDeObjetos.Length == 0)
        {
            Debug.LogWarning("No hay audios para reproducir.");
            return;
        }

        int index;
        do
        {
            index = Random.Range(0, AudiosJuegoDeObjetos.Length);
        } while (indicesReproducidos.Contains(index));

        audioSource.clip = AudiosJuegoDeObjetos[index];
        audioSource.Play();

        indicesReproducidos.Add(index);

        if (indicesReproducidos.Count == AudiosJuegoDeObjetos.Length)
        {
            indicesReproducidos.Clear();
        }
    }

    public string GetRandomAudioClipName()
    {
        if (AudiosJuegoDeObjetos.Length == 0)
        {
            Debug.LogWarning("No hay audios para reproducir.");
            return "";
        }

        int index;
        do
        {
            index = Random.Range(0, AudiosJuegoDeObjetos.Length);
        } while (indicesReproducidos.Contains(index));

        return AudiosJuegoDeObjetos[index].name;
    }
}