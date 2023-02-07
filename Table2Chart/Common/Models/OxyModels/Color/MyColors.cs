using OxyPlot;
using System;

namespace Table2Chart.Common.Models.OxyModels.Color
{
    public static class MyColors
    {
        /// <summary>
        /// 随机颜色
        /// </summary>
        public static OxyColor Random
        {
            get
            {
                Random rd = new Random(GetRandomSeed());
                uint v = (uint)rd.Next(0, int.MaxValue);
                return OxyColor.FromUInt32(v);
            }
        }

        /// <summary>
        /// 取随机数种子
        /// </summary>
        /// <returns></returns>
        public static int GetRandomSeed()
        {
            byte[] bytes = new byte[100];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 50);
        }

        /// <summary>
        /// The undefined color.
        /// </summary>
        public static OxyColor Undefined => OxyColor.FromUInt32(0x00000000);

        /// <summary>
        /// The automatic color.
        /// </summary>
        public static OxyColor Automatic => OxyColor.FromUInt32(0x00000001);

        /// <summary>
        /// The alice blue.
        /// </summary>
        public static OxyColor AliceBlue => OxyColor.FromUInt32(0xFFF0F8FF);

        /// <summary>
        /// The antique white.
        /// </summary>
        public static OxyColor AntiqueWhite => OxyColor.FromUInt32(0xFFFAEBD7);

        /// <summary>
        /// The aqua.
        /// </summary>
        public static OxyColor Aqua => OxyColor.FromUInt32(0xFF00FFFF);

        /// <summary>
        /// The aquamarine.
        /// </summary>
        public static OxyColor Aquamarine => OxyColor.FromUInt32(0xFF7FFFD4);

        /// <summary>
        /// The azure.
        /// </summary>
        public static OxyColor Azure => OxyColor.FromUInt32(0xFFF0FFFF);

        /// <summary>
        /// The beige.
        /// </summary>
        public static OxyColor Beige => OxyColor.FromUInt32(0xFFF5F5DC);

        /// <summary>
        /// The bisque.
        /// </summary>
        public static OxyColor Bisque => OxyColor.FromUInt32(0xFFFFE4C4);

        /// <summary>
        /// The black.
        /// </summary>
        public static OxyColor Black => OxyColor.FromUInt32(0xFF000000);

        /// <summary>
        /// The blanched almond.
        /// </summary>
        public static OxyColor BlanchedAlmond => OxyColor.FromUInt32(0xFFFFEBCD);

        /// <summary>
        /// The blue.
        /// </summary>
        public static OxyColor Blue => OxyColor.FromUInt32(0xFF0000FF);

        /// <summary>
        /// The blue violet.
        /// </summary>
        public static OxyColor BlueViolet => OxyColor.FromUInt32(0xFF8A2BE2);

        /// <summary>
        /// The brown.
        /// </summary>
        public static OxyColor Brown => OxyColor.FromUInt32(0xFFA52A2A);

        /// <summary>
        /// The burly wood.
        /// </summary>
        public static OxyColor BurlyWood => OxyColor.FromUInt32(0xFFDEB887);

        /// <summary>
        /// The cadet blue.
        /// </summary>
        public static OxyColor CadetBlue => OxyColor.FromUInt32(0xFF5F9EA0);

        /// <summary>
        /// The chartreuse.
        /// </summary>
        public static OxyColor Chartreuse => OxyColor.FromUInt32(0xFF7FFF00);

        /// <summary>
        /// The chocolate.
        /// </summary>
        public static OxyColor Chocolate => OxyColor.FromUInt32(0xFFD2691E);

        /// <summary>
        /// The coral.
        /// </summary>
        public static OxyColor Coral => OxyColor.FromUInt32(0xFFFF7F50);

        /// <summary>
        /// The cornflower blue.
        /// </summary>
        public static OxyColor CornflowerBlue => OxyColor.FromUInt32(0xFF6495ED);

