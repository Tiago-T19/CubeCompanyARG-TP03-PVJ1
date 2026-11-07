# Trabajo Práctico N° 3
> Asignatura: Programación de Videojuegos I 
Carrera: Tecnicatura Universitaria en Diseño Integral de Videojuegos (TUDIVJ) — UNJu |
Trabajo Práctico N° 3: Sistema de Interfaz de Usuario (UI), Control de Flujo de Juego, Arquitectura de
Datos y Versionamiento Colaborativo con Ramas en Git/GitHub |
Equipo Docente: Mg. Ing. Ariel Alejandro Vega | Tecn. Kevin Alexis Roman Llampa
### Grupo: CubeCompanyARG
| **Integrantes** | **LU** | **UsuarioGithub** | **Rama** | **Rol** |
|---|---|---|---|---|
| Carla Dalila Angelo | TUV000563 | AngeloCarla | AngeloCarla | Programador |
| Sebastian Luciano Martinez | TUV000621 | SebasMartinez07 | Seba | Programador |
| Tiago L. Misael Tolaba | TUV000834 | Tiago-T19 | TolabaTiago | Programador |
| Lucas Joaquin Aldapi | TUV000559 | Readyplayer24 | LucasAldapi | Diseñador del Nivel |
| ... | ... | ... | ... | ... |
| ... | ... | ... | ... | ... |

___
## 🛠️ Version de Unity

> Unity: Unity 6.3 LTS
Proyecto: 3D
Lenguaje: C#
___

## 🎮 Descripcion del Proyecto

___
## 🕹️ Controles del Jugador
| Accion | Tecla/Boton | Descripcion|
|-----|-----------|--------|
| **Movimiento** | `W` , `A` , `S` , `D` / `Flechas` | Mueve al personaje por el escenario |
| **Saltar** | `ESPACIO` | Saltar entre plataformas |
| **Interactuar** | `E` / Contacto con zona (`Trigger`) | Interactua con objetos y recolecta |
___
## ⚙️ Mecanicas
### 1. Interaccion del jugador
- **Encargado:** Carla Angelo

    El jugador puede interactuar con los objetos del entorno al acercarse a ellos y presionar la tecla `E`. Para ello, los objetos deben tener el componente `Interactable.cs`, que implementa la interfaz `IInteractable`, y contar con un script que defina la accion que realizaran

    Esta mecanica permite recoger los objetos principales necesarios para completar la mision, y realizar otras interacciones con elementos del escenario

### 2. Mision
- **Encargado:** Carla Angelo

   El jugador debe recuperar una cantidad determinada de objetos distribuidos por el escenario y llevarlos al punto de entrega para completar la mision. Cada objeto recogido se registra y suma puntos al jugador

   Una vez entregados todos los objetos requeridos, la mision se marca como completada y habilita el nivel 2

### 3. Sistema de puntos y HighScore con PlayerPrefs
- **Encargado:** Carla Angelo

   El jugador obtiene puntos al recoger determinados objetos del escenario. El puntaje se acumula durante la partida y se compara con el record historico almacenado

   Si el jugador supera su mejor puntuacion, el record se actualiza y se guarda mediante PlayerPrefs, permitiendo conservarlo entre partidas
___
## 📝 Explicacion Tecnica de Codigo:
### 1. Interaccion del jugador
- **Encargado:** Carla Angelo
- **Scripts principales:** `PlayerInteraction.cs`, `Interactable.cs` e `IInteractable.cs`
- **Funcionamiento:** El sistema detecta los objetos cercanos, y selecciona el mas proximo, para ejecutar la accion correspondiente cuando el jugador presiona la tecla `E`

### PlayerInteraction.cs
```csharp
    private void Update()
    {
        currentObject = GetClosestObject();

        if (currentObject != null && Input.GetKeyDown(KeyCode.E))
        {
           if (currentObject.canInteract())
            {
                IInteractable selectedObject = currentObject;

                nearbyObjects.Remove(selectedObject);
                currentObject = null;

                selectedObject.Interact(); // Interaccion
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null && !nearbyObjects.Contains(interactable))
        {
            nearbyObjects.Add(interactable);
            Debug.Log("<color=white>Presiona <color=purple>E</color> para interactuar</color>");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null)
        {
            nearbyObjects.Remove(interactable);
        }
    }

    private IInteractable GetClosestObject()
    {
        IInteractable closest = null;
        float shortestDistance = Mathf.Infinity;

        nearbyObjects.RemoveAll(obj => obj == null || (MonoBehaviour)obj == null);

        foreach (IInteractable obj in nearbyObjects)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closest = obj;
            }
        }

        return closest;
    }
```
### Interactable.cs

```csharp
    // --- Todo objeto que tenga este componente sera interactuable ---
    public void Interact()
    {
        onInteract.Invoke(); // Evento de interaccion
    }
```
   `onInteract.Invoke()`: Ejecuta los eventos configurados desde el Inspector de Unity, permitiendo asignar diferentes acciones a los objetos sin modificar el script principal

