using UnityEngine;



public class HingePendulumController : MonoBehaviour
{
    private HingeJoint2D hinge;

    [Header("Configuración del Motor")]
    [SerializeField] private float motorPower = 120f;     // Velocidad del vaivén
    [SerializeField] private float maxTorque = 10000f;    // Torsión máxima para mover la plataforma
    [SerializeField] private float frequency = 1.2f;      // Ritmo del balanceo

    void Start()
    {
        hinge = GetComponent<HingeJoint2D>();

        if (hinge != null)
        {
            hinge.useMotor = true;
        }
    }

    void Update()
    {
        if (hinge == null) return;

        JointMotor2D motor = hinge.motor;
        motor.motorSpeed = Mathf.Sin(Time.time * frequency) * motorPower;
        motor.maxMotorTorque = maxTorque; // Usamos Torque para el HingeJoint2D
        hinge.motor = motor;
    }
}