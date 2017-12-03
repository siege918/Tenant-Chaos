using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace LD40_2
{
    class RepairAlert : Alert
    {
        string[] BreakableItems = new string[]
        {
            "kitchen sink",
            "front doorknob",
            "fridge",
            "bathroom sink",
            "toilet",
            "garbage disposal",
            "cabinet",
            "door",
            "window",
            "floor",
            "ceiling fan",
            "light bulbs",
            "shower",
            "bathtub"
        };

        string[] BreakingWays = new string[]
        {
            "shattered",
            "broke",
            "stained",
            "blew up",
            "pissed on",
            "...\"lost\"...",
            "cleaved",
            "sliced",
            "collapsed",
            "hacked"
        };

        public static Dictionary<double, string[]> messages = new Dictionary<double, string[]>() {
            {.5d, new string[] {
                    "I am so so so so sorry! I didn't want to cause any trouble, but I ${broke} my ${item}, and I really hope you'll help me out.",
                    "I don't want to be a burden, but I accidentally ${broke} my ${item}. Can you help me fix it?",
                    "Please don't be mad; I ${broke} my ${ITEM}. Can you help me out? I would appreciate it."
                }
            },
            {1,
                new string[]
                {
                    "I am writing to report a maintenance issue. Someone ${broke} the ${item}. Please send someone over to look at it.",
                    "I need maintenance to come over for an emergency repair. I ${broke} my ${item} and need to have it fixed.",
                    "Through no fault of my own, my ${item} was ${broke}. Can you send a maintenance person over?"
                }
            },
            {1.5,
                new string[]
                {
                    "Send maintenance. My ${item} has been ${broke}. Please hurry.",
                    "Come and fix my ${item}. I don't like to wait. Come quickly.",
                    "It is incredibly important that my ${item} is fixed, after being ${broke} earlier this week. It is very important to my daily life. Please respond."
                }
            },
            {2,
                new string[]
                {
                    "Send maintenance. My ${item} has been ${broke}. Please hurry.",
                    "Come and fix my ${item}. I don't like to wait. Come quickly.",
                    "It is incredibly important that my ${item} is fixed, after being ${broke} earlier this week. It is very important to my daily life. Please respond."
                }
            },
            {3,
                new string[]
                {
                    "Look. I know you're not really a ${curse}. I want to believe you'll fix my ${item}. I just ${broke} it and I need help. So, y'know, fucking do it.",
                    "Fix. My. Shit.",
                    "I ${broke} my ${item} a few days back, and you have yet to fix it. Can we get this taken care of?",
                    "Listen ${curse}, I'm ready to go to court over this. Fix my ${item}. My lease says that you cover ${item}s being ${broke}. So get to it."
                }
            },
            {4,
                new string[]
                {
                    "YOU ABSOLUTE ${CURSE}! IT'S BROKEN! IT'S ALL FUCKING BROKEN! THEY ${BROKE} MY FUCKING ${ITEM}! FIX IT FIX IT FIX IT!",
                    "It's no big deal. It's no big deal. Don't worry about it. It's just that my FUCKING ${ITEM} GOT ${BROKE}",
                    "YOU ${CURSE}! YOU FUCKING ${CURSE}! YOU ABSOLUTE ${CURSE}! FIX MY ${ITEM}! I FUCKING ${BROKE} IT AND I NEED TO TO BE NOT ${BROKE}!"
                }
            }
        };
        
        private int Price = 10;
        public RepairAlert(Game1 Game, Apartment apartment, int X) : base(Game, apartment, X)
        {

        }

        public override void Initialize(Apartment apartment, Apartment toApartment)
        {
            double rage = apartment.TemperModifier;

            double severityOdds = Game1.rand.NextDouble();
            double ageInMonths = apartment.AgeInMonths;
            if (severityOdds < .01d * (ageInMonths / 3))
            {
                Price = game.BuildingCost / 2;
            }
            else if (severityOdds < .5 + (.01d * (ageInMonths / 3)))
            {
                Price = game.BuildingCost / 5;
            }
            else if (severityOdds < .8 + (.01d * (ageInMonths / 3)))
            {
                Price = game.BuildingCost / 10;
            }
            else
            {
                Price = game.BuildingCost / 100;
            }

            if (Price < 10)
            {
                Price = (int)(Game1.rand.Next(3,10) * apartment.AgeInMonths);
            }

            greeting = apartment.Greeting;
            body = WrapText(
                    ParseMessage(messages[rage][Game1.rand.Next(messages[rage].Length)])
                );
            farewell = apartment.Farewell + "\n" + apartment.TenantName + "\nApartment " + apartment.ApartmentNumber;
            choices = new string[4];
            choices[0] = "Fix it ($" + Price + ")";
            choices[1] = "\"I'll do it later\"";
            choices[2] = "\"Do it yourself\"";
            choices[3] = "Refuse";
            expirationTime = new TimeSpan(0, 0, 15);
        }

        public override void Expire()
        {
            apartment.ModifyTrust(-.5);
            base.Expire();
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
                game.GiveMoney(Price * -1);
                apartment.ModifyTrust(.1);
            }
            else if (index == 1)
            {
                apartment.ModifyTrust(-.1);
            }
            else if (index == 2)
            {
                apartment.ModifyTrust(-.2);
            }
            else
            {
                apartment.ModifyTrust(-.5);
            }
            base.Answer(index);
        }

        private string ParseMessage(string message)
        {
            string msg = Tenant.ParseCurses(message);
            string item = BreakableItems[Game1.rand.Next(BreakableItems.Length)];
            string broke = BreakingWays[Game1.rand.Next(BreakingWays.Length)];
            msg = msg.Replace("${ITEM}", item.ToUpper());
            msg = msg.Replace("${item}", item);
            msg = msg.Replace("${BROKE}", broke.ToUpper());
            msg = msg.Replace("${broke}", broke);
            return msg;
        }
    }
}
