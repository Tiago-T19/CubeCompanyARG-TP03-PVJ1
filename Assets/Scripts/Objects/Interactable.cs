using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interactable : MonoBehaviour, IInteractable
{
    // --- Todo objeto que tenga este componente sera interactuable ---
    [SerializeField] private bool isEnable = true; // Variable que indica si el objeto se puede interactuar
    [SerializeField] private UnityEvent onInteract; // Evento que se ejecuta al interactuar con el objeto
    public bool canInteract() => isEnable; // Comprobar si se puede interactuar con el objeto

    public void Interact()
    {
        onInteract.Invoke(); // Evento de interaccion
    }
}
