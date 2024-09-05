using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoxGame : MonoBehaviour
{
  [SerializeField] private GameObject box;
  [SerializeField] private GameObject[] gameObjectsFbx;
  [SerializeField] private TMP_Text mensaje;
  [SerializeField] private float distanciaMaxima = 1.5f;

  void Start()
  {
    gameObjectsFbx = GameObject.FindGameObjectsWithTag("objetfbx");
  }

  void Update()
  {
    foreach (GameObject obj in gameObjectsFbx)
    {
      if (Vector3.Distance(obj.transform.position, box.transform.position) <= distanciaMaxima)
      {
        obj.transform.parent = box.transform;
      }
    }


    if (box.transform.childCount >= 2)
    {
      string primerNombre = box.transform.GetChild(0).name;
      bool todosIguales = true;
      for (int i = 1; i < box.transform.childCount; i++)
      {
        if (box.transform.GetChild(i).name != primerNombre)
        {
          todosIguales = false;
          break;
        }
      }
      
      if (todosIguales)
      {
        mensaje.text = "¡Bien hecho!";
      }
      else
      {
        mensaje.text = "¡Mal! Los objetos no coinciden.";
      }
    }
    else
    {
      mensaje.text = "";
    }
  }
}