using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
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

            Matrix.Invert(ref view, out view);
            view.M43 = -view.M43;
            
            // Get projection matrix
            Matrix projection = camera != null
                ? camera.projectionMatrix
                : Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60), GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000f);

            
            // Draw the model
            /*model.*/Draw(model, Transform.localToWorldMatrix, view, projection);
        }

        internal static void Draw(Model model, Matrix world, Matrix view, Matrix projection)
        {
            //int count = model.Bones.Count;
            //if (model.sharedDrawBoneMatrices == null || sharedDrawBoneMatrices.Length < count)
            //{
            //    sharedDrawBoneMatrices = new Matrix[count];
            //}

            
            //model.CopyAbsoluteBoneTransformsTo(sharedDrawBoneMatrices);
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    IEffectMatrices obj = (effect as IEffectMatrices) ?? throw new InvalidOperationException();
                    obj.World = mesh.ParentBone.Transform * world;
                    obj.View = view;
                    obj.Projection = projection;

                    // Check for basic effect
                    if(effect is BasicEffect basic)
                    {
                        // Check for lighting
                        if (basic.LightingEnabled == true && Light.HasActiveLights == true)
                        {
                            basic.DirectionalLight0.DiffuseColor = Light.MainLight.Color.ToVector3();
                            basic.DirectionalLight0.SpecularColor = Light.MainLight.Specular.ToVector3();
                            basic.DirectionalLight0.Direction = Light.MainLight.Direction;
                        }
                        else
                            basic.LightingEnabled = true;
                    }
                }

                mesh.Draw();
            }
        }
    }
}
