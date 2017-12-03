using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD40_2
{
    class RentIncreaseAlert : Alert
    {
        public RentIncreaseAlert(Game1 Game, Apartment apartment, int X) : base(Game, apartment, X)
        {
            
        }

        public override void Initialize(Apartment apartment, Apartment toApartment)
        {
            greeting = "Congratulations!";
            body = WrapText(
                    "We would like to give you the authority to increase your base rent price. Let us know if you'd like to go through with it and we'll file the paperwork."
                );
            farewell = "Sincerely,\n\nThe Authority On Housing";
            choices = new string[3];
            choices[0] = "Raise Rent 10%";
            choices[1] = "Raise Rent 50%";
            choices[2] = "Pass (More Trust)";
            expirationTime = new TimeSpan(0, 2, 0);
        }

        public override void Answer(int index)
        {
            if (index == 0)
            {
                game.IncreaseRent(.1);
                game.ModifyTrust(-.1);
            }
            else if (index == 1)
            {
                game.IncreaseRent(.5);
                game.ModifyTrust(-.5);
            }
            else
            {
                game.ModifyTrust(.1);
            }
            base.Answer(index);
        }

        public override void Dismiss()
        {
            base.Dismiss();
        }
    }
}
