using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace UniGameEngine.Physics
{
    [DataContract]
    public sealed class VehicleController : Component, IGameFixedUpdate
    {
        // Type
        [DataContract]
        private sealed class VehicleWheel
        {
            // Public
            [DataMember]
            public WheelCollider Collider;
            [DataMember]
            public bool IsDriven;
            [DataMember]
            public bool IsSteered;
            [DataMember]
            public float SteeringMultiplier = 1f;
        }

        // Private
        [DataMember(Name = "Wheels")]
        private List<VehicleWheel> wheels = new List<VehicleWheel>
        {
            new VehicleWheel { IsSteered = true },      // FL
            new VehicleWheel { IsSteered = true },      // FR
            new VehicleWheel { IsDriven = true },       // RL
            new VehicleWheel { IsDriven = true },       // RR
        };
        [DataMember]
        private float maxSteerAngle = 35f;

        private float steerAngle = 0f;

        // Properties
        public IEnumerable<WheelCollider> Wheels
        {
            get { return wheels.Select(w => w.Collider); }
        }

        public int NumberOfWheels
        {
            get { return wheels.Count; }
        }

        public float MaxSteerAngle
        {
            get { return maxSteerAngle; }
            set
            {
                maxSteerAngle = value;

                // Check for steering out of bounds
                if (steerAngle > maxSteerAngle)
                {
                    SteerAngle = maxSteerAngle;
                }
                else if (steerAngle < -maxSteerAngle)
                {
                    SteerAngle = -maxSteerAngle;
                }
            }
        }

        public float SteerAngle
        {
            get { return steerAngle; }
            set
            {
                steerAngle = value;

                // Update all wheels
                foreach(VehicleWheel wheel in wheels)
                {
                    // Update steering taking multiplier into account
                    if(wheel != null && wheel.IsSteered == true)
                        wheel.Collider.SteerAngle = steerAngle * wheel.SteeringMultiplier;
                }
            }
        }

        // Methods
        public void OnFixedUpdate(GameTime gameTime, float fixedStep)
        {

        }

        public void AddTorque(float torque)
        {
            // Update all wheels
            foreach(VehicleWheel wheel in wheels)
            {
                // Update torque of the wheel
                if (wheel != null && wheel.IsDriven == true)
                    wheel.Collider.AddTorque(torque);
            }
        }

        public void AddWheel(WheelCollider wheel, bool isDriven, bool isSteered, float steeringMultiplier = 1f)
        {
            wheels.Add(new VehicleWheel
            {
                Collider = wheel,
                IsDriven = isDriven,
                IsSteered = isSteered,
                SteeringMultiplier = steeringMultiplier
            });
        }
    }
}
