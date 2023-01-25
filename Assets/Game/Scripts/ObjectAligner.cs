using UnityEngine;

public class ObjectAligner : MonoBehaviour
{
    public float sensitivity = 0.1f;
    private Quaternion initialRotation;
    private Vector3 initialAcceleration;

    void Start()
    {
        // Record the initial rotation of the object
        initialRotation = transform.rotation;
        // Record the initial acceleration reading from the device's accelerometer
        initialAcceleration = Input.acceleration;
        // Align the initial acceleration with the floor
        initialAcceleration.y = 0;
        initialAcceleration = initialAcceleration.normalized;
    }

    void Update()
    {
        // Get the current acceleration reading from the device's accelerometer
        Vector3 acceleration = Input.acceleration;
        // Align the acceleration with the floor
        acceleration.x = 0;
        acceleration = acceleration.normalized;
        // Calculate the rotation needed to align the object with the floor
        Quaternion deltaRotation = Quaternion.FromToRotation(initialAcceleration, acceleration);
        // Apply the rotation to the object
        transform.rotation = initialRotation * deltaRotation;
    }
}
