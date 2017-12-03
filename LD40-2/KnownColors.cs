using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD40_2
{
    class KnownColors
    {
        private static Dictionary<string, Color> _colors;

        public static string[] ColorNames
        {
            get
            {
                return Colors.Keys.ToArray();
            }
        }

        public static Color GetColor(string name)
        {
            try
            {
                return Colors[name];
            }
            catch
            {
                return Color.Gray;
            }
        }

        public static Dictionary<string, Color> Colors
        {
            get
            {
                if (_colors == null)
                {
                    _colors = new Dictionary<string, Color>() {
                        {"Alice Blue", Color.AliceBlue},
                        {"Antique White", Color.AntiqueWhite},
                        {"Aqua", Color.Aqua},
                        {"Aquamarine", Color.Aquamarine},
                        {"Azure", Color.Azure},
                        {"Beige", Color.Beige},
                        {"Bisque", Color.Bisque},
                        {"Black", Color.Black},
                        {"Blanched Almond", Color.BlanchedAlmond},
                        {"Blue", Color.Blue},
                        {"Blue Violet", Color.BlueViolet},
                        {"Brown", Color.Brown},
                        {"Burly Wood", Color.BurlyWood},
                        {"Cadet Blue", Color.CadetBlue},
                        {"Chartreuse", Color.Chartreuse},
                        {"Chocolate", Color.Chocolate},
                        {"Coral", Color.Coral},
                        {"Cornflower Blue", Color.CornflowerBlue},
                        {"Cornsilk", Color.Cornsilk},
                        {"Crimson", Color.Crimson},
                        {"Cyan", Color.Cyan},
                        {"Dark Blue", Color.DarkBlue},
                        {"Dark Cyan", Color.DarkCyan},
                        {"Dark Goldenrod", Color.DarkGoldenrod},
                        {"Dark Gray", Color.DarkGray},
                        {"Dark Green", Color.DarkGreen},
                        {"Dark Khaki", Color.DarkKhaki},
                        {"Dark Magenta", Color.DarkMagenta},
                        {"Dark Olive Green", Color.DarkOliveGreen},
                        {"Dark Orange", Color.DarkOrange},
                        {"Dark Orchid", Color.DarkOrchid},
                        {"Dark Red", Color.DarkRed},
                        {"Dark Salmon", Color.DarkSalmon},
                        {"Dark Sea Green", Color.DarkSeaGreen},
                        {"Dark Slate Blue", Color.DarkSlateBlue},
                        {"Dark Slate Gray", Color.DarkSlateGray},
                        {"Dark Turquoise", Color.DarkTurquoise},
                        {"Dark Violet", Color.DarkViolet},
                        {"Deep Pink", Color.DeepPink},
                        {"Deep Sky Blue", Color.DeepSkyBlue},
                        {"Dim Gray", Color.DimGray},
                        {"Dodger Blue", Color.DodgerBlue},
                        {"Firebrick", Color.Firebrick},
                        {"Floral White", Color.FloralWhite},
                        {"Forest Green", Color.ForestGreen},
                        {"Fuchsia", Color.Fuchsia},
                        {"Gainsboro", Color.Gainsboro},
                        {"Ghost White", Color.GhostWhite},
                        {"Gold", Color.Gold},
                        {"Goldenrod", Color.Goldenrod},
                        {"Gray", Color.Gray},
                        {"Green", Color.Green},
                        {"Green Yellow", Color.GreenYellow},
                        {"Honeydew", Color.Honeydew},
                        {"Hot Pink", Color.HotPink},
                        {"Indian Red", Color.IndianRed},
                        {"Indigo", Color.Indigo},
                        {"Ivory", Color.Ivory},
                        {"Khaki", Color.Khaki},
                        {"Lavender", Color.Lavender},
                        {"Lavender Blush", Color.LavenderBlush},
                        {"Lawn Green", Color.LawnGreen},
                        {"Lemon Chiffon", Color.LemonChiffon},
                        {"Light Blue", Color.LightBlue},
                        {"Light Coral", Color.LightCoral},
                        {"Light Cyan", Color.LightCyan},
                        {"Light Goldenrod Yellow", Color.LightGoldenrodYellow},
                        {"Light Gray", Color.LightGray},
                        {"Light Green", Color.LightGreen},
                        {"Light Pink", Color.LightPink},
                        {"Light Salmon", Color.LightSalmon},
                        {"Light Sea Green", Color.LightSeaGreen},
                        {"Light Sky Blue", Color.LightSkyBlue},
                        {"Light Slate Gray", Color.LightSlateGray},
                        {"Light Steel Blue", Color.LightSteelBlue},
                        {"Light Yellow", Color.LightYellow},
                        {"Lime", Color.Lime},
                        {"Lime Green", Color.LimeGreen},
                        {"Linen", Color.Linen},
                        {"Magenta", Color.Magenta},
                        {"Maroon", Color.Maroon},
                        {"Medium Aquamarine", Color.MediumAquamarine},
                        {"Medium Blue", Color.MediumBlue},
                        {"Medium Orchid", Color.MediumOrchid},
                        {"Medium Purple", Color.MediumPurple},
                        {"Medium Sea Green", Color.MediumSeaGreen},
                        {"Medium Slate Blue", Color.MediumSlateBlue},
                        {"Medium Spring Green", Color.MediumSpringGreen},
                        {"Medium Turquoise", Color.MediumTurquoise},
                        {"Medium Violet Red", Color.MediumVioletRed},
                        {"Midnight Blue", Color.MidnightBlue},
                        {"Mint Cream", Color.MintCream},
                        {"Misty Rose", Color.MistyRose},
                        {"Moccasin", Color.Moccasin},
                        {"Navajo White", Color.NavajoWhite},
                        {"Navy", Color.Navy},
                        {"Old Lace", Color.OldLace},
                        {"Olive", Color.Olive},
                        {"Olive Drab", Color.OliveDrab},
                        {"Orange", Color.Orange},
                        {"Orange Red", Color.OrangeRed},
                        {"Orchid", Color.Orchid},
                        {"Pale Goldenrod", Color.PaleGoldenrod},
                        {"Pale Green", Color.PaleGreen},
                        {"Pale Turquoise", Color.PaleTurquoise},
                        {"Pale Violet Red", Color.PaleVioletRed},
                        {"Papaya Whip", Color.PapayaWhip},
                        {"Peach Puff", Color.PeachPuff},
                        {"Peru", Color.Peru},
                        {"Pink", Color.Pink},
                        {"Plum", Color.Plum},
                        {"Powder Blue", Color.PowderBlue},
                        {"Purple", Color.Purple},
                        {"Red", Color.Red},
                        {"Rosy Brown", Color.RosyBrown},
                        {"Royal Blue", Color.RoyalBlue},
                        {"Saddle Brown", Color.SaddleBrown},
                        {"Salmon", Color.Salmon},
                        {"Sandy Brown", Color.SandyBrown},
                        {"Sea Green", Color.SeaGreen},
                        {"Sea Shell", Color.SeaShell},
                        {"Sienna", Color.Sienna},
                        {"Silver", Color.Silver},
                        {"Sky Blue", Color.SkyBlue},
                        {"Slate Blue", Color.SlateBlue},
                        {"Slate Gray", Color.SlateGray},
                        {"Snow", Color.Snow},
                        {"Spring Green", Color.SpringGreen},
                        {"Steel Blue", Color.SteelBlue},
                        {"Tan", Color.Tan},
                        {"Teal", Color.Teal},
                        {"Thistle", Color.Thistle},
                        {"Tomato", Color.Tomato},
                        {"Turquoise", Color.Turquoise},
                        {"Violet", Color.Violet},
                        {"Wheat", Color.Wheat},
                        {"White", Color.White},
                        {"White Smoke", Color.WhiteSmoke},
                        {"Yellow", Color.Yellow},
                        {"Yellow Green", Color.YellowGreen}
                    };
                }
                return _colors;
            }
        }
    }
}
