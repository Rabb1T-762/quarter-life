using UnityEngine;

namespace _Project.Scripts
{
    public class SourceMovement
    {
        private float friction;

        private float groundAccelerate;

        private float maxVelocityGround;

        private float airAccelerate;

        private float maxVelocityAir;
        
        // accelDir: normalized direction that the player has requested to move
        // (taking into account the movement keys and look direction)
        // prevVelocity: The current velocity of the player, before any additional calculations
        // accelerate: The server-defined player acceleration value
        // maxVelocity: The server-defined maximum player velocity (this is not strictly adhered to due to strafe-jumping)
        private Vector3 Accelerate(Vector3 accelDir, Vector3 prevVelocity, float accelerate, float maxVelocity)
        {   
            // Vector projection of Current velocity onto accelDir.
            float projVel = Vector3.Dot(prevVelocity, accelDir); 
            
            // Accelerated velocity in direction of movement
            float accelVel = accelerate * Time.deltaTime; 

            // If necessary, truncate the accelerated velocity
            // so the vector projection does not exceed max_velocity
            if (projVel + accelVel > maxVelocity)
                accelVel = maxVelocity - projVel;

            return prevVelocity + accelDir * accelVel;
        }

        private Vector3 MoveGround(Vector3 accelDir, Vector3 prevVelocity)
        {
            // Apply Friction
            float speed = prevVelocity.magnitude;
            if (speed != 0) // To avoid divide by zero errors
            {
                float drop = speed * friction * Time.fixedDeltaTime;
                prevVelocity *= Mathf.Max(speed - drop, 0) / speed; // Scale the velocity based on friction.
            }

            // groundAccelerate and maxVelocityGround are server-defined movement variables
            return Accelerate(accelDir, prevVelocity, groundAccelerate, maxVelocityGround);
        }

        private Vector3 MoveAir(Vector3 accelDir, Vector3 prevVelocity)
        {
            // airAccelerate and maxVelocityAir are server-defined movement variables
            return Accelerate(accelDir, prevVelocity, airAccelerate, maxVelocityAir);
        }
    }
}