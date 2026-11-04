using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private List<IInteractable> nearbyObjects = new List<IInteractable>(); // Lista de objetos con los que se puede interactuar
    private IInteractable currentObject;

    private void Update()
    {
        currentObject = GetClosestObject(); // Buscamos el objeto cercano

        if (currentObject != null && Input.GetKeyDown(KeyCode.E))
        {
            if (currentObject.canInteract())
            {
                IInteractable selectedObject = currentObject;

                // Quitamos el objeto de la lista
                nearbyObjects.Remove(selectedObject);
                currentObject = null;

                selectedObject.Interact(); // Interaccion

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null && !nearbyObjects.Contains(interactable)) // Si no esta en la lista
        {
            nearbyObjects.Add(interactable); // Agrega a la lista
            Debug.Log("<color=white>Presiona <color=purple>E</color> para interactuar</color>");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null)
        {
            nearbyObjects.Remove(interactable); // Quita de la lista
        }
    }

    private IInteractable GetClosestObject()
    {
        IInteractable closest = null;
        float shortestDistance = Mathf.Infinity; // Guarda la distancia mas corta

        nearbyObjects.RemoveAll(obj => obj == null || (MonoBehaviour)obj == null); // Limpia la lista

        foreach (IInteractable obj in nearbyObjects)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position); // Calcula la distancia

            // Si la distancia es menor a la mas corta
            if (distance < shortestDistance)
            {
                shortestDistance = distance; // Guarda la distancia mas corta
                closest = obj; // Guarda el objeto mas cercano
            }
        }

        return closest; // Devuelve el objeto mas cercano
    }
}