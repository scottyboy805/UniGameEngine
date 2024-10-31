
namespace UniGameEngine
{
    internal sealed class DesktopUniGame : UniGame
    {
        // Properties
        public override bool IsEditor => false;
        public override bool IsPlaying => true;
    }
}
