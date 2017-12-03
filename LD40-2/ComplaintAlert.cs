using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LD40_2
{
    class ComplaintAlert : Alert
    {
        Apartment toApartment;

        private static string[] nouns = new string[]
        {
            "fucking",
            "peacocking",
            "jazzercising",
            "redditing",
            "masturbating",
            "eating",
            "reading",
            "stomping",
            "yelling",
            "yodelling"
        };

        private static string[] verbs = new string[]
        {
            "see",
            "hear",
            "smell"
        };

        public ComplaintAlert(Game1 game, Apartment apartment, int X, Apartment toApartment) : base(game, apartment, X, toApartment)
        {

        }

        public override void Initialize(Apartment apartment, Apartment toApartment)
        {
            this.toApartment = toApartment;
            greeting = apartment.Greeting;
            body = WrapText(
                    Parse("I can always ${verb} my neighbors in Apartment " + toApartment.ApartmentNumber + " ${noun}, and I'm sick of it. It's cutting into my ${noun} time. Can you talk to them?")
                );
            farewell = apartment.Farewell + "\n" + apartment.TenantName + "\nApartment " + apartment.ApartmentNumber;
            choices = new string[4];
            choices[0] = "\"Deal with it yourselves.\"";
            choices[1] = "Give a Warning";
            choices[2] = "Evict Apartment " + toApartment.ApartmentNumber;
            choices[3] = "Evict Apartment " + apartment.ApartmentNumber;
            expirationTime = new TimeSpan(0, 0, 30);
        }

        public override void Update(GameTime gameTime, MouseState mouseState, Point mousePos, bool allowInteract, bool removed)
        {
            if (!Apartment.Occupied(apartment) || !Apartment.Occupied(toApartment))
                Dismiss();

            base.Update(gameTime, mouseState, mousePos, allowInteract, removed);
        }

        public override void Answer(int index)
        {
            if (index == 0)
            {
                apartment.ModifyTrust(-.1);
                toApartment.ModifyTrust(.1);
            }
            else if (index == 1)
            {
                apartment.ModifyTrust(.1);
                toApartment.ModifyTrust(-.1);
            }
            else if (index == 2)
            {
                apartment.ModifyTrust(1);
                toApartment.MoveOut();
            }
            else
            {
                apartment.MoveOut();
                toApartment.ModifyTrust(.5);
            }
            base.Answer(index);
        }

        public static string Parse(string val)
        {
            while (val.Contains("${NOUN}"))
            {
                string noun = nouns[Game1.rand.Next(nouns.Length)];
                val = val.ReplaceFirst("${NOUN}", noun.ToUpper());
            }
            while (val.Contains("${noun}"))
            {
                string noun = nouns[Game1.rand.Next(nouns.Length)];
                val = val.ReplaceFirst("${noun}", noun);
            }
            while (val.Contains("${verb}"))
            {
                string verb = verbs[Game1.rand.Next(verbs.Length)];
                val = val.ReplaceFirst("${verb}", verb);
            }

            return val;
        }
    }
}
