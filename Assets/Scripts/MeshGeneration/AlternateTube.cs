namespace TubeMeshGeneration
{
    public class AlternateTube : Tube
    {
        private Tube _mainTube;
        protected AlternateTube(Tube tube) : base(tube)
        {
            _mainTube = tube;
        }

    }
}