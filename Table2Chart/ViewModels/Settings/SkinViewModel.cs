using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Table2Chart.Common;
using Table2Chart.Common.MVVM;
using Table2Chart.Extensions;

namespace Table2Chart.ViewModels.Settings
{
    /// <summary>
    /// 皮肤个性化的ViewModel
    /// </summary>
    public class SkinViewModel : NavigationViewModel
    {
        public SkinViewModel(IContainerProvider containerProvider) : base(containerProvider)
        {
            ITheme theme = _paletteHelper.GetTheme();
            BaseTheme baseTheme = theme.GetBaseTheme();
            IsDarkTheme = baseTheme == BaseTheme.Dark;
            _primaryColor = theme.PrimaryMid.Color;
            _secondaryColor = theme.SecondaryMid.Color;
            SelectedColor = _primaryColor;

            if (theme is Theme internalTheme)
            {
                _isColorAdjusted = internalTheme.ColorAdjustment != null;
                var colorAdjustment = internalTheme.ColorAdjustment ?? new ColorAdjustment();
                _desiredContrastRatio = colorAdjustment.DesiredContrastRatio;
                _contrastValue = colorAdjustment.Contrast;
                _colorSelectionValue = colorAdjustment.Colors;
            }

            var themeManager = _paletteHelper.GetThemeManager();
            if (themeManager != null)
            {
                themeManager.ThemeChanged += (_, e) =>
                {
                    IsDarkTheme = e.NewTheme?.GetBaseTheme() == BaseTheme.Dark;
                };
            }
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
            var theme = _paletteHelper.GetTheme() as Theme;
            //主题
            JsonConfigHelper.SaveConfig(theme, JsonConfigHelper.ConfigFile.SkinConfig);
        }

        private readonly PaletteHelper _paletteHelper = new PaletteHelper();
        public IEnumerable<ISwatch> Swatches { get; } = SwatchHelper.Swatches;

        private Color? _primaryColor;

        private Color? _secondaryColor;

        private Color? _primaryForegroundColor;

        private Color? _secondaryForegroundColor;

        private bool _IsDarkTheme = false;

        public bool IsDarkTheme
        {
            get => _IsDarkTheme;
            set
            {
                if (SetProperty(ref _IsDarkTheme, value))
                {
                    ModifyTheme(theme => theme.SetBaseTheme(value ? Theme.Dark : Theme.Light));
                }
            }
        }

        private bool _isColorAdjusted;

        public bool IsColorAdjusted
        {
            get => _isColorAdjusted;
            set
            {
                if (SetProperty(ref _isColorAdjusted, value))
                {
                    ModifyTheme(theme =>
                    {
                        if (theme is Theme internalTheme)
                        {
                            internalTheme.ColorAdjustment = value
                                ? new ColorAdjustment
                                {
                                    DesiredContrastRatio = DesiredContrastRatio,
                                    Contrast = ContrastValue,
                                    Colors = ColorSelectionValue
                                }
                                : null;
                        }
                    });
                }
            }
        }

        private float _desiredContrastRatio = 4.5f;

        public float DesiredContrastRatio
        {
            get => _desiredContrastRatio;
            set
            {
                if (SetProperty(ref _desiredContrastRatio, value))
                {
                    ModifyTheme(theme =>
                    {
                        if (theme is Theme internalTheme && internalTheme.ColorAdjustment != null)
                            internalTheme.ColorAdjustment.DesiredContrastRatio = value;
                    });
                }
            }
        }

        public IEnumerable<Contrast> ContrastValues => Enum.GetValues(typeof(Contrast)).Cast<Contrast>();

        private Contrast _contrastValue;

        public Contrast ContrastValue
        {
            get => _contrastValue;
            set
            {
                if (SetProperty(ref _contrastValue, value))
                {
                    ModifyTheme(theme =>
                    {
                        if (theme is Theme internalTheme && internalTheme.ColorAdjustment != null)
                            internalTheme.ColorAdjustment.Contrast = value;
                    });
                }
            }
        }

