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
        [DataMember(Name = "Model")]
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
        public override void OnDraw(Camera camera)
        {
            // Check for mesh
            if (model == null)
                return;

            // Get view matrix
            Matrix view = camera != null
                ? camera.Transform.localToWorldMatrix
                : Matrix.Identity;

            // Get projection matrix
            Matrix projection = camera != null
                ? camera.projectionMatrix
                : Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60), GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000f);

            // Draw the model
            model.Draw(Transform.localToWorldMatrix, view, projection);
        }
    }
}
