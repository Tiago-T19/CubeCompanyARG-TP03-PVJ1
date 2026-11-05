using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class MissionObject : MonoBehaviour
{
    public static event Action OnMissionObjectCollected; // Evento que avisa cuando se recoge un objeto de mision

    private bool isCollected; // Comprueba si ya fue recogido

    public void CollectMissionObject()
    {
        if (isCollected) return;

        isCollected = true; // Marcamos el objeto como recogido

        OnMissionObjectCollected?.Invoke(); // Avisamos que se recogio un objeto

        Destroy(gameObject); // Eliminamos el objeto del escenario
    }
}
