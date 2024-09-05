using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Necesario para acceder a componentes Image

public class SeñaladoresParpadean : MonoBehaviour
{
    public Material[] materials;
    public float switchInterval = 1f;

    public GameObject[] señaladores;
    private int currentMaterialIndex = 0;

    private void Start()
    {
        señaladores = FindGameObjectsWithSeñaladorName();
        StartCoroutine(SwitchMaterialsRoutine());
    }
    
    private void Update() {
        // Actualizar la lista de señaladores en cada frame
        señaladores = FindGameObjectsWithSeñaladorName();
    }

    private GameObject[] FindGameObjectsWithSeñaladorName()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        var filteredObjects = new System.Collections.Generic.List<GameObject>();

        foreach (var obj in allObjects)
        {
            if (obj.name.ToLower().Contains("señalador"))
            {
                filteredObjects.Add(obj);
            }
        }

        return filteredObjects.ToArray();
    }

    private IEnumerator SwitchMaterialsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(switchInterval);

            currentMaterialIndex = (currentMaterialIndex + 1) % materials.Length;

            foreach (var señalador in señaladores)
            {
                Image image = señalador.GetComponent<Image>();

                if (image != null)
                {
                    image.material = materials[currentMaterialIndex];
                }
                else
                {
                    Debug.LogWarning("El objeto " + señalador.name + " no tiene un componente Image.");
                }
            }
        }
    }
}