        /// <summary>
        /// The cornsilk.
        /// </summary>
        public static OxyColor Cornsilk => OxyColor.FromUInt32(0xFFFFF8DC);

        /// <summary>
        /// The crimson.
        /// </summary>
        public static OxyColor Crimson => OxyColor.FromUInt32(0xFFDC143C);

        /// <summary>
        /// The cyan.
        /// </summary>
        public static OxyColor Cyan => OxyColor.FromUInt32(0xFF00FFFF);

        /// <summary>
        /// The dark blue.
        /// </summary>
        public static OxyColor DarkBlue => OxyColor.FromUInt32(0xFF00008B);

        /// <summary>
        /// The dark cyan.
        /// </summary>
        public static OxyColor DarkCyan => OxyColor.FromUInt32(0xFF008B8B);

        /// <summary>
        /// The dark goldenrod.
        /// </summary>
        public static OxyColor DarkGoldenrod => OxyColor.FromUInt32(0xFFB8860B);

        /// <summary>
        /// The dark gray.
        /// </summary>
        public static OxyColor DarkGray => OxyColor.FromUInt32(0xFFA9A9A9);

        /// <summary>
        /// The dark green.
        /// </summary>
        public static OxyColor DarkGreen => OxyColor.FromUInt32(0xFF006400);

        /// <summary>
        /// The dark khaki.
        /// </summary>
        public static OxyColor DarkKhaki => OxyColor.FromUInt32(0xFFBDB76B);

        /// <summary>
        /// The dark magenta.
        /// </summary>
        public static OxyColor DarkMagenta => OxyColor.FromUInt32(0xFF8B008B);

        /// <summary>
        /// The dark olive green.
        /// </summary>
        public static OxyColor DarkOliveGreen => OxyColor.FromUInt32(0xFF556B2F);

        /// <summary>
        /// The dark orange.
        /// </summary>
        public static OxyColor DarkOrange => OxyColor.FromUInt32(0xFFFF8C00);

        /// <summary>
        /// The dark orchid.
        /// </summary>
        public static OxyColor DarkOrchid => OxyColor.FromUInt32(0xFF9932CC);

        /// <summary>
        /// The dark red.
        /// </summary>
        public static OxyColor DarkRed => OxyColor.FromUInt32(0xFF8B0000);

        /// <summary>
        /// The dark salmon.
        /// </summary>
        public static OxyColor DarkSalmon => OxyColor.FromUInt32(0xFFE9967A);

        /// <summary>
        /// The dark sea green.
        /// </summary>
        public static OxyColor DarkSeaGreen => OxyColor.FromUInt32(0xFF8FBC8F);

        /// <summary>
        /// The dark slate blue.
        /// </summary>
        public static OxyColor DarkSlateBlue => OxyColor.FromUInt32(0xFF483D8B);

        /// <summary>
        /// The dark slate gray.
        /// </summary>
        public static OxyColor DarkSlateGray => OxyColor.FromUInt32(0xFF2F4F4F);

        /// <summary>
        /// The dark turquoise.
        /// </summary>
        public static OxyColor DarkTurquoise => OxyColor.FromUInt32(0xFF00CED1);

        /// <summary>
        /// The dark violet.
        /// </summary>
        public static OxyColor DarkViolet => OxyColor.FromUInt32(0xFF9400D3);

        /// <summary>
        /// The deep pink.
        /// </summary>
        public static OxyColor DeepPink => OxyColor.FromUInt32(0xFFFF1493);

        /// <summary>
        /// The deep sky blue.
        /// </summary>
        public static OxyColor DeepSkyBlue => OxyColor.FromUInt32(0xFF00BFFF);

        /// <summary>
        /// The dim gray.
        /// </summary>
        public static OxyColor DimGray => OxyColor.FromUInt32(0xFF696969);

        /// <summary>
        /// The dodger blue.
        /// </summary>
        public static OxyColor DodgerBlue => OxyColor.FromUInt32(0xFF1E90FF);