### IInteractable.cs (Interfaz)
```csharp
    Transform transform { get; } // Posicion y rotacion del objeto
    bool canInteract(); // Comprobar si se puede interactuar
    void Interact(); // Interaccion
```

### 2. Mision
- **Encargado:** Carla Angelo
- **Scripts principales:** `MissionManager.cs`, `MissionDelivery.cs` e `MissionObject.cs`
- **Funcionamiento:** El sistema registra los objetos que el jugador recoge. La recoleccion se registra mediante un evento que comunica a `MissionManager` cuando un objeto de mision es recogido. Cuando el jugador llega al punto de entrega e interactua con el, `MissionDelivery` solicita al `MissionManager` que registre los objetos entregados y muestre el estado de la mision. Si se alcanza la cantidad requerida, la mision se marca como completada.

### MissionManager.cs
Controla los objetos recuperados, pendientes y entregados, y comprueba si se completo la mision

```csharp
    private int recoveredObjects; // Objetos recogidos
    public int RecoveredObjects => recoveredObjects;
    private int deliveredObjects; // Objetos entregados
    public int DeliveredObjects => deliveredObjects;
    public int PendingObjects => recoveredObjects - deliveredObjects;

    public bool AllObjectsDelivered => deliveredObjects >= totalMissionObjects;

    private void RegisterCollectedObject()
    {
        if (recoveredObjects >= totalMissionObjects) return;

        recoveredObjects++; // Suma un objeto recogido

        Debug.Log($"<color=purple>Objetos recuperados: <color=white>{recoveredObjects}/{totalMissionObjects}</color></color>");
    }

    public void DeliverObjects()
    {
        if (PendingObjects <= 0) return;

        deliveredObjects = recoveredObjects; // Registra la entrega

        Debug.Log($"<color=green>Objetos entregados: <color=white>{deliveredObjects}/{totalMissionObjects}</color></color>");

        if (AllObjectsDelivered)
        {
            Debug.Log("<color=green>Mision completada</color>");
        }
    }
```
### MissionDelivery.cs
Gestiona la entrega de objetos

```csharp
    public void Delivery()
    {
        missionManager.DeliverObjects();
        missionManager.ShowMissionStatus();
    }
```
### MissionObject.cs
Gestiona la recoleccion, notifica al sistema y suma puntos

```csharp
    public void CollectMissionObject()
    {
        if (isCollected) return;

        isCollected = true; // Marcamos el objeto como recogido

        OnMissionObjectCollected?.Invoke(); // Avisamos que se recogio un objeto

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddPoints(points); // Suma los puntos
        }

        Destroy(gameObject); // Eliminamos el objeto del escenario
    }
```
### 3. Sistema de puntos y HighScore
- **Encargado:** Carla Angelo
- **Script principal:** `ScoreManager.cs`
- **Funcionamiento:** El sistema administra los puntos obtenidos por el jugador durante la partida y registra el mejor puntaje alcanzado. El record se guarda utilizando PlayerPrefs, permitiendo conservarlo entre partidas, incluso despues de cerrar el juego

### ScoreManager.cs
Singleton: Utiliza una instancia estatica (Instance) para permitir que otros scripts accedan al administrador de puntuacion sin necesidad de buscarlo nuevamente
```csharp
    private void Start()
    {
        currentScore = 0;
        highScore = PlayerPrefs.GetInt("highScore", 0); // Cargamos el record guardado en PlayerPrefs
    }

    public void AddPoints(int points)
    {
        currentScore += points;

        if (currentScore < 0) currentScore = 0;

        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("highScore", highScore);
            PlayerPrefs.Save();

            Debug.Log($"<color=green><b>¡Nuevo record!</b></color>");
        }

        Debug.Log($"<color=yellow><b>Puntos: <color=white>{currentScore}</b></color></color>");
        Debug.Log($"<color=yellow><b>Mejor Puntaje: <color=white>{highScore}</b></color></color>");
    }
```
___

## 📸 Capturas de Pantalla
![Imagen](Images/1.png)
___

## ▶️ Como abrir y ejecutar el proyecto

1. Descargar o clonar el repositorio desde GitHub.
2. Abrir Unity Hub.
3. Abrir el proyecto utilizando **Unity 6.3 LTS**
4. Abrir la escena principal ubicada en `Assets/Scenes/`
5. Presionar **Play** para ejecutar el juego
6. Utilizar los controles indicados en la seccion **Controles del Jugador**
___
## 📚 Bibliografia
- [README.md - Markdown](https://markdown.es)
- [Material de clase — Programacion de Videojuegos I](https://virtual.unju.edu.ar/course/view.php?id=2520)
- [Material dado por el profesor Kevin Alexis Roman Llampa](https://github.com/Kivito3d/TP3_PVJ1_2026.git)
- Apuntes y ejemplos dados durante las clases de Programacion de Videojuegos I