        public IEnumerable<ColorSelection> ColorSelectionValues => Enum.GetValues(typeof(ColorSelection)).Cast<ColorSelection>();

        private ColorSelection _colorSelectionValue;

        public ColorSelection ColorSelectionValue
        {
            get => _colorSelectionValue;
            set
            {
                if (SetProperty(ref _colorSelectionValue, value))
                {
                    ModifyTheme(theme =>
                    {
                        if (theme is Theme internalTheme && internalTheme.ColorAdjustment != null)
                            internalTheme.ColorAdjustment.Colors = value;
                    });
                }
            }
        }

        private static void ModifyTheme(Action<ITheme> modificationAction)
        {
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();

            modificationAction?.Invoke(theme);

            paletteHelper.SetTheme(theme);
        }

        private ColorScheme _activeScheme;

        public ColorScheme ActiveScheme
        {
            get => _activeScheme;
            set
            {
                if (_activeScheme != value)
                {
                    _activeScheme = value;
                    RaisePropertyChanged();
                }
            }
        }

        private Color? _selectedColor;

        public Color? SelectedColor
        {
            get => _selectedColor;
            set
            {
                if (_selectedColor != value)
                {
                    _selectedColor = value;
                    RaisePropertyChanged();

                    // if we are triggering a change internally its a hue change and the colors will match
                    // so we don't want to trigger a custom color change.
                    Color? currentSchemeColor;

                    switch (ActiveScheme)
                    {
                        case ColorScheme.Primary:
                            currentSchemeColor = _primaryColor;
                            break;

                        case ColorScheme.Secondary:
                            currentSchemeColor = _secondaryColor;
                            break;

                        case ColorScheme.PrimaryForeground:
                            currentSchemeColor = _primaryForegroundColor;
                            break;

                        case ColorScheme.SecondaryForeground:
                            currentSchemeColor = _secondaryForegroundColor;
                            break;

                        default:
                            throw new NotSupportedException($"{ActiveScheme} is not a handled ColorScheme.. Ye daft programmer!");
                    }

                    if (_selectedColor != currentSchemeColor && value is Color color)
                    {
                        ChangeCustomColor(color);
                    }
                }
            }
        }

        private DelegateCommand<object> _ChangeCustomHueCommand;

        public DelegateCommand<object> ChangeCustomHueCommand =>
            _ChangeCustomHueCommand ?? (_ChangeCustomHueCommand = new DelegateCommand<object>(ChangeCustomColor));

        private DelegateCommand _ChangeToPrimaryCommand;

        public DelegateCommand ChangeToPrimaryCommand =>
            _ChangeToPrimaryCommand ?? (_ChangeToPrimaryCommand = new DelegateCommand(() => ChangeScheme(ColorScheme.Primary)));

        private DelegateCommand _ChangeToSecondaryCommand;

        public DelegateCommand ChangeToSecondaryCommand =>
            _ChangeToSecondaryCommand ?? (_ChangeToSecondaryCommand = new DelegateCommand(() => ChangeScheme(ColorScheme.Secondary)));

        private DelegateCommand _ChangeToPrimaryForegroundCommand;

        public DelegateCommand ChangeToPrimaryForegroundCommand =>
            _ChangeToPrimaryForegroundCommand ?? (_ChangeToPrimaryForegroundCommand = new DelegateCommand(() => ChangeScheme(ColorScheme.PrimaryForeground)));

        private DelegateCommand _ChangeToSecondaryForegroundCommand;

        public DelegateCommand ChangeToSecondaryForegroundCommand =>
            _ChangeToSecondaryForegroundCommand ?? (_ChangeToSecondaryForegroundCommand = new DelegateCommand(() => ChangeScheme(ColorScheme.SecondaryForeground)));

        private DelegateCommand<bool?> _ToggleBaseCommand;

        public DelegateCommand<bool?> ToggleBaseCommand =>
            _ToggleBaseCommand ?? (_ToggleBaseCommand = new DelegateCommand<bool?>(o => ApplyBase((bool)o)));

        private DelegateCommand<object> _ChangeHueCommand;

