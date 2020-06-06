namespace IWBTM.Game.UserInterface
{
    public class IWannaSelectableButtonBackground : IWannaButtonBackground
    {
        public IWannaSelectableButtonBackground()
        {
            BorderColour = IWannaColour.Blue;
        }

        public void Activate() => BorderThickness = 5;

        public void Deactivate() => BorderThickness = 0;
    }
}
