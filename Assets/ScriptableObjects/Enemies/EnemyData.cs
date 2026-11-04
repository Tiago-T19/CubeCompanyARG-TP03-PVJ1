using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "TP3/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string nombre = "Enemigo";
    [Header("Vida y Puntos")]
    public float vidaMax = 100f;
    public int puntosAlMorir = 10;
    public int danio = 15;
    [Header("IA")]
    public float velocidad = 3.5f;
    public float detectionRadius = 8f;
    public float attackRadius = 2f;
    public float attackCooldown = 1.5f;
}