        /// <summary>
        /// The firebrick.
        /// </summary>
        public static OxyColor Firebrick => OxyColor.FromUInt32(0xFFB22222);

        /// <summary>
        /// The floral white.
        /// </summary>
        public static OxyColor FloralWhite => OxyColor.FromUInt32(0xFFFFFAF0);

        /// <summary>
        /// The forest green.
        /// </summary>
        public static OxyColor ForestGreen => OxyColor.FromUInt32(0xFF228B22);

        /// <summary>
        /// The fuchsia.
        /// </summary>
        public static OxyColor Fuchsia => OxyColor.FromUInt32(0xFFFF00FF);

        /// <summary>
        /// The gainsboro.
        /// </summary>
        public static OxyColor Gainsboro => OxyColor.FromUInt32(0xFFDCDCDC);

        /// <summary>
        /// The ghost white.
        /// </summary>
        public static OxyColor GhostWhite => OxyColor.FromUInt32(0xFFF8F8FF);

        /// <summary>
        /// The gold.
        /// </summary>
        public static OxyColor Gold => OxyColor.FromUInt32(0xFFFFD700);

        /// <summary>
        /// The goldenrod.
        /// </summary>
        public static OxyColor Goldenrod => OxyColor.FromUInt32(0xFFDAA520);

        /// <summary>
        /// The gray.
        /// </summary>
        public static OxyColor Gray => OxyColor.FromUInt32(0xFF808080);

        /// <summary>
        /// The green.
        /// </summary>
        public static OxyColor Green => OxyColor.FromUInt32(0xFF008000);

        /// <summary>
        /// The green yellow.
        /// </summary>
        public static OxyColor GreenYellow => OxyColor.FromUInt32(0xFFADFF2F);

        /// <summary>
        /// The honeydew.
        /// </summary>
        public static OxyColor Honeydew => OxyColor.FromUInt32(0xFFF0FFF0);

        /// <summary>
        /// The hot pink.
        /// </summary>
        public static OxyColor HotPink => OxyColor.FromUInt32(0xFFFF69B4);

        /// <summary>
        /// The indian red.
        /// </summary>
        public static OxyColor IndianRed => OxyColor.FromUInt32(0xFFCD5C5C);

        /// <summary>
        /// The indigo.
        /// </summary>
        public static OxyColor Indigo => OxyColor.FromUInt32(0xFF4B0082);

        /// <summary>
        /// The ivory.
        /// </summary>
        public static OxyColor Ivory => OxyColor.FromUInt32(0xFFFFFFF0);

        /// <summary>
        /// The khaki.
        /// </summary>
        public static OxyColor Khaki => OxyColor.FromUInt32(0xFFF0E68C);

        /// <summary>
        /// The lavender.
        /// </summary>
        public static OxyColor Lavender => OxyColor.FromUInt32(0xFFE6E6FA);

        /// <summary>
        /// The lavender blush.
        /// </summary>
        public static OxyColor LavenderBlush => OxyColor.FromUInt32(0xFFFFF0F5);

        /// <summary>
        /// The lawn green.
        /// </summary>
        public static OxyColor LawnGreen => OxyColor.FromUInt32(0xFF7CFC00);

        /// <summary>
        /// The lemon chiffon.
        /// </summary>
        public static OxyColor LemonChiffon => OxyColor.FromUInt32(0xFFFFFACD);

        /// <summary>
        /// The light blue.
        /// </summary>
        public static OxyColor LightBlue => OxyColor.FromUInt32(0xFFADD8E6);

        /// <summary>
        /// The light coral.
        /// </summary>
        public static OxyColor LightCoral => OxyColor.FromUInt32(0xFFF08080);

        /// <summary>
        /// The light cyan.
        /// </summary>
        public static OxyColor LightCyan => OxyColor.FromUInt32(0xFFE0FFFF);

        /// <summary>
        /// The light goldenrod yellow.
        /// </summary>
        public static OxyColor LightGoldenrodYellow => OxyColor.FromUInt32(0xFFFAFAD2);

