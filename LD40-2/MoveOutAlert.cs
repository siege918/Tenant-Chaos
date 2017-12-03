using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD40_2
{
    class MoveOutAlert : Alert
    {
        int cost;
        public MoveOutAlert(Game1 game, Apartment apartment, int X) : base(game, apartment, X, null)
        {

        }

        public override void Initialize(Apartment apartment, Apartment toApartment)
        {
            cost = Game1.rand.Next(game.BuildingCost);
            greeting = apartment.Greeting;
            body = WrapText(
                    "The tenant in Apartment " + apartment.ApartmentNumber + " is fed up with you and has moved out. They trashed the place, and you'll need to pay $" + cost + " in damages."
                );
            farewell = apartment.Farewell + "\n\n" + apartment.TenantName;
            choices = new string[1];
            choices[0] = "\"Well shit.\"";
            expirationTime = new TimeSpan(0, 3, 0);
        }

        public override void Dismiss()
        {
            game.GiveMoney(-1 * cost);
            apartment.MoveOut();
            base.Dismiss();
        }
    }
}
