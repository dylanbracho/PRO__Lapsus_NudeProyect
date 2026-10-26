using UnityEngine;

public class PendulumMove : MonoBehaviour
{
    [SerializeField] private float maxAngle = 25f; // Qué tanto se inclina a los lados (amplitud)
    [SerializeField] private float speed = 2f;     // Qué tan rápido se balancea
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime * speed;

        // Rota suavemente de un lado a otro creando la curva exacta de un péndulo
        float currentAngle = Mathf.Sin(timer) * maxAngle;
        transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
    }
}