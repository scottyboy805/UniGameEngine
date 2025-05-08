using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UniGameEngine
{
    public class TestScript : BehaviourScript
    {
        BasicEffect e;
        public override void OnUpdate(GameTime gameTime)
        {
            Transform.LocalRotation *= Quaternion.CreateFromAxisAngle(Vector3.Forward,
                MathHelper.ToRadians((float)gameTime.ElapsedGameTime.TotalSeconds));
            return;
            Transform.LocalRotation *= Quaternion.CreateFromAxisAngle(Vector3.Up, 
                MathHelper.ToRadians((float)gameTime.ElapsedGameTime.TotalSeconds));
        }
    }
}
