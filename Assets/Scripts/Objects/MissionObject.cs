using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class MissionObject : MonoBehaviour
{
    [Header("Mission Object")]
    [SerializeField] private int points; // Puntos que se suman al recoger
    public static event Action OnMissionObjectCollected; // Evento que avisa cuando se recoge un objeto de mision

    private bool isCollected; // Comprueba si ya fue recogido

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
}
