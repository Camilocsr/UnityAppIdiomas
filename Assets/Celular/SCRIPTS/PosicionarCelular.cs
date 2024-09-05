using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PocicionMenus : MonoBehaviour
{
  public OVRHand hand;
  public GameObject Inventory;
  public GameObject Anchor;
  public GameObject InicialPrefab;
  bool UIActive;

  private bool isPanelPinned = false;

  private void Start()
  {
    Inventory.SetActive(false);
    UIActive = false;
  }

  private void Update()
  {
    if (hand.GetFingerIsPinching(OVRHand.HandFinger.Thumb) && hand.GetFingerIsPinching(OVRHand.HandFinger.Index))
    {
      UIActive = !UIActive;

      if (UIActive)
      {
        InicialPrefab.SetActive(true);
        Inventory = InicialPrefab;
        isPanelPinned = false;

        // Alinea la rotación del objeto con la cámara
        AlignObjectWithCamera();
      }
    }

    if (UIActive)
    {
      if (hand.GetFingerIsPinching(OVRHand.HandFinger.Thumb) && hand.GetFingerIsPinching(OVRHand.HandFinger.Middle))
      {
        isPanelPinned = true;
      }

      if (isPanelPinned)
      {
        // Lógica cuando el panel está "pinned"
        // Puedes agregar lógica adicional si es necesario
      }
      else
      {
        Inventory.transform.position = Anchor.transform.position;
        // Mantén la rotación del objeto alineada con la cámara
        AlignObjectWithCamera();
      }
    }
  }

  private void AlignObjectWithCamera()
  {
    // Obtiene la dirección de la cámara desde el objeto hasta la cámara
    Vector3 lookAtDir = Camera.main.transform.position - Inventory.transform.position;

    // Calcula la rotación necesaria para mirar hacia la cámara
    Quaternion lookRotation = Quaternion.LookRotation(lookAtDir, Vector3.up);

    // Aplica la rotación al objeto
    Inventory.transform.rotation = lookRotation;
  }
}
