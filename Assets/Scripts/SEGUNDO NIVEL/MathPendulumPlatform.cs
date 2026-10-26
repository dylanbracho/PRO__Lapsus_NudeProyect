using UnityEngine;

public class MathPendulumPlatform : MonoBehaviour
{
    [SerializeField] private Transform pivotPoint;   // El punto fijo de arriba (el "techo" o anclaje)
    [SerializeField] private float ropeLength = 4f;  // La longitud de la cuerda (radio del arco)
    [SerializeField] private float maxAngle = 35f;   // Qué tan lejos llega a los lados (amplitud del arco)
    [SerializeField] private float speed = 2f;       // Velocidad del recorrido

    private float timer = 0f;
    private Vector2 pivotPosition;

    void Start()
    {
        // Si pusiste un objeto arriba como referencia, toma su posición. Si no, calcúlala hacia arriba.
        if (pivotPoint != null)
        {
            pivotPosition = pivotPoint.position;
        }
        else
        {
            pivotPosition = (Vector2)transform.position + new Vector2(0f, ropeLength);
        }
    }

    void Update()
    {
        // Avanzamos el tiempo para movernos de un lado a otro suavemente
        timer += Time.deltaTime * speed;

        // Mathf.Sin hace que oscile de forma cíclica (va y vuelve por el mismo camino)
        float currentAngle = Mathf.Sin(timer) * maxAngle;

        // Convertimos los grados a radianes para las matemáticas de Unity
        float rad = currentAngle * Mathf.Deg2Rad;

        // FÓRMULA MATEMÁTICA EXACTA EN X Y Y AL MISMO TIEMPO:
        float x = pivotPosition.x + ropeLength * Mathf.Sin(rad);
        float y = pivotPosition.y - ropeLength * Mathf.Cos(rad);

        // Actualizamos la posición exacta de la plataforma combinando X y Y
        transform.position = new Vector2(x, y);
    }
}