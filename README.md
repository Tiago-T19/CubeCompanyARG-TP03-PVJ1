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
| **Saltar** | `ESPACIO` | Saltar por el mapa |
| **Interactuar** | `E` / Contacto con zona (`Trigger`) | Interactua con objetos y recolecta |
| **Ver Mision** | `Q` | permite ver el estado de los objetos de mision si fueron recogidos o no |
| **Inventario** | `TAB` | Interactua con objetos y recolecta |
| **Disparar/Atacar** | `CLICK IZQUIERDO` | Disapara y ataca  |
| **Usar consumible** | `CLICK DERECHO` | Permite usar el botiquin  |


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
   
### 4. Sistema de inventario
- **Encargado:** Tolaba Tiago

El jugador cuenta con un inventario de acceso rápido de 3 espacios (slots) que le permite equipar y alternar dinámicamente entre su armamento (como un rifle M4 y un cuchillo) y objetos consumibles.

Los botiquines de curación distribuidos por el mapa pueden recogerse e interactuar con este sistema, permitiendo al jugador acumular múltiples unidades para gestionar su salud de manera estratégica durante la partida.


### 5. Sistema de equipamiento
- **Encargado:** Tolaba Tiago

El jugador tiene un sistema de equipamiento que  se encarga de administrar los recursos que tienen equipados en el inventario para que puedan usarse,  puede cambiar el objeto equipado con la "Rueda del Raton", se usara un contorno blanco para demostrar el item escogido.

### 6. Sistema de Raycast para las armas
- **Encargado:** Tolaba Tiago

El Jugador Este sistema se encarga de crear un rayo para el arma M4 que le permite guiar el trayecto de la bala y impactar con los enemigos y tambien permite guiar el uso del cuchillo.



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

### 4. Sistema de inventario
- **Encargado:** Tolaba Tiago
- **Scripts principales:** `InventoryManager.cs`, `HotBarUIController.cs`,`InventorySlot.cs`, `InventoryController.cs`, `ItemData.cs` y `PickAble.cs`
- **Funcionamiento:**Gestiona un inventario de 3 espacios de acceso rápido, se pueden ver con la tecla ` TAB ` . Permite recoger objetos del entorno (como armas y botiquines), alternar su selección usando la rueda del ratón y utilizar consumibles para curar al jugador mediante el clic izquierdo, actualizando la interfaz en tiempo real.

### InventoryManager.cs
`OnInventoryChanged?.Invoke():` Dispara un evento que avisa a la interfaz (UI) que debe redibujar los iconos, evitando que la UI tenga que revisar el inventario en cada frame.

```csharp
    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Cambio de slot con la rueda del ratón
        if (scroll > 0f)
        {
            selectedSlot++;
            if (selectedSlot >= slots.Length) selectedSlot = 0;
            EquipSelectedItem();
        }
        if (scroll < 0f)
        {
            selectedSlot--;
            if (selectedSlot < 0) selectedSlot = slots.Length - 1;
            EquipSelectedItem();
        }

        // Uso de consumibles
        if (Input.GetMouseButtonDown(0))
        {
            UseSelectedItem();
        }
    }

    public bool AddItem(ItemData itemData, int quantity)
    {
        // 1. Busca si el objeto ya existe para acumularlo (stack)
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null && slots[i].itemData == itemData)
            {
                slots[i].quantity += quantity;
                OnInventoryChanged?.Invoke();
                return true;
            }
        }

        // 2. Si no existe, busca un espacio vacío
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = new InventorySlot(itemData, quantity);
                OnInventoryChanged?.Invoke();
                return true;
            }
        }
        return false; // Inventario lleno
    }

    private void UseSelectedItem()
    {
        if (slots[selectedSlot] == null) return;

        ItemData item = slots[selectedSlot].itemData;

        // Verifica si es consumible y cura al jugador
        if (item.isConsumable)
        {
            HealthSystem player = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();
            if (player != null)
            {
                player.Heal(item.healAmount);
            }

            slots[selectedSlot].quantity--;
            if (slots[selectedSlot].quantity <= 0) slots[selectedSlot] = null;
            
            OnInventoryChanged?.Invoke(); // Actualiza la UI
        }
    }
    ```

### HotBarUIController.c 
Utiliza el sistema de consultas Query del UI Toolkit de Unity para encontrar los elementos de la interfaz por su nombre.+= UpdateUI y -= UpdateUI: Sigue el patrón observador. La interfaz "escucha" cuándo hay un cambio en el inventario para actualizar los iconos solo cuando es necesario, y se desconecta en OnDestroy para prevenir memory leaks osea fugas de memoria.  

