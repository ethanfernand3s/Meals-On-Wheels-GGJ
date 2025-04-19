using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CarWheelTransforms
{
    public Transform wheelTransform;
    public bool CanSteer;
    public Transform gfxTransform;
}
public class CarPhysics : MonoBehaviour
{
    public CarWheelTransforms[] carWheelTransforms = new CarWheelTransforms[4];
    
        public float restLength = 0.5f;
        public float springStrength = 20f;
        public float damperStrength = 4f;
        public float wheelRadius = .5f;
        public float topCarSpeed = 300;
        public float carSpeedForce = 400;
        
        private float steerInput;
        private float accelInput;
        private Rigidbody carRigidBody;
        public AnimationCurve gripFactorCurve;
        public AnimationCurve torqueFactorCurve;
    
      private void Start()
      {
          carRigidBody = GetComponent<Rigidbody>();
      }
    private void FixedUpdate()
    {
        steerInput = Input.GetAxis("Horizontal");
        accelInput = Input.GetAxis("Vertical");
        foreach(var wheelTransform in carWheelTransforms)
        {
            CalculateTireForces(wheelTransform);
            if (wheelTransform.CanSteer) {
                // Rotate the wheel's transform around its Y-axis by steerAngle.
                Vector3 currentEuler = wheelTransform.wheelTransform.localEulerAngles;
                currentEuler.y = steerInput * 27f; // Y-axis only
                wheelTransform.wheelTransform.localEulerAngles = currentEuler;
            }
        }

        Debug.Log(steerInput);
        
        Debug.Log(accelInput);

    }

    private void CalculateTireForces(CarWheelTransforms wheelTransforms)
    {
        Transform wheelTransform = wheelTransforms.wheelTransform;
        Transform gfxTransform = wheelTransforms.gfxTransform;
        bool bCanSteer = wheelTransforms.CanSteer;
        
        Vector3 down = wheelTransform.TransformDirection(Vector3.down);
        Ray ray = new Ray(wheelTransform.position, -wheelTransform.up);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        // Use the interactLayer in the Raycast
        if (Physics.Raycast(ray, out RaycastHit hit, restLength + wheelRadius))
        {
            // ----------Suspension------------
            
            //world-space direction of spring force;
            Vector3 springDir = wheelTransform.up;

            //world-space velocity of this tire
            Vector3 tireWorldVel = carRigidBody.GetPointVelocity(wheelTransform.position);

            //calculate offset from the raycast ETHAN! WAS HERE
            float offset = restLength - hit.distance;

            //calculate THE velocity along the spring direction
            float vel = Vector3.Dot(springDir, tireWorldVel);

            //calculate the force
            float force = (offset * springStrength) - (vel * damperStrength);

            //apply force to wheel Y (this is suspension)
            carRigidBody.AddForceAtPosition(springDir * force, wheelTransform.position);
            gfxTransform.position = wheelTransform.position + (down * (hit.distance - wheelRadius));
        // ----------Suspension------------
        
        // ----------Steering------------
        //world direction of spring force
        Vector3 steeringDir = wheelTransform.right ;

       
        //get the tire velocity in the steering direction
        float steeringVel = Vector3.Dot(steeringDir, tireWorldVel);
        
        //the change in velocity  is the steering val * grip 0 = no grip 1 = full grip
        float desiredVelChange = -steeringVel *gripFactorCurve.Evaluate(Mathf.Abs(steeringVel));

       
        //turn change in velocity in a acceleration 
        float desiredAccel = desiredVelChange / Time.deltaTime;
        //Steering force
        carRigidBody.AddForceAtPosition(steeringDir * desiredAccel,wheelTransform.position);
        
        // ----------Steering------------
        
        // ----------Acceleration------------

        //world space direction of Accel/braking
        Vector3 accelDir = wheelTransform.forward;
        
        //get the forward speed of the car  relative to the car
        float carSpeed = Vector3.Dot(wheelTransform.forward, carRigidBody.linearVelocity);
        
        //normailze 
        float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed / topCarSpeed));
        
        // allowed torque
        float availableTorque = torqueFactorCurve.Evaluate(normalizedSpeed) * accelInput * carSpeedForce;
        
        carRigidBody.AddForceAtPosition(accelDir * availableTorque, wheelTransform.position);
        gfxTransform.Rotate(Vector3.up, carSpeed);
        }
        else
        {
           
            gfxTransform.position = wheelTransform.position + (down *  restLength );
            
        }
        
       
    }
}