        /// <summary>
        /// The light gray.
        /// </summary>
        public static OxyColor LightGray => OxyColor.FromUInt32(0xFFD3D3D3);

        /// <summary>
        /// The light green.
        /// </summary>
        public static OxyColor LightGreen => OxyColor.FromUInt32(0xFF90EE90);

        /// <summary>
        /// The light pink.
        /// </summary>
        public static OxyColor LightPink => OxyColor.FromUInt32(0xFFFFB6C1);

        /// <summary>
        /// The light salmon.
        /// </summary>
        public static OxyColor LightSalmon => OxyColor.FromUInt32(0xFFFFA07A);

        /// <summary>
        /// The light sea green.
        /// </summary>
        public static OxyColor LightSeaGreen => OxyColor.FromUInt32(0xFF20B2AA);

        /// <summary>
        /// The light sky blue.
        /// </summary>
        public static OxyColor LightSkyBlue => OxyColor.FromUInt32(0xFF87CEFA);

        /// <summary>
        /// The light slate gray.
        /// </summary>
        public static OxyColor LightSlateGray => OxyColor.FromUInt32(0xFF778899);

        /// <summary>
        /// The light steel blue.
        /// </summary>
        public static OxyColor LightSteelBlue => OxyColor.FromUInt32(0xFFB0C4DE);

        /// <summary>
        /// The light yellow.
        /// </summary>
        public static OxyColor LightYellow => OxyColor.FromUInt32(0xFFFFFFE0);

        /// <summary>
        /// The lime.
        /// </summary>
        public static OxyColor Lime => OxyColor.FromUInt32(0xFF00FF00);

        /// <summary>
        /// The lime green.
        /// </summary>
        public static OxyColor LimeGreen => OxyColor.FromUInt32(0xFF32CD32);

        /// <summary>
        /// The linen.
        /// </summary>
        public static OxyColor Linen => OxyColor.FromUInt32(0xFFFAF0E6);

        /// <summary>
        /// The magenta.
        /// </summary>
        public static OxyColor Magenta => OxyColor.FromUInt32(0xFFFF00FF);

        /// <summary>
        /// The maroon.
        /// </summary>
        public static OxyColor Maroon => OxyColor.FromUInt32(0xFF800000);

        /// <summary>
        /// The medium aquamarine.
        /// </summary>
        public static OxyColor MediumAquamarine => OxyColor.FromUInt32(0xFF66CDAA);

        /// <summary>
        /// The medium blue.
        /// </summary>
        public static OxyColor MediumBlue => OxyColor.FromUInt32(0xFF0000CD);

        /// <summary>
        /// The medium orchid.
        /// </summary>
        public static OxyColor MediumOrchid => OxyColor.FromUInt32(0xFFBA55D3);

        /// <summary>
        /// The medium purple.
        /// </summary>
        public static OxyColor MediumPurple => OxyColor.FromUInt32(0xFF9370DB);

        /// <summary>
        /// The medium sea green.
        /// </summary>
        public static OxyColor MediumSeaGreen => OxyColor.FromUInt32(0xFF3CB371);

        /// <summary>
        /// The medium slate blue.
        /// </summary>
        public static OxyColor MediumSlateBlue => OxyColor.FromUInt32(0xFF7B68EE);

        /// <summary>
        /// The medium spring green.
        /// </summary>
        public static OxyColor MediumSpringGreen => OxyColor.FromUInt32(0xFF00FA9A);

        /// <summary>
        /// The medium turquoise.
        /// </summary>
        public static OxyColor MediumTurquoise => OxyColor.FromUInt32(0xFF48D1CC);

        /// <summary>
        /// The medium violet red.
        /// </summary>
        public static OxyColor MediumVioletRed => OxyColor.FromUInt32(0xFFC71585);

        /// <summary>
        /// The midnight blue.
        /// </summary>
        public static OxyColor MidnightBlue => OxyColor.FromUInt32(0xFF191970);

