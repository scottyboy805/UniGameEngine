using System;
using Jitter2;
using Microsoft.Xna.Framework;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Quaternion = Microsoft.Xna.Framework.Quaternion;
using Jitter2.Parallelization;
using JitterBody = Jitter2.Dynamics.RigidBody;
using System.Runtime.CompilerServices;
using Jitter2.LinearMath;
using Jitter2.DataStructures;
using Jitter2.Collision;
using Jitter2.Collision.Shapes;
using System.Collections.Generic;
using UniGameEngine.Scene;

namespace UniGameEngine.Physics
{
    public struct PhysicsHit
    {
        // Public
        public Collider Collider;
        public RigidBody Body;        
        public Vector3 Normal;
        public float Distance;
    }

    public sealed class PhysicsSimulation : IDisposable
    {
        // Private        
        private UniGame game = null;
        private GameSettings gameSettings = null;
        private int threadCount = 1;
        private float fixedStepTimer = 0f;

        // Internal
        internal World physicsWorld = null;
        internal Dictionary<RigidBodyShape, Collider> activeColliders = new Dictionary<RigidBodyShape, Collider>();

        // Properties
        public int Priority => -100;

        // Constructor
        internal PhysicsSimulation(UniGame game)
        {
            this.game = game;
            this.gameSettings = game.GameSettings;

            // Get thread count
            threadCount = Math.Max(1, Environment.ProcessorCount > 4
                ? Environment.ProcessorCount - 2
                : Environment.ProcessorCount - 1);

            World.Capacity worldCapacity = new World.Capacity
            { 
                BodyCount = 64000,
                ContactCount = 64000,
                ConstraintCount = 32000,
                SmallConstraintCount = 32000,
            };

            // Create the world
            physicsWorld = new World(worldCapacity);

            // Setup gravity
            Vector3 gravity = gameSettings.Gravity;
            physicsWorld.Gravity = Unsafe.As<Vector3, JVector>(ref gravity);

            // Setup iterations
            physicsWorld.SubstepCount = 2;
            physicsWorld.SolverIterations = (8, 4);

            // Update thread pool
            ThreadPool.Instance.ChangeThreadCount(threadCount);                       
        }

        // Methods
        public bool Raycast(Vector3 origin, Vector3 direction, out PhysicsHit hit)
        {
            // Get raycast structures
            JVector pos = Unsafe.As<Vector3, JVector>(ref origin);
            JVector dir = Unsafe.As<Vector3, JVector>(ref direction);

            // Reset the hit
            hit = default;

            IDynamicTreeProxy hitShape;
            JVector hitNormal;
            float distance;

            // Perform raycast
            bool result = physicsWorld.DynamicTree.RayCast(pos, dir, null, null,
                out hitShape, out hitNormal, out distance);

            // Check for success
            if(result == true)
            {
                // Get the body
                RigidBodyShape hitBodyShape = (RigidBodyShape)hitShape;

                // Update hit data
                hit.Collider = GetCollider(hitBodyShape);
                hit.Body = hitBodyShape.RigidBody.Tag as RigidBody;                
                hit.Normal = Unsafe.As<JVector, Vector3>(ref hitNormal);
                hit.Distance = distance;
            }
            return result;
        }

        internal Collider GetCollider(RigidBodyShape shape)
        {
            Collider result;
            activeColliders.TryGetValue(shape, out result);
            return result;
        }

        public void Step(GameTime gameTime)
        {
            fixedStepTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            float fixedStep = 1f / 100f;

            // Update fixed time
            while (fixedStepTimer >= fixedStep)
            {
                // Update physics world
                physicsWorld.Step(fixedStep, true);
                fixedStepTimer -= fixedStep;

                // Sync after update
                SyncDynamicBodies();

                // Run physics update for behaviour scripts
                PerformPhysicsUpdate(gameTime, fixedStep);
            }            
        }

        void IDisposable.Dispose()
        {
            physicsWorld.Clear();
            physicsWorld = null;
        }

        private void SyncDynamicBodies()
        {
            // Process all bodies
            ReadOnlyActiveList<JitterBody> activeBodies = physicsWorld.RigidBodies;

            // Update only active bodies
            for(int i = 0; i < activeBodies.Active; i++)
            {
                // Get the body
                JitterBody body = activeBodies[i];

                // Check for body associated
                if(body.Tag is RigidBody rigidBody)
                {
                    SyncTransform(body, rigidBody.Transform);
                    SyncMomentum(body, rigidBody);
                }
            }
        }

        private void PerformPhysicsUpdate(GameTime gameTime, float fixedStep)
        {
            // Run fixed update
            foreach (GameScene scene in game.scenes)
            {
                // Check for enabled
                if (scene.Enabled == false)
                    continue;

                foreach (IGameFixedUpdate fixedUpdate in scene.sceneFixedUpdateCalls)
                {
                    try
                    {
                        fixedUpdate.OnFixedUpdate(gameTime, fixedStep);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }
        }

        internal static void ApplyTransform(JitterBody body, Transform transform)
        {
            // Apply position and rotation
            ApplyTransformPosition(body, transform.WorldPosition);
            ApplyTransformRotation(body, transform.WorldRotation);
        }

        internal static void ApplyTransformPosition(JitterBody body, Vector3 position)
        {
            body.Position = Unsafe.As<Vector3, JVector>(ref position);
        }

        internal static void ApplyTransformRotation(JitterBody body, Quaternion rotation)
        {
            body.Orientation = Unsafe.As<Quaternion, JQuaternion>(ref rotation);
        }

        internal static void ApplyMomentum(JitterBody body, RigidBody rigidBody)
        {
            ApplyMomentumVelocity(body, rigidBody.Velocity);
            ApplyMomentumAngularVelocity(body, rigidBody.AngularVelocity);
        }

        internal static void ApplyMomentumVelocity(JitterBody body, Vector3 velocity)
        {
            body.Velocity = Unsafe.As<Vector3, JVector>(ref velocity);
        }

        internal static void ApplyMomentumAngularVelocity(JitterBody body, Vector3 angularVelocity)
        {
            body.AngularVelocity = Unsafe.As<Vector3, JVector>(ref angularVelocity);
        }

        internal static void SyncTransform(JitterBody body, Transform transform)
        {
            transform.WorldPosition = SyncTransformPosition(body);
            transform.WorldRotation = SyncTransformRotation(body);
        }

        internal static Vector3 SyncTransformPosition(JitterBody body)
        {
            JVector position = body.Position;
            return Unsafe.As<JVector, Vector3>(ref position);
        }

        internal static Quaternion SyncTransformRotation(JitterBody body)
        {
            JQuaternion rotation = body.Orientation;
            return Unsafe.As<JQuaternion, Quaternion>(ref rotation);
        }

        internal static void SyncMomentum(JitterBody body, RigidBody rigidBody)
        {
            // Velocity
            JVector velocity = body.Velocity;
            rigidBody.velocity = Unsafe.As<JVector, Vector3>(ref velocity);

            // Angular velocity
            JVector angularVelocity = body.AngularVelocity;
            rigidBody.angularVelocity = Unsafe.As<JVector, Vector3>(ref angularVelocity);
        }
    }
}