```csharp
 private UIDocument document;

 private Image slot1;
 private Image slot2;
 private Image slot3;

 private void Start()
 {
     document = GetComponent<UIDocument>();

     VisualElement root = document.rootVisualElement;

     slot1 = root.Q<Image>("slot01");
     slot2 = root.Q<Image>("slot02");
     slot3 = root.Q<Image>("slot03");

     Debug.Log("slot01: " + slot1);
     Debug.Log("slot02: " + slot2);
     Debug.Log("slot03: " + slot3);

     InventoryManager.Instance.OnInventoryChanged += UpdateUI;

     UpdateUI();
 }

 private void OnDestroy()
 {
     if (InventoryManager.Instance != null)
         InventoryManager.Instance.OnInventoryChanged -= UpdateUI;
 }

 private void UpdateUI()
 {
     if (InventoryManager.Instance == null)
         return;

     InventorySlot[] slots =
         InventoryManager.Instance.GetSlots();

     slot1.image = null;
     slot2.image = null;
     slot3.image = null;

     if (slots.Length > 0 && slots[0] != null)
     {
         slot1.image = slots[0].itemData.icon.texture;
     }

     if (slots.Length > 1 && slots[1] != null)
     {
         slot2.image = slots[1].itemData.icon.texture;
     }

     if (slots.Length > 2 && slots[2] != null)
     {
         slot3.image = slots[2].itemData.icon.texture;
     }
 }

```

### InventorySlot.cs`
Es una clase pura (no hereda de MonoBehaviour), diseñada exclusivamente como una estructura de datos ágil. Actúa como un contenedor que guarda la referencia de qué objeto está ocupando el espacio (itemData) y la cantidad acumulada del mismo (quantity).

```csharp
public ItemData itemData;
public int quantity;
public InventorySlot(ItemData data, int qty)
{
    this.itemData = data;
    this.quantity = qty;
}
```

### InventoryController.cs
Este script se encarga de la experiencia visual y los controles de la interfaz usando UI Toolkit.
root.style.display: Oculta o muestra el inventario general al presionar la tecla Tab.

UnityCursor.lockState: Es fundamental en juegos 3D (First/Third Person), ya que libera el ratón cuando el inventario está abierto para que el jugador pueda interactuar con la UI, y lo vuelve a bloquear en el centro de la pantalla al cerrarlo.

AddToClassList / RemoveFromClassList: Modifica dinámicamente las clases de estilo (similares a CSS) para crear el efecto visual de "resaltado" sobre el slot que el jugador está seleccionando con la rueda del ratón, separando la lógica del diseño visual.

```csharp
 private VisualElement root;
 private bool inventoryOpen = false;

 // Slots visuales
 private VisualElement slot1;
 private VisualElement slot2;
 private VisualElement slot3;

 private int selectedSlot = 0;
 public int SelectedSlot => selectedSlot;

 private void Start()
 {
     root = GetComponent<UIDocument>().rootVisualElement;
     // Buscamos los contenedores de los slots
     slot1 = root.Q<VisualElement>("Slot1");
     slot2 = root.Q<VisualElement>("Slot2");
     slot3 = root.Q<VisualElement>("Slot3");

     // Resaltamos el primer slot
     UpdateSelectedVisual();

     root.style.display = DisplayStyle.None;

     UnityCursor.lockState = CursorLockMode.Locked;
     UnityCursor.visible = false;
 }

 private void UpdateSelectedVisual()
 {
     slot1.RemoveFromClassList("slotSelected");
     slot2.RemoveFromClassList("slotSelected");
     slot3.RemoveFromClassList("slotSelected");

     switch (selectedSlot)
     {
         case 0:
             slot1.AddToClassList("slotSelected");
             break;

         case 1:
             slot2.AddToClassList("slotSelected");
             break;

         case 2:
             slot3.AddToClassList("slotSelected");
             break;
     }
 }

 private void Update()
 {
     if (Input.GetKeyDown(KeyCode.Tab))
     {
         inventoryOpen = !inventoryOpen;

         root.style.display = inventoryOpen
             ? DisplayStyle.Flex
             : DisplayStyle.None;

         if (inventoryOpen)
         {
             UnityCursor.lockState = CursorLockMode.None;
             UnityCursor.visible = true;
         }
         else
         {
             UnityCursor.lockState = CursorLockMode.Locked;
             UnityCursor.visible = false;
         }
     }

     HandleScrollWheel();
 }

 private void HandleScrollWheel()
 {
     float scroll = Input.GetAxis("Mouse ScrollWheel");

     int maxSlots =
         InventoryManager.Instance.GetSlots().Length;

     if (scroll > 0f)
     {
         selectedSlot--;

         if (selectedSlot < 0)
             selectedSlot = maxSlots - 1;

         UpdateSelectedVisual();

         Debug.Log("Slot Seleccionado: " + selectedSlot);
     }

     if (scroll < 0f)
     {
         selectedSlot++;

         if (selectedSlot >= maxSlots)
             selectedSlot = 0;

         UpdateSelectedVisual();

         Debug.Log("Slot Seleccionado: " + selectedSlot);
     }
 }
```