        /// <summary>
        /// The mint cream.
        /// </summary>
        public static OxyColor MintCream => OxyColor.FromUInt32(0xFFF5FFFA);

        /// <summary>
        /// The misty rose.
        /// </summary>
        public static OxyColor MistyRose => OxyColor.FromUInt32(0xFFFFE4E1);

        /// <summary>
        /// The moccasin.
        /// </summary>
        public static OxyColor Moccasin => OxyColor.FromUInt32(0xFFFFE4B5);

        /// <summary>
        /// The navajo white.
        /// </summary>
        public static OxyColor NavajoWhite => OxyColor.FromUInt32(0xFFFFDEAD);

        /// <summary>
        /// The navy.
        /// </summary>
        public static OxyColor Navy => OxyColor.FromUInt32(0xFF000080);

        /// <summary>
        /// The old lace.
        /// </summary>
        public static OxyColor OldLace => OxyColor.FromUInt32(0xFFFDF5E6);

        /// <summary>
        /// The olive.
        /// </summary>
        public static OxyColor Olive => OxyColor.FromUInt32(0xFF808000);

        /// <summary>
        /// The olive drab.
        /// </summary>
        public static OxyColor OliveDrab => OxyColor.FromUInt32(0xFF6B8E23);

        /// <summary>
        /// The orange.
        /// </summary>
        public static OxyColor Orange => OxyColor.FromUInt32(0xFFFFA500);

        /// <summary>
        /// The orange red.
        /// </summary>
        public static OxyColor OrangeRed => OxyColor.FromUInt32(0xFFFF4500);

        /// <summary>
        /// The orchid.
        /// </summary>
        public static OxyColor Orchid => OxyColor.FromUInt32(0xFFDA70D6);

        /// <summary>
        /// The pale goldenrod.
        /// </summary>
        public static OxyColor PaleGoldenrod => OxyColor.FromUInt32(0xFFEEE8AA);

        /// <summary>
        /// The pale green.
        /// </summary>
        public static OxyColor PaleGreen => OxyColor.FromUInt32(0xFF98FB98);

        /// <summary>
        /// The pale turquoise.
        /// </summary>
        public static OxyColor PaleTurquoise => OxyColor.FromUInt32(0xFFAFEEEE);

        /// <summary>
        /// The pale violet red.
        /// </summary>
        public static OxyColor PaleVioletRed => OxyColor.FromUInt32(0xFFDB7093);

        /// <summary>
        /// The papaya whip.
        /// </summary>
        public static OxyColor PapayaWhip => OxyColor.FromUInt32(0xFFFFEFD5);

        /// <summary>
        /// The peach puff.
        /// </summary>
        public static OxyColor PeachPuff => OxyColor.FromUInt32(0xFFFFDAB9);

        /// <summary>
        /// The peru.
        /// </summary>
        public static OxyColor Peru => OxyColor.FromUInt32(0xFFCD853F);

        /// <summary>
        /// The pink.
        /// </summary>
        public static OxyColor Pink => OxyColor.FromUInt32(0xFFFFC0CB);

        /// <summary>
        /// The plum.
        /// </summary>
        public static OxyColor Plum => OxyColor.FromUInt32(0xFFDDA0DD);

        /// <summary>
        /// The powder blue.
        /// </summary>
        public static OxyColor PowderBlue => OxyColor.FromUInt32(0xFFB0E0E6);

        /// <summary>
        /// The purple.
        /// </summary>
        public static OxyColor Purple => OxyColor.FromUInt32(0xFF800080);

        /// <summary>
        /// The red.
        /// </summary>
        public static OxyColor Red => OxyColor.FromUInt32(0xFFFF0000);

        /// <summary>
        /// The rosy brown.
        /// </summary>
        public static OxyColor RosyBrown => OxyColor.FromUInt32(0xFFBC8F8F);

        /// <summary>
        /// The royal blue.
        /// </summary>
        public static OxyColor RoyalBlue => OxyColor.FromUInt32(0xFF4169E1);

