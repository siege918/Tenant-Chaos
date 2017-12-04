using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD40_2
{
    public class Tenant
    {
        public static Dictionary<double, string[]> greetings = new Dictionary<double, string[]>() {
            {.5d, new string[] {
                    "To whom it may concern:",
                    "Sorry to bother you,",
                    "I hope this is a good time,",
                    "If you're not too busy,",
                    "If you have time,"
                }
            },
            {1,
                new string[]
                {
                    "Hello",
                    "Hi,",
                    "Excuse me,",
                    "Dear Landlord,",
                    "Pardon me,"
                }
            },
            {1.5,
                new string[]
                {
                    "Hey",
                    "Hi",
                    "Yo",
                    "Dear Landlord Person,",
                    "Excuse Me"
                }
            },
            {2,
                new string[]
                {
                    "Dude,",
                    "Hey!",
                    "EXCUSE me",
                    "Hey hey hey"
                }
            },
            {3,
                new string[]
                {
                    "LANDLORD:",
                    "ExCUSE ME",
                    "PLEASE RESPOND:",
                    "LISTEN ${CURSE}:",
                    "HEY ${CURSE}:",
                    "${CURSE}"
                }
            },
            {4,
                new string[]
                {
                    "${CURSE} ${CURSE},",
                    "DEAR YOU ${CURSE},",
                    "HEY ${CURSE} ${CURSE} ${CURSE}"
                }
            }
        };

        public static Dictionary<double, string[]> farewells = new Dictionary<double, string[]>() {
            {.5d, new string[] {
                    "Sincerely,",
                    "Love,",
                    "Sorry to bother you,",
                    "Sorry,",
                    "I hope that's okay,"
                }
            },
            {1,
                new string[]
                {
                    "Sincerely,",
                    "Thanks,",
                    "Thanks!"
                }
            },
            {1.5,
                new string[]
                {
                    "Sincerely,",
                    "Sinseerly,",
                    "Hope to hear back soon,",
                    "Please respond,",
                    "I look forward to your prompt response,"
                }
            },
            {2,
                new string[]
                {
                    "Hurry up,",
                    "Whenever you feel like answering,",
                    "Hurry up,",
                    "Sincerely,"
                }
            },
            {3,
                new string[]
                {
                    "Fuck you,",
                    "Hit me up,",
                    "PLEASE RESPOND,",
                    "WHATEVER ${CURSE},",
                    "Sincerely,",
                    "${CURSE}"
                }
            },
            {4,
                new string[]
                {
                    "${CURSE} ${CURSE},",
                    "FUCK YOU ${CURSE},",
                    "FUCK YOU YOU ${CURSE} ${CURSE} ${CURSE}"
                }
            }
        };

        static string[] CurseStarts = new string[]{
            "FUCK",
            "PISS",
            "SHIT",
            "LOVE",
            "ASS",
            "DICK",
            "DOUCHE"
        };

        static string[] CurseEnds = new string[]
        {
            "NUGGET",
            "CANVAS",
            "LOVER",
            "HUNTER",
            "SNIFFER",
            "HIDER",
            "-READER",
            "EATER",
            "FUCKER",
            "SHITTER",
            "PLAYER",
            "RUDDER",
            "PLANTER",
            "CLIMBER",
            "TWEETER",
            "CANOE",
            "CHAIR",
            "ANUS"
        };

        private RandomNameGeneratorLibrary.PersonNameGenerator nameGenerator;
        public double TemperModifier;
        public int RentModifier;
        public int Trust;
        public string Name;

        public Tenant(Apartment apartment)
        {
            double rageOdds = .10d * (apartment.AgeInMonths / 3);

            if (rageOdds > .25)
            {
                rageOdds = .25;
            }
            Trust = 100 - (int)(apartment.AgeInMonths / 2);
            if (Trust < 50)
            {
                Trust = 50;
            }

            if (Game1.rand.NextDouble() < rageOdds)
            {
                double val = Game1.rand.NextDouble();
                if (val < .15)
                {
                    TemperModifier = 4;
                    RentModifier = 400;
                }
                else if (val < .5)
                {
                    TemperModifier = 3;
                    RentModifier = 200;
                }
                else
                {
                    TemperModifier = 2;
                    RentModifier = 100;
                }
            }
            else
            {
                double val = Game1.rand.NextDouble();
                if (val < .15)
                {
                    TemperModifier = .5;
                }
                else if (val < .6)
                {
                    TemperModifier = 1;
                }
                else
                {
                    TemperModifier = 1.5;
                }

                RentModifier = 0;
            }
            
            if (nameGenerator == null)
                nameGenerator = new RandomNameGeneratorLibrary.PersonNameGenerator(Game1.rand);

            Name = nameGenerator.GenerateRandomFirstAndLastName();
        }

        public string GetGreetingLine()
        {
            string[] vals = greetings[TemperModifier];
            string val = vals[Game1.rand.Next(vals.Length)];

            val = ParseCurses(val);

            return Alert.WrapText(val);
        }

        public string GetFarewellLine()
        {
            string[] vals = farewells[TemperModifier];
            string val = vals[Game1.rand.Next(vals.Length)];

            val = ParseCurses(val);

            return Alert.WrapText(val);
        }

        public static string ParseCurses(string val)
        {
            while (val.Contains("${CURSE}"))
            {
                string curse = CurseStarts[Game1.rand.Next(CurseStarts.Length)];
                curse += CurseEnds[Game1.rand.Next(CurseEnds.Length)];
                val = val.ReplaceFirst("${CURSE}", curse);
            }
            while (val.Contains("${curse}"))
            {
                string curse = CurseStarts[Game1.rand.Next(CurseStarts.Length)];
                curse += CurseEnds[Game1.rand.Next(CurseEnds.Length)];
                val = val.ReplaceFirst("${curse}", curse.ToLower());
            }
            return val;
        }

        public void ModifyTrust(double rate)
        {
            Trust += (int)(Trust * rate);
        }
    }
}
