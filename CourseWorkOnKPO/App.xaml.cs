using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace CourseWorkOnKPO
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            ThemeCustomization();
        }

        private void ThemeCustomization()
        {
            StyleManager.ApplicationTheme = new FluentTheme();

            // StyleManager.ApplicationTheme = new Expression_DarkTheme();

            ////FluentControlEffectMode.
            FluentPalette.LoadPreset(FluentPalette.ColorVariation.Dark);
            //заливка вкладки и процессного блока - зеленая полоска
            FluentPalette.Palette.AccentColor = Color.FromArgb(200,72,61,139);
            //при наведении мыши на плитку - зеленая обводка плитки
            FluentPalette.Palette.AccentMouseOverColor = Color.FromArgb(120, 42, 0, 100);
            //цвет контура процесснного блока и вкладки - тонкая серая линия
            FluentPalette.Palette.BasicColor = Color.FromArgb(100,72,61,139);
            //цвет иконок процессного блока
            FluentPalette.Palette.IconColor = Color.FromArgb(255, 200, 200, 200);
            //заливка строки, на которой располагаются вкладки
            FluentPalette.Palette.MainColor = Color.FromArgb(255, 50, 50, 52);
            //цвет штрифта подписи плиток
            FluentPalette.Palette.MarkerColor = Color.FromArgb(255, 255, 255, 255);
            //цвет при наведении мыши на процессные блоки + вкладки
            FluentPalette.Palette.MouseOverColor =Color.FromArgb(150,72,61,139);
            //цвет выбранного процессного блока
            // FluentPalette.Palette.PressedColor = Color.FromArgb(255, 0x26,0x26,0x26);
            FluentPalette.Palette.PressedColor = Color.FromArgb(25,72,61,139);
            //фон процессного блока
            FluentPalette.Palette.AlternativeColor = Color.FromArgb(5,72,61,139);
            //заливка вкладки
            FluentPalette.Palette.PrimaryBackgroundColor = Color.FromArgb(255, 26, 26, 26);
            // 
            FluentPalette.Palette.ReadOnlyBackgroundColor = Color.FromArgb(255, 60, 60, 62);
            FluentPalette.Palette.ReadOnlyBorderColor = Color.FromArgb(255, 60, 60, 62);
            //??
            FluentPalette.Palette.AccentFocusedColor = Color.FromArgb(255, 50, 50, 52);
            FluentPalette.Palette.AccentPressedColor = Color.FromArgb(255, 50, 0, 52);
            FluentPalette.Palette.PrimaryMouseOverColor = Color.FromArgb(255, 100, 100, 100);
            FluentPalette.Palette.PrimaryColor = Color.FromArgb(255, 38, 38, 38);
            FluentPalette.Palette.MarkerInvertedColor = Color.FromArgb(255, 142, 142, 130);
            //**********************************************************************************************
        }
    }

}
