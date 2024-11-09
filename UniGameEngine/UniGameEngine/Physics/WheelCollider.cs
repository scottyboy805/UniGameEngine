using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace UniGameEngine.Physics
{
    [DataContract]
    public sealed class WheelCollider : Component
    {
        // Private
        private float displacement = 0f, lastDisplacement = 0f;
        private float upSpeed = 0f;
        private bool grounded = false;
        private float driveTorque = 0f;
        private float angularVelocity = 0f;
        private float angularVelocityForGrip = 0f;
        private float torque = 0f;

        // Internal
        internal RigidBody attachedBody = null;

        // Properties
        public PhysicsSimulation Physics
        {
            get { return Game.Physics; }
        }

        public RigidBody Body
        {
            get
            {
                if (attachedBody != null)
                    return attachedBody;

                return GameObject.GetComponentInParent<RigidBody>();
            }
        }

        public bool IsGrounded => grounded;
        public float WheelRotation { get; private set; } = 0f;

        [DataMember]
        public bool IsLocked { get; set; }
        [DataMember]
        public float SteerAngle { get; set; }
        [DataMember]
        public float Damping { get; set; } = 1500;
        [DataMember]
        public float Spring { get; set; } = 35000;
        [DataMember]
        public float Inertia { get; set; } = 5;
        [DataMember]
        public float Radius { get; set; } = 0.5f;
        [DataMember]
        public float SideFriction { get; set; } = 3.2f;
        [DataMember]
        public float ForwardFriction { get; set; } = 5.0f;
        [DataMember]
        public float SuspensionTravel { get; set; } = 0.2f;
        [DataMember]
        public float MaximumAngularVelocity { get; set; } = 200f;
        [DataMember]
        public int NumberOfRays { get; set; } = 1;

        // Methods
        protected override void OnEnable()
        {
            attachedBody = GameObject.GetComponentInParent<RigidBody>(true);
        }

        public void AddTorque(float torque)
        {
            driveTorque += torque;
        }

        public void PostStep(float timeStep)
        {
            // Check for no update
            if (timeStep <= 0.0f || attachedBody == null || attachedBody.Enabled == false)
                return;

            float origAngVel = angularVelocity;
            upSpeed = (displacement - lastDisplacement) / timeStep;

            // Check for wheel locked in place
            if (IsLocked == true)
            {
                angularVelocity = 0;
                torque = 0;
            }
            else
            {
                angularVelocity += torque * timeStep / Inertia;
                torque = 0;

                // Don't apply much torque if not grounded
                if (grounded == false) 
                    driveTorque *= 0.1f;

                // Prevent friction from reversing dir - todo do this better by limiting the torque
                if ((origAngVel > angularVelocityForGrip && angularVelocity < angularVelocityForGrip) ||
                    (origAngVel < angularVelocityForGrip && angularVelocity > angularVelocityForGrip))
                    angularVelocity = angularVelocityForGrip;

                angularVelocity += driveTorque * timeStep / Inertia;
                driveTorque = 0;

                // Update rotation velocity
                float maxAngVel = MaximumAngularVelocity;
                angularVelocity = Math.Clamp(angularVelocity, -maxAngVel, maxAngVel);

                // Update rotation value
                WheelRotation += timeStep * angularVelocity;
            }
        }

        public void PreStep(float timeStep)
        {
            // Check for no update
            if (attachedBody == null || attachedBody.Enabled == false)
                return;

            // var dr = Playground.Instance.DebugRenderer;

            Vector3 force = Vector3.Zero;
            //JVector force = JVector.Zero;
            lastDisplacement = displacement;
            displacement = 0.0f;

            float vel = attachedBody.Velocity.Length();

            Vector3 worldPos = Transform.WorldPosition;
            Vector3 worldAxis = attachedBody.Transform.Up;

            //JVector worldPos = car.Position + JVector.Transform(Position, car.Orientation);
            //JVector worldAxis = JVector.Transform(Up, car.Orientation);

            Vector3 forward = attachedBody.Transform.Forward;
            Vector3 wheelFwd = (Transform.localToWorldMatrix * Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(worldAxis, SteerAngle))).Forward;

            //JVector forward = JVector.Transform(-JVector.UnitZ, car.Orientation); //-car.Orientation.GetColumn(2);
            //JVector wheelFwd = JVector.Transform(forward, JMatrix.CreateRotationMatrix(worldAxis, SteerAngle));

            Vector3 wheelLeft = Vector3.Cross(worldAxis, wheelFwd);
            wheelLeft.Normalize();

            //JVector wheelLeft = JVector.Cross(worldAxis, wheelFwd);
            //wheelLeft.Normalize();

            Vector3 wheelUp = Vector3.Cross(wheelFwd, wheelLeft);

            //JVector wheelUp = JVector.Cross(wheelFwd, wheelLeft);

            float rayLen = 2.0f * Radius + SuspensionTravel;

            Vector3 wheelRayEnd = worldPos - Radius * worldAxis;
            Vector3 wheelRayOrigin = wheelRayEnd + rayLen * worldAxis;
            Vector3 wheelRayDelta = wheelRayEnd - wheelRayOrigin;

            //JVector wheelRayEnd = worldPos - Radius * worldAxis;
            //JVector wheelRayOrigin = wheelRayEnd + rayLen * worldAxis;
            //JVector wheelRayDelta = wheelRayEnd - wheelRayOrigin;

            float deltaFwd = 2.0f * Radius / (NumberOfRays + 1);
            float deltaFwdStart = deltaFwd;

            grounded = false;

            Vector3 groundNormal = Vector3.Zero;
            Vector3 groundPos = Vector3.Zero;

            //JVector groundNormal = JVector.Zero;
            //JVector groundPos = JVector.Zero;
            float deepestFrac = float.MaxValue;
            Collider worldBody = null;
            //RigidBody worldBody = null!;

            for (int i = 0; i < NumberOfRays; i++)
            {
                float distFwd = deltaFwdStart + i * deltaFwd - Radius;
                float zOffset = Radius * (1.0f - (float)Math.Cos(Math.PI / 2.0f * (distFwd / Radius)));
                Vector3 newOrigin = wheelRayOrigin + distFwd * wheelFwd + zOffset * wheelUp;
                //JVector newOrigin = wheelRayOrigin + distFwd * wheelFwd + zOffset * wheelUp;

                PhysicsHit hit;
                bool result = Physics.Raycast(newOrigin, wheelRayDelta, out hit);

                //RigidBody body;

                //bool result = world.DynamicTree.RayCast(newOrigin, wheelRayDelta,
                //    rayCast, null, out IDynamicTreeProxy? shape, out JVector normal, out float frac);

                // Debug Rendering
                // dr.PushPoint(DebugRenderer.Color.Green, Conversion.FromJitter(newOrigin), 0.2f);
                // dr.PushPoint(DebugRenderer.Color.Red, Conversion.FromJitter(newOrigin + wheelRayDelta), 0.2f);

                Vector3 minBox = worldPos - new Vector3(Radius);
                Vector3 maxBox = worldPos + new Vector3(Radius);

                //JVector minBox = worldPos - new JVector(Radius);
                //JVector maxBox = worldPos + new JVector(Radius);

                // dr.PushBox(DebugRenderer.Color.Green, Conversion.FromJitter(minBox), Conversion.FromJitter(maxBox));

                if (result && hit.Distance <= 1.0f)
                {
                    // shape must be RigidBodyShape since we filter out other ray tests
                    //body = (shape as RigidBodyShape)!.RigidBody;

                    if (hit.Distance < deepestFrac)
                    {
                        deepestFrac = hit.Distance;
                        groundPos = newOrigin + hit.Distance * wheelRayDelta;
                        worldBody = hit.Collider;
                        groundNormal = hit.Normal;
                    }

                    grounded = true;
                }
            }

            if (!grounded) return;

            // dr.PushPoint(DebugRenderer.Color.Green, Conversion.FromJitter(groundPos), 0.2f);

            if (groundNormal.LengthSquared() > 0.0f) groundNormal.Normalize();

            // System.Diagnostics.Debug.WriteLine(groundPos.ToString());

            displacement = rayLen * (1.0f - deepestFrac);
            displacement = Math.Clamp(displacement, 0.0f, SuspensionTravel);

            float displacementForceMag = displacement * Spring;

            // reduce force when suspension is par to ground
            displacementForceMag *= Vector3.Dot(groundNormal, worldAxis);
            //displacementForceMag *= JVector.Dot(groundNormal, worldAxis);

            // apply damping
            float dampingForceMag = upSpeed * Damping;

            float totalForceMag = displacementForceMag + dampingForceMag;

            if (totalForceMag < 0.0f) totalForceMag = 0.0f;

            Vector3 extraForce = totalForceMag * worldAxis;
            //JVector extraForce = totalForceMag * worldAxis;

            force += extraForce;

            // side-slip friction and drive force. Work out wheel- and floor-relative coordinate frame

            Vector3 groundUp = groundNormal;
            Vector3 groundLeft = Vector3.Cross(groundNormal, wheelFwd);

            //JVector groundUp = groundNormal;
            //JVector groundLeft = JVector.Cross(groundNormal, wheelFwd);
            if (groundLeft.LengthSquared() > 0.0f) groundLeft.Normalize();

            Vector3 groundFwd = Vector3.Cross(groundLeft, groundUp);
            //JVector groundFwd = JVector.Cross(groundLeft, groundUp);

            Vector3 wheelPointVel = attachedBody.Velocity + Vector3.Cross(attachedBody.AngularVelocity, worldPos);  // ????????????
                                                                                                                    //JVector wheelPointVel = car.Velocity +
                                                                                                                    //JVector.Cross(car.AngularVelocity, JVector.Transform(Position, car.Orientation));

            // rimVel=(wxr)*v
            Vector3 rimVel = angularVelocity * Vector3.Cross(wheelLeft, groundPos - worldPos);
            //JVector rimVel = angularVelocity * JVector.Cross(wheelLeft, groundPos - worldPos);
            wheelPointVel += rimVel;

            if (worldBody == null) throw new Exception("car: world body is null.");

            //JVector worldVel = worldBody.Velocity +
            //                   JVector.Cross(worldBody.AngularVelocity, groundPos - worldBody.Position);

            //wheelPointVel -= worldVel;

            // sideways forces
            float noslipVel = 0.2f;
            float slipVel = 0.4f;
            float slipFactor = 0.7f;

            float smallVel = 3.0f;
            float friction = SideFriction;

            float sideVel = Vector3.Dot(wheelPointVel, groundLeft);
            //float sideVel = JVector.Dot(wheelPointVel, groundLeft);

            if (sideVel > slipVel || sideVel < -slipVel)
            {
                friction *= slipFactor;
            }
            else if (sideVel > noslipVel || sideVel < -noslipVel)
            {
                friction *= 1.0f - (1.0f - slipFactor) * (Math.Abs(sideVel) - noslipVel) / (slipVel - noslipVel);
            }

            if (sideVel < 0.0f)
                friction *= -1.0f;

            if (Math.Abs(sideVel) < smallVel)
                friction *= Math.Abs(sideVel) / smallVel;

            float sideForce = -friction * totalForceMag;

            extraForce = sideForce * groundLeft;
            force += extraForce;

            // fwd/back forces
            friction = ForwardFriction;
            float fwdVel = Vector3.Dot(wheelPointVel, groundFwd);
            //float fwdVel = JVector.Dot(wheelPointVel, groundFwd);

            if (fwdVel > slipVel || fwdVel < -slipVel)
            {
                friction *= slipFactor;
            }
            else if (fwdVel > noslipVel || fwdVel < -noslipVel)
            {
                friction *= 1.0f - (1.0f - slipFactor) * (Math.Abs(fwdVel) - noslipVel) / (slipVel - noslipVel);
            }

            if (fwdVel < 0.0f)
                friction *= -1.0f;

            if (Math.Abs(fwdVel) < smallVel)
                friction *= Math.Abs(fwdVel) / smallVel;

            float fwdForce = -friction * totalForceMag;

            extraForce = fwdForce * groundFwd;
            force += extraForce;

            // fwd force also spins the wheel
            Vector3 wheelCentreVel = attachedBody.Velocity + Vector3.Cross(attachedBody.AngularVelocity, worldPos);
            //JVector wheelCentreVel = car.Velocity +
            //                         JVector.Cross(car.AngularVelocity, JVector.Transform(Position, car.Orientation));

            angularVelocityForGrip = Vector3.Dot(wheelCentreVel, groundFwd) / Radius;
            //angularVelocityForGrip = JVector.Dot(wheelCentreVel, groundFwd) / Radius;
            torque += -fwdForce * Radius;

            // add force to car
            attachedBody.AddForce(force, groundPos);
            //car.AddForce(force, groundPos);

            //RenderWindow.Instance.DebugRenderer.PushPoint(DebugRenderer.Color.White, Conversion.FromJitter(groundPos), 0.2f);

            // add force to the world
            //if (!worldBody.IsStatic)
            //{
            //    const float maxOtherBodyAcc = 500.0f;
            //    float maxOtherBodyForce = maxOtherBodyAcc * worldBody.Mass;

            //    if (force.LengthSquared() > (maxOtherBodyForce * maxOtherBodyForce))
            //        force *= maxOtherBodyForce / force.Length();

            //    worldBody.SetActivationState(true);

            //    worldBody.AddForce(force * -1, groundPos);
            //}
        }
    }
}
