using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD40_2
{
    class TutorialAlert2 : Alert
    {
        public TutorialAlert2(Game1 Game, Apartment apartment, int X) : base(Game, apartment, X)
        {

        }

        public override void Initialize(Apartment apartment, Apartment toApartment)
        {
            greeting = "";
            body = WrapText(
                    "Eventually you'll build apartments here, and get tenants. Be sure to listen to what they ask for and help how you can without going broke. If you spend more money than you have, you'll lose!"
                );
            farewell = "";
            choices = new string[1];
            choices[0] = "Continue";
            expirationTime = new TimeSpan(1, 0, 0);
        }

        public override void Dismiss()
        {
            game.AddAlert(new TutorialAlert3(game, null, game.alertX));
            base.Dismiss();
        }
    }
}