        public DelegateCommand<object> ChangeHueCommand =>
            _ChangeHueCommand ?? (_ChangeHueCommand = new DelegateCommand<object>(ChangeHue));

        private void ChangeHue(object obj)
        {
            var hue = (Color)obj;

            SelectedColor = hue;
            if (ActiveScheme == ColorScheme.Primary)
            {
                _paletteHelper.ChangePrimaryColor(hue);
                _primaryColor = hue;
                _primaryForegroundColor = _paletteHelper.GetTheme().PrimaryMid.GetForegroundColor();
            }
            else if (ActiveScheme == ColorScheme.Secondary)
            {
                _paletteHelper.ChangeSecondaryColor(hue);
                _secondaryColor = hue;
                _secondaryForegroundColor = _paletteHelper.GetTheme().SecondaryMid.GetForegroundColor();
            }
            else if (ActiveScheme == ColorScheme.PrimaryForeground)
            {
                SetPrimaryForegroundToSingleColor(hue);
                _primaryForegroundColor = hue;
            }
            else if (ActiveScheme == ColorScheme.SecondaryForeground)
            {
                SetSecondaryForegroundToSingleColor(hue);
                _secondaryForegroundColor = hue;
            }
        }

        private void ApplyBase(bool isDark)
        {
            ITheme theme = _paletteHelper.GetTheme();
            IBaseTheme baseTheme = isDark ? new MaterialDesignDarkTheme() : new MaterialDesignLightTheme();
            theme.SetBaseTheme(baseTheme);
            _paletteHelper.SetTheme(theme);
        }

        private void ChangeScheme(ColorScheme scheme)
        {
            ActiveScheme = scheme;
            if (ActiveScheme == ColorScheme.Primary)
            {
                SelectedColor = _primaryColor;
            }
            else if (ActiveScheme == ColorScheme.Secondary)
            {
                SelectedColor = _secondaryColor;
            }
            else if (ActiveScheme == ColorScheme.PrimaryForeground)
            {
                SelectedColor = _primaryForegroundColor;
            }
            else if (ActiveScheme == ColorScheme.SecondaryForeground)
            {
                SelectedColor = _secondaryForegroundColor;
            }
        }

        private void ChangeCustomColor(object obj)
        {
            var color = (Color)obj;

            if (ActiveScheme == ColorScheme.Primary)
            {
                _paletteHelper.ChangePrimaryColor(color);
                _primaryColor = color;
            }
            else if (ActiveScheme == ColorScheme.Secondary)
            {
                _paletteHelper.ChangeSecondaryColor(color);
                _secondaryColor = color;
            }
            else if (ActiveScheme == ColorScheme.PrimaryForeground)
            {
                SetPrimaryForegroundToSingleColor(color);
                _primaryForegroundColor = color;
            }
            else if (ActiveScheme == ColorScheme.SecondaryForeground)
            {
                SetSecondaryForegroundToSingleColor(color);
                _secondaryForegroundColor = color;
            }
        }

        private void SetPrimaryForegroundToSingleColor(Color color)
        {
            ITheme theme = _paletteHelper.GetTheme();

            theme.PrimaryLight = new ColorPair(theme.PrimaryLight.Color, color);
            theme.PrimaryMid = new ColorPair(theme.PrimaryMid.Color, color);
            theme.PrimaryDark = new ColorPair(theme.PrimaryDark.Color, color);

            _paletteHelper.SetTheme(theme);
        }

        private void SetSecondaryForegroundToSingleColor(Color color)
        {
            ITheme theme = _paletteHelper.GetTheme();

            theme.SecondaryLight = new ColorPair(theme.SecondaryLight.Color, color);
            theme.SecondaryMid = new ColorPair(theme.SecondaryMid.Color, color);
            theme.SecondaryDark = new ColorPair(theme.SecondaryDark.Color, color);

            _paletteHelper.SetTheme(theme);
        }
    }

    public enum ColorScheme
    {
        Primary,
        Secondary,
        PrimaryForeground,
        SecondaryForeground
    }
}