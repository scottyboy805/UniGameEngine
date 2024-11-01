using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;

namespace UniGameEngine.Graphics
{
    public enum ShadowMode
    {
        /// <summary>
        /// No shadows.
        /// </summary>
        None = 0,
        /// <summary>
        /// Shadows and meshes are both rendered.
        /// </summary>
        On,
        /// <summary>
        /// Only shadows are rendered.
        /// </summary>
        ShadowsOnly,
    }

    public class ModelRenderer : Renderer
    {
        // Protected
        [DataMember(Name = "model")]
        protected Model model = null;
        [DataMember(Name = "ShadowMode")]
        protected ShadowMode shadowMode = ShadowMode.On;

        // Properties
        public override BoundingBox Bounds
        {
            get
            {
                return default;
            }
        }

        public Model Model
        {
            get { return model; }
            set { model = value; }
        }

        public ShadowMode ShadowMode
        {
            get { return shadowMode; }
            set { shadowMode = value; }
        }

        // Methods
        public override void OnDraw()
        {
            // Check for mesh
            if (model == null)
                return;

            Matrix world = Matrix.CreateTranslation(0, 0, 0);
            Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), Vector3.UnitY);
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60), GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000f);
            
            

            // Draw the model
            model.Draw(Transform.localToWorldMatrix, view, projection);
            //model.Draw(Transform.localToWorldMatrix, Camera.MainCamera.Transform.localToWorldMatrix, Camera.MainCamera.ViewProjectionMatrix);

            //// Get channel count
            //int meshChannelCount = mesh.NumberOfChannels;

            //// Process all channels
            //for (int i = 0; i < meshChannelCount; i++)
            //{
            //    // Get the material
            //    Material material = i < materials.Length
            //        ? materials[i] 
            //        : null;

            //    // Check for material
            //    if (material == null || material.Effect == null)
            //        continue;

            //    // Get the channel
            //    MeshChannel channel = mesh.Channels[i];

            //    material.Effect.Parameters["WorldViewProj"].SetValue(Camera.AllActiveCameras[0].ViewProjectionMatrix);

            //    // Update the buffers
            //    GraphicsDevice.Indices = channel.indexBuffer;
            //    GraphicsDevice.SetVertexBuffer(channel.vertexBuffer);

            //    // Render effect
            //    foreach(EffectPass pass in material.Effect.CurrentTechnique.Passes)
            //    {
            //        // Apply the pass
            //        pass.Apply();

            //        // Draw call
            //        GraphicsDevice.DrawIndexedPrimitives(channel.primitiveType, 0, 0, channel.indexBuffer.IndexCount);
            //    }
            //}
        }
    }
}
