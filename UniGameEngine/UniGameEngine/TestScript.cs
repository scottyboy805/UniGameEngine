using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UniGameEngine
{
    internal class TestScript : BehaviourScript
    {
        BasicEffect e;
        public override void OnUpdate(GameTime gameTime)
        {
            return;
            Transform.LocalRotation *= Quaternion.CreateFromAxisAngle(Vector3.Up, 
                MathHelper.ToRadians((float)gameTime.ElapsedGameTime.TotalSeconds));
        }
    }
}
