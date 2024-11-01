using System;
using System.Collections.Generic;
using System.Numerics;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
using BepuPhysics.Constraints;
using BepuUtilities;
using BepuUtilities.Memory;
using Microsoft.Xna.Framework;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Quaternion = Microsoft.Xna.Framework.Quaternion;

namespace UniGameEngine.Physics
{
    public unsafe sealed class PhysicsSimulation : IGameUpdate, IDisposable
    {
        // Type
        private struct NarrowPhaseCallbacks : INarrowPhaseCallbacks
        {
            // Methods
            public bool AllowContactGeneration(int workerIndex, CollidableReference a, CollidableReference b, ref float speculativeMargin)
            {
                return a.Mobility == CollidableMobility.Dynamic || b.Mobility == CollidableMobility.Dynamic;
            }

            public bool AllowContactGeneration(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB)
            {
                return true;
            }

            public bool ConfigureContactManifold<TManifold>(int workerIndex, CollidablePair pair, ref TManifold manifold, out PairMaterialProperties pairMaterial) where TManifold : unmanaged, IContactManifold<TManifold>
            {
                pairMaterial.FrictionCoefficient = 1f;
                pairMaterial.MaximumRecoveryVelocity = 2f;
                pairMaterial.SpringSettings = new SpringSettings(30, 1);
                return true;
            }

            public bool ConfigureContactManifold(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB, ref ConvexContactManifold manifold)
            {
                return true;
            }

            public void Initialize(Simulation simulation)
            {
            }

            public void Dispose()
            {
            }
        }

        private struct PoseIntegratorCallbacks : IPoseIntegratorCallbacks
        {
            // Private
            private Vector3 gravity;
            private Vector3Wide gravityWideDt;

            // Properties
            public AngularIntegrationMode AngularIntegrationMode => AngularIntegrationMode.Nonconserving;
            public bool AllowSubstepsForUnconstrainedBodies => false;
            public bool IntegrateVelocityForKinematics => false;

            // Constructor
            public PoseIntegratorCallbacks(Vector3 gravity)
            {
                this.gravity = gravity;
            }

            // Methods
            public void Initialize(Simulation simulation)
            {
            }

            public void PrepareForIntegration(float dt)
            {
                gravityWideDt = Vector3Wide.Broadcast((gravity * dt).ToNumerics());
            }

            public void IntegrateVelocity(Vector<int> bodyIndices, Vector3Wide position, QuaternionWide orientation, BodyInertiaWide localInertia, Vector<int> integrationMask, int workerIndex, Vector<float> dt, ref BodyVelocityWide velocity)
            {
                velocity.Linear += gravityWideDt;
            }            
        }

        // Private        
        private int threadCount = 1;
        private ThreadDispatcher threadDispatcher = null;
        private BufferPool bufferPool = new BufferPool();

        // Internal
        internal Simulation simulation = null;
        internal Dictionary<BodyHandle, RigidBody> dynamicBodies = new Dictionary<BodyHandle, RigidBody>();

        // Properties
        public int Priority => -100;

        // Constructor
        internal PhysicsSimulation(GameSettings gameSettings)
        {
            // Get thread count
            threadCount = Math.Max(1, Environment.ProcessorCount > 4
                ? Environment.ProcessorCount - 2
                : Environment.ProcessorCount - 1);

            // Create thread dispatcher
            threadDispatcher = new ThreadDispatcher(threadCount);
            
            // Create simulation
            simulation = Simulation.Create(bufferPool,
                new NarrowPhaseCallbacks(),
                new PoseIntegratorCallbacks(gameSettings.Gravity),
                new SolveDescription(8, 1));
                       
        }

        // Methods
        public void OnStart()
        {

        }

        public void OnUpdate(GameTime gameTime)
        {
            // Update simulation
            simulation.Timestep(0.015f, threadDispatcher);

            // Sync after update
            SyncDynamicBodies();
        }

        public void Dispose()
        {
            // Release simulation
            simulation.Dispose();
            simulation = null;

            threadDispatcher.Dispose();
            threadDispatcher = null;

            bufferPool.Clear();
        }

        private void SyncDynamicBodies()
        {
            // Process all bodies
            foreach(KeyValuePair<BodyHandle, RigidBody> body in dynamicBodies)
            {
                // Get the body
                BodyReference referenceBody = simulation.Bodies[body.Key];

                // Get the motion state
                MotionState motionState = referenceBody.MotionState;

                // Apply the pose
                SyncPose(body.Value.Transform, motionState.Pose);

                // Apply the velocity
                SyncVelocity(body.Value, motionState.Velocity);
            }
        }

        internal static RigidPose GetPose(Transform transform)
        {
            RigidPose pose;
            // Pos
            pose.Position.X = transform.WorldPosition.X;
            pose.Position.Y = transform.WorldPosition.Y;
            pose.Position.Z = transform.WorldPosition.Z;
            // Rot
            pose.Orientation.X = transform.WorldRotation.X;
            pose.Orientation.Y = transform.WorldRotation.Y;
            pose.Orientation.Z = transform.WorldRotation.Z;
            pose.Orientation.W = transform.WorldRotation.W;
            return pose;
        }

        internal static void SyncPose(Transform transform, in RigidPose pose)
        {
            // Get pos
            Vector3 pos;
            pos.X = pose.Position.X;
            pos.Y = pose.Position.Y;
            pos.Z = pose.Position.Z;

            // Get rotation
            Quaternion rot;
            rot.X = pose.Orientation.X;
            rot.Y = pose.Orientation.Y;
            rot.Z = pose.Orientation.Z;
            rot.W = pose.Orientation.W;

            // Update transform
            transform.WorldPosition = pos;
            transform.WorldRotation = rot;
        }

        internal static void SyncVelocity(RigidBody body, in BodyVelocity velocity)
        {
            // Get linear
            Vector3 lin;
            lin.X = velocity.Linear.X;
            lin.Y = velocity.Linear.Y;
            lin.Z = velocity.Linear.Z;

            // Get angular
            Vector3 ang;
            ang.X = velocity.Angular.X;
            ang.Y = velocity.Angular.Y;
            ang.Z = velocity.Angular.Z;

            // Update body
            body.velocity = lin;
            body.angularVelocity = ang;
        }
    }
}
