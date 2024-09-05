using UnityEngine;
using TMPro;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Unity.VisualScripting;
using System.Threading;

public class ClienteFtp : MonoBehaviour
{
  private TcpClient client;
  private NetworkStream stream;

  public string serverAddress = "";
  public int port = 80;

  public TMP_Text micStatusText;
  public TMP_Text audioStatusText;
  public TMP_Text TextoDeOpenAi;
  public TMP_Text CamibioIdiomaAI;
  private bool isMicrophoneActive = false;
  private AudioClip recordedClip;

  public GameObject objetoParaAnimar;

  public AudioSource AudioSource;


  private async void Start()
  {
    try
    {
      client = new TcpClient(serverAddress, port);
      stream = client.GetStream();
      Debug.Log("Cliente conectado al servidor.");
      TextoDeOpenAi.text = "Se establecio conexion con el server Exitosamente...";
    }
    catch (Exception ex)
    {
      Debug.LogError("Error al conectar al servidor: " + ex.Message);
      TextoDeOpenAi.text = "Error al hacer la conexion con el server: " + ex.Message;
    }
  }

  private async void Update()
  {
    bool isAudioPlaying = AudioSource.isPlaying;
    ToggleAnimator(objetoParaAnimar, isAudioPlaying);
  }

  private void ActivarMicrofono()
  {
    if (!isMicrophoneActive && Microphone.devices.Length > 0)
    {
      isMicrophoneActive = true;
      micStatusText.text = "Micrófono activado";
      recordedClip = Microphone.Start(null, false, 5, 44100);
    }
    else
    {
      micStatusText.text = "Micrófono no disponible o ya activado";
    }
  }

  private async void DesactivarMicrofono()
  {
    if (isMicrophoneActive)
    {
      isMicrophoneActive = false;
      micStatusText.text = "Micrófono desactivado";

      Microphone.End(null);

      if (recordedClip != null)
      {
        Debug.Log("Grabación de audio finalizada.");

        byte[] audioData = WavUtility.FromAudioClip(recordedClip);

        bool enviadoCorrectamente = await EnviarAudioAlServidor(audioData);
        if (enviadoCorrectamente)
        {
          micStatusText.text = "Audio enviado correctamente al servidor.";
          await ReceiveFile();

        }
        else
        {
          micStatusText.text = "Error al enviar el audio al servidor.";
        }
      }
      else
      {
        Debug.LogWarning("No se ha grabado ningún audio.");
      }
    }
  }

  public async void AlternarAccionMicro()
  {
    if (isMicrophoneActive)
    {
      DesactivarMicrofono();
    }
    else
    {
      ActivarMicrofono();
    }
  }

  private async Task<bool> EnviarAudioAlServidor(byte[] audioData)
  {
    try
    {
      if (client == null || !client.Connected)
      {
        Debug.LogError("El cliente no está conectado al servidor.");
        return false;
      }

      string header = "AUDIO";

      byte[] headerBytes = System.Text.Encoding.UTF8.GetBytes(header);


      byte[] headerSizeBytes = BitConverter.GetBytes(headerBytes.Length);
      await stream.WriteAsync(headerSizeBytes, 0, headerSizeBytes.Length);


      await stream.WriteAsync(headerBytes, 0, headerBytes.Length);


      byte[] fileSizeBytes = BitConverter.GetBytes(audioData.Length);
      await stream.WriteAsync(fileSizeBytes, 0, fileSizeBytes.Length);


      await stream.WriteAsync(audioData, 0, audioData.Length);

      return true;
    }
    catch (Exception ex)
    {
      Debug.LogError("Error al enviar el audio al servidor: " + ex.Message);
      return false;
    }
  }