        /// <summary>
        /// The saddle brown.
        /// </summary>
        public static OxyColor SaddleBrown => OxyColor.FromUInt32(0xFF8B4513);

        /// <summary>
        /// The salmon.
        /// </summary>
        public static OxyColor Salmon => OxyColor.FromUInt32(0xFFFA8072);

        /// <summary>
        /// The sandy brown.
        /// </summary>
        public static OxyColor SandyBrown => OxyColor.FromUInt32(0xFFF4A460);

        /// <summary>
        /// The sea green.
        /// </summary>
        public static OxyColor SeaGreen => OxyColor.FromUInt32(0xFF2E8B57);

        /// <summary>
        /// The sea shell.
        /// </summary>
        public static OxyColor SeaShell => OxyColor.FromUInt32(0xFFFFF5EE);

        /// <summary>
        /// The sienna.
        /// </summary>
        public static OxyColor Sienna => OxyColor.FromUInt32(0xFFA0522D);

        /// <summary>
        /// The silver.
        /// </summary>
        public static OxyColor Silver => OxyColor.FromUInt32(0xFFC0C0C0);

        /// <summary>
        /// The sky blue.
        /// </summary>
        public static OxyColor SkyBlue => OxyColor.FromUInt32(0xFF87CEEB);

        /// <summary>
        /// The slate blue.
        /// </summary>
        public static OxyColor SlateBlue => OxyColor.FromUInt32(0xFF6A5ACD);

        /// <summary>
        /// The slate gray.
        /// </summary>
        public static OxyColor SlateGray => OxyColor.FromUInt32(0xFF708090);

        /// <summary>
        /// The snow.
        /// </summary>
        public static OxyColor Snow => OxyColor.FromUInt32(0xFFFFFAFA);

        /// <summary>
        /// The spring green.
        /// </summary>
        public static OxyColor SpringGreen => OxyColor.FromUInt32(0xFF00FF7F);

        /// <summary>
        /// The steel blue.
        /// </summary>
        public static OxyColor SteelBlue => OxyColor.FromUInt32(0xFF4682B4);

        /// <summary>
        /// The tan.
        /// </summary>
        public static OxyColor Tan => OxyColor.FromUInt32(0xFFD2B48C);

        /// <summary>
        /// The teal.
        /// </summary>
        public static OxyColor Teal => OxyColor.FromUInt32(0xFF008080);

        /// <summary>
        /// The thistle.
        /// </summary>
        public static OxyColor Thistle => OxyColor.FromUInt32(0xFFD8BFD8);

        /// <summary>
        /// The tomato.
        /// </summary>
        public static OxyColor Tomato => OxyColor.FromUInt32(0xFFFF6347);

        /// <summary>
        /// The transparent.
        /// </summary>
        public static OxyColor Transparent => OxyColor.FromUInt32(0x00FFFFFF);

        /// <summary>
        /// The turquoise.
        /// </summary>
        public static OxyColor Turquoise => OxyColor.FromUInt32(0xFF40E0D0);

        /// <summary>
        /// The violet.
        /// </summary>
        public static OxyColor Violet => OxyColor.FromUInt32(0xFFEE82EE);

        /// <summary>
        /// The wheat.
        /// </summary>
        public static OxyColor Wheat => OxyColor.FromUInt32(0xFFF5DEB3);

        /// <summary>
        /// The white.
        /// </summary>
        public static OxyColor White => OxyColor.FromUInt32(0xFFFFFFFF);

        /// <summary>
        /// The white smoke.
        /// </summary>
        public static OxyColor WhiteSmoke => OxyColor.FromUInt32(0xFFF5F5F5);

        /// <summary>
        /// The yellow.
        /// </summary>
        public static OxyColor Yellow => OxyColor.FromUInt32(0xFFFFFF00);

        /// <summary>
        /// The yellow green.
        /// </summary>
        public static OxyColor YellowGreen => OxyColor.FromUInt32(0xFF9ACD32);
    }
}