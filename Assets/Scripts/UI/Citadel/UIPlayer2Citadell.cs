using CitadelShowdown.Citadel;
using Zenject;

namespace CitadelShowdown.UI.Citadel
{
    public class UIPlayer2Citadell : UICitadelBase
    {

        [Inject]
        public void Construct(Player1Citadel player1Citadel)
        {
            citadel = player1Citadel;
        }
    }
}