  public void ToggleAnimator(GameObject obj, bool isAudioPlaying)
  {
    if (obj != null)
    {
      Animator animator = obj.GetComponent<Animator>();
      if (animator != null)
      {
        animator.enabled = isAudioPlaying;
        Debug.Log("Animator del objeto " + obj.name + " " + (isAudioPlaying ? "activado" : "desactivado"));
      }
      else
      {
        Debug.LogError("El objeto " + obj.name + " no tiene el componente Animator.");
      }
    }
    else
    {
      Debug.LogError("El objeto pasado como argumento es nulo.");
    }
  }
  private async Task ReceiveFile()
  {
    try
    {
      var cancellationTokenSource = new CancellationTokenSource();
      cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(18));

      byte[] headerSizeBytes = new byte[4];
      await stream.ReadAsync(headerSizeBytes, 0, headerSizeBytes.Length, cancellationTokenSource.Token);
      int headerSize = BitConverter.ToInt32(headerSizeBytes, 0);

      byte[] headerBytes = new byte[headerSize];
      await stream.ReadAsync(headerBytes, 0, headerBytes.Length, cancellationTokenSource.Token);
      string header = System.Text.Encoding.UTF8.GetString(headerBytes);

      Console.WriteLine("Encabezado del audio recibido: " + header);

      TextoDeOpenAi.text = header;

      byte[] fileSizeBytes = new byte[4];
      await stream.ReadAsync(fileSizeBytes, 0, fileSizeBytes.Length, cancellationTokenSource.Token);
      int fileSize = BitConverter.ToInt32(fileSizeBytes, 0);

      byte[] fileData = new byte[fileSize];
      int totalBytesRead = 0;

      while (totalBytesRead < fileSize)
      {
        int bytesRead = await stream.ReadAsync(fileData, totalBytesRead, fileSize - totalBytesRead, cancellationTokenSource.Token);
        if (bytesRead == 0)
        {
          throw new IOException("La conexión se cerró antes de que se recibiera el archivo completo.");
        }
        totalBytesRead += bytesRead;
      }

      AudioClip audioClip = WavUtility.ToAudioClip(fileData, 0, "ArchivoRecibido");

      AudioSource.clip = audioClip;
      AudioSource.Play();
    }
    catch (OperationCanceledException)
    {
      Console.WriteLine("El tiempo de espera de 5 segundos ha expirado.");
      TextoDeOpenAi.text = "Ocurrio un erro con tu audio, porfavor envialo de nuevo gracias.";
    }
    catch (Exception ex)
    {
      Debug.LogError("Error al recibir y reproducir audio del servidor: " + ex.Message);
      micStatusText.text = "Error al recibir y reproducir audio del servidor: " + ex.Message;
    }
  }


  private async void OnDestroy()
  {
    if (client != null)
    {
      stream.Close();
      client.Close();
      Debug.Log("Cliente desconectado del servidor FTP.");
    }
  }

  public async Task EnviarMensaje(string mensaje)
  {
    try
    {
      if (client == null || !client.Connected)
      {
        Debug.LogError("El cliente no está conectado al servidor.");
        return;
      }


      string header = "TEXTO";


      byte[] headerBytes = System.Text.Encoding.UTF8.GetBytes(header);


      byte[] headerSizeBytes = BitConverter.GetBytes(headerBytes.Length);
      await stream.WriteAsync(headerSizeBytes, 0, headerSizeBytes.Length);


      await stream.WriteAsync(headerBytes, 0, headerBytes.Length);


      byte[] mensajeBytes = System.Text.Encoding.UTF8.GetBytes(mensaje);


      byte[] mensajeSizeBytes = BitConverter.GetBytes(mensajeBytes.Length);
      await stream.WriteAsync(mensajeSizeBytes, 0, mensajeSizeBytes.Length);


      await stream.WriteAsync(mensajeBytes, 0, mensajeBytes.Length);

      Debug.Log("Mensaje enviado al servidor.");

      await RecibirMensaje();
    }
    catch (Exception ex)
    {
      Debug.LogError("Error al enviar el mensaje al servidor: " + ex.Message);
    }
  }

  public async Task<string> RecibirMensaje()
  {
    try
    {
      byte[] headerSizeBytes = new byte[4];
      await stream.ReadAsync(headerSizeBytes, 0, headerSizeBytes.Length);
      int headerSize = BitConverter.ToInt32(headerSizeBytes, 0);

      byte[] headerBytes = new byte[headerSize];
      int bytesRead = await stream.ReadAsync(headerBytes, 0, headerBytes.Length);
      if (bytesRead != headerSize)
      {
        throw new IOException("No se leyó la cantidad esperada de bytes para el encabezado.");
      }
      string header = System.Text.Encoding.UTF8.GetString(headerBytes);

      byte[] messageSizeBytes = new byte[4];
      bytesRead = await stream.ReadAsync(messageSizeBytes, 0, messageSizeBytes.Length);
      if (bytesRead != 4)
      {
        throw new IOException("No se leyó la cantidad esperada de bytes para el tamaño del mensaje.");
      }
      int messageSize = BitConverter.ToInt32(messageSizeBytes, 0);

      byte[] messageBytes = new byte[messageSize];
      bytesRead = await stream.ReadAsync(messageBytes, 0, messageBytes.Length);
      if (bytesRead != messageSize)
      {
        throw new IOException("No se leyó la cantidad esperada de bytes para el mensaje.");
      }
      string message = System.Text.Encoding.UTF8.GetString(messageBytes);

      Debug.Log("Mensaje recibido del servidor: " + message);
      TextoDeOpenAi.text = message;

      return message;
    }
    catch (Exception ex)
    {
      Debug.LogError("Error al recibir el mensaje del servidor: " + ex.Message);
      return null;
    }
  }


}