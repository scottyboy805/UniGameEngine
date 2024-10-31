using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace UniGameEngine
{
    public sealed class GameUpdateComparer : IComparer<IGameUpdate>
    {
        // Methods
        public int Compare(IGameUpdate x, IGameUpdate y)
        {
            return x.Priority.CompareTo(y.Priority);
        }
    }

    public interface IGameUpdate
    {
        // Properties
        int Priority { get; }

        // Methods
        void OnStart();

        void OnUpdate(GameTime gameTime);
    }
}
