using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD40_2
{
    class TutorialAlert3 : Alert
    {
        public TutorialAlert3(Game1 Game, Apartment apartment, int X) : base(Game, apartment, X)
        {

        }

        public override void Initialize(Apartment apartment, Apartment toApartment)
        {
            greeting = "";
            body = WrapText(
                    "I'll go ahead and give you enough money to buy your first property. Wait around for a tenant to show up, and then you'll start to earn money monthly! (Well, every 15 seconds).\n\n " +
                    "Good luck! Once you have the money, click the \"Buy\" button below and wait for your first tenant!\n\n" +
                    "(Oh, and this game is not for kids)"
                );
            farewell = "";
            choices = new string[1];
            choices[0] = "Continue (Gain $200)";
            expirationTime = new TimeSpan(1, 0, 0);
        }

        public override void Dismiss()
        {
            game.GiveMoney(200);
            game.StartTime();
            base.Dismiss();
        }
    }
}