### ItemData.cs
Al heredar de ScriptableObject permite crear y modificar diferentes tipos de armas o botiquines directamente desde el menú de Unity sin tocar el código.

```csharp
[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemID;
    public string itemName;
    public Sprite icon;
    public GameObject prefab;
    
    // Propiedades de Consumibles (Ej: Botiquín)
    public bool isConsumable;
    public int healAmount;
}
```

### PickAble.cs
Se conecta con el sistema de interacción general (implementando IInteractable). Al presionar la tecla de interacción, el objeto intenta recogerse y se destruye del mundo 3D.

```csharp
private bool pickedUp = false;

public ItemData ItemData => itemData;
public int Quantity => quantity;

public bool canInteract()
{
    return !pickedUp;
}

public void Interact()
{
    TryPickUp();
}

public bool TryPickUp()
{
    if (pickedUp || itemData == null)
        return false;

    pickedUp = true;

    if (destroyOnPickUp)
        Destroy(gameObject);

    return true;
}
```
### 5. Sistema de equipamiento
- **Encargado:** Tolaba Tiago
- **Scripts principales:** `EquipmentManager.cs` 
- **Funcionamiento:** Controla el uso de los items seleccionados para que les permitan atacar, disparar o consumir.

### EquipmentManager.cs
```csharp
 public static EquipmentManager Instance;

 public Transform handPoint;

 private GameObject currentObject;

 private void Awake()
 {
     Instance = this;
 }

 public void Equip(ItemData item)
 {
     if (currentObject != null)
     {
         Destroy(currentObject);
     }

     if (item.prefab != null)
     {
         currentObject = Instantiate(
             item.prefab,
             handPoint
         );

         currentObject.transform.localPosition = Vector3.zero;
         currentObject.transform.localRotation = Quaternion.identity;
     }
 }
```
### 6. Sistema de Raycast 
- **Encargado:** Tolaba Tiago
- **Scripts principales:** `GunController.cs` y `MeleeAttack.cs`
- **Funcionamiento:** Controla la mecánica de disparo del arma equipada (como la M4) utilizando la física de Raycast y el ataque a corta distancia con el cuchillo. 

### GunController.cs
Permite disparar un nuemro de 30 balas para luego poder recargar, cuenta con municion ilimitada
```csharp
public float damage = 15f;
public float range = 100f;
[SerializeField] private LayerMask enemyLayer;
public int maxAmmo = 30;
private int currentAmmo;

private Camera cam;

private void Start()
{
    currentAmmo = maxAmmo;
    cam = Camera.main;
}

private void Update()
{
    if (Input.GetMouseButtonDown(0))
    {
        Shoot();
    }

    if (Input.GetKeyDown(KeyCode.R))
    {
        Reload();
    }
}

private void Shoot()
{
    if (currentAmmo <= 0)
    {
        Debug.Log("Sin balas");
        return;
    }

    currentAmmo--;

    Debug.Log("Disparo. Balas: " + currentAmmo);

    Ray ray = cam.ScreenPointToRay(
        new Vector3(
            Screen.width / 2,
            Screen.height / 2
        )
    );

    if (Physics.Raycast(ray, out RaycastHit hit, range, enemyLayer))
    {
        Debug.Log("Impactó en " + hit.collider.name);

        HealthSystem health =
        hit.collider.GetComponent<HealthSystem>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }
}

private void Reload()
{
    currentAmmo = maxAmmo;

    Debug.Log("Recargando...");
}
```

### MeleeAttack.cs

```csharp
 public float damage = 10f;
 public float range = 2f;
 [SerializeField] private LayerMask enemyLayer;
 private Camera cam;

 private void Start()
 {
     cam = Camera.main;
 }

 private void Update()
 {
     if (Input.GetMouseButtonDown(0))
     {
         Attack();
     }
 }

 private void Attack()
 {
     Debug.Log("Cuchillazo");

     Ray ray = cam.ScreenPointToRay(
         new Vector3(
             Screen.width / 2,
             Screen.height / 2
         )
     );

     if (Physics.Raycast(ray, out RaycastHit hit, range, enemyLayer))
     {
         HealthSystem health =
         hit.collider.GetComponent<HealthSystem>();

         if (health != null)
         {
             health.TakeDamage(damage);
         }
     }
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
