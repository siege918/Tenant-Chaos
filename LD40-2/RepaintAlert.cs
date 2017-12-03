using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace LD40_2
{
    class RepaintAlert : Alert
    {
        Color color;
        public RepaintAlert(Game1 Game, Apartment apartment, int X) : base(Game, apartment, X)
        {

        }

        public override void Initialize(Apartment apartment, Apartment toApartment)
        {
            string[] colorNames = KnownColors.ColorNames;
            string colorName = colorNames[Game1.rand.Next(colorNames.Length)];
            color = KnownColors.GetColor(colorName);

            greeting = apartment.Greeting;
            body = WrapText(
                    "I want to repaint my apartment. Can I paint it " + colorName + "?"
                );
            farewell = apartment.Farewell + "\n" + apartment.TenantName + "\nApartment" + apartment.ApartmentNumber;
            choices = new string[2];
            choices[0] = "\"Go ahead.\"";
            choices[1] = "\"Gross. No.\"";
            expirationTime = new TimeSpan(0, 0, 30);
        }

        public override void Update(GameTime gameTime, MouseState mouseState, Point mousePos, bool allowInteract, bool removed)
        {
            if (!Apartment.Occupied(apartment))
                Dismiss();

            base.Update(gameTime, mouseState, mousePos, allowInteract, removed);
        }

        public override void Answer(int index)
        {
            if (index == 0)
            {
                apartment.SetColor(color);
                apartment.ModifyTrust(.1);
            }
            else
            {
                apartment.ModifyTrust(-.05);
            }
            base.Answer(index);
        }
    }
}
