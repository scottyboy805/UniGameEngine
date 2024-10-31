
using System.Collections.Generic;

namespace UniGameEngine
{
    public sealed class GameDrawComparer : IComparer<IGameDraw>
    {
        // Methods
        public int Compare(IGameDraw x, IGameDraw y)
        {
            return x.DrawOrder.CompareTo(y.DrawOrder);
        }
    }

    public interface IGameDraw
    {
        // Properties
        int DrawOrder { get; }

        // Methods
        void OnDraw();
    }
}
