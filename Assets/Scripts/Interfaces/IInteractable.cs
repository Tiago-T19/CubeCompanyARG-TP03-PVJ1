using UnityEngine;

public interface IInteractable
{
    Transform transform { get; } // Posicion y rotacion del objeto
    bool canInteract(); // Comprobar si se puede interactuar
    void Interact(); // Interaccion
}
