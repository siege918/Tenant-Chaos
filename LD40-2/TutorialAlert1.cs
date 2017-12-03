using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD40_2
{
    class TutorialAlert1 : Alert
    {
        public TutorialAlert1(Game1 Game, Apartment apartment, int X) : base(Game, apartment, X)
        {

        }

        public override void Initialize(Apartment apartment, Apartment toApartment)
        {
            greeting = "Congratulations!";
            body = WrapText(
                    "Welcome to your new plot of land! It may not look like much, but it's your shot at making some serious cash!\n\n"
                    + "In a just a bit I'll hand you your first bit of cash, but before we do, we'll explain a little bit of what's going on here!\n\n"
                    + "You can use your scroll wheel to zoom in and out of your view of the apartment.You can also left - click and drag to pan around (although there's not much to see here yet)."
                );
            farewell = "";
            choices = new string[1];
            choices[0] = "Continue";
            expirationTime = new TimeSpan(1, 0, 0);
        }

        public override void Dismiss()
        {
            game.AddAlert(new TutorialAlert2(game, null, game.alertX));
            base.Dismiss();
        }
    }
}
