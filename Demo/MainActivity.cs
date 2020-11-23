// Created by Rofiq Setiawan (rofiqsetiawan@gmail.com)

#nullable enable

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using CheeseBind;
using Com.Mukesh.CountryPickerLib;
using Com.Mukesh.CountryPickerLib.Listeners;
using AlertDialog = AndroidX.AppCompat.App.AlertDialog;
using Locale = Java.Util.Locale;
using R = Demo.Resource;

namespace Demo
{
    [Register("id.karamunting.countrypickerdemo.MainActivity")]
    [Activity(MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnCountryPickerListener
    {
#pragma warning disable 649

        [BindView(R.Id.country_picker_button)]
        private Button _pickCountryButton;

        [BindView(R.Id.by_name_button)]
        private Button _findByNameButton;

        [BindView(R.Id.by_sim_button)]
        private Button _findBySimButton;

        [BindView(R.Id.by_local_button)]
        private Button _findByLocaleButton;

        [BindView(R.Id.by_iso_button)]
        private Button _findByIsoButton;

        [BindView(R.Id.theme_toggle_switch)]
        private SwitchCompat _themeSwitch;

        [BindView(R.Id.custom_style_toggle_switch)]
        private SwitchCompat _styleSwitch;

        [BindView(R.Id.bottom_sheet_switch)]
        private SwitchCompat _useBottomSheet;

        [BindView(R.Id.search_switch)]
        private SwitchCompat _searchSwitch;

        [BindView(R.Id.sort_by_radio_group)]
        private RadioGroup _sortByRadioGroup;

#pragma warning restore 649

        private CountryPicker _countryPicker;
        private int _sortBy = CountryPicker.SortByNone;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(R.Layout.activity_main);

            Cheeseknife.Bind(this);

            SetListener();
        }

        private void SetListener()
        {
            _findByNameButton.Click += (s, e) => FindByName();
            _findBySimButton.Click += (s, e) => FindBySim();
            _findByLocaleButton.Click += (s, e) => FindByLocale();
            _findByIsoButton.Click += (s, e) => FindByIson();

            _sortByRadioGroup.CheckedChange += (s, e) =>
            {
                _sortBy = e.CheckedId switch
                {
                    R.Id.none_radio_button => CountryPicker.SortByNone,
                    R.Id.name_radio_button => CountryPicker.SortByName,
                    R.Id.dial_code_radio_button => CountryPicker.SortByDialCode,
                    R.Id.iso_radio_button => CountryPicker.SortByIso,
                    _ => CountryPicker.SortByNone,
                };
            };

            _pickCountryButton.Click += (s, e) => ShowPicker();
        }

        private void FindByIson()
        {
            _countryPicker = new CountryPicker.Builder().With(this).Listener(this).Build();
            var builder = new AlertDialog.Builder(this, R.Style.Base_Theme_MaterialComponents_Dialog_Alert);
            var input = new EditText(this)
            {
                InputType = InputTypes.ClassText | InputTypes.TextVariationPassword
            };
            builder.SetTitle("Country Code")
                .SetView(input)
                .SetCancelable(false)
                .SetPositiveButton(
                    textId: Android.Resource.String.Ok,
                    handler: (sender, args) =>
                    {
                        var country = _countryPicker.GetCountryByISO(input.Text);
                        if (country == null)
                            ShowToast("Country not found");
                        else
                            ShowResultActivity(country);
                    }
                ).SetNegativeButton(
                    textId: Android.Resource.String.Cancel,
                    handler: (sender, args) => (sender as IDialogInterface)?.Cancel()
                ).Show();
        }

        private void FindByLocale()
        {
            _countryPicker = new CountryPicker.Builder().With(this).Listener(this).Build();
            var country = _countryPicker.GetCountryByLocale(Locale.Default);
            if (country == null)
                ShowToast("Country not found");
            else
                ShowResultActivity(country);
        }

        private void FindBySim()
        {
            _countryPicker = new CountryPicker.Builder().With(this).Listener(this).Build();
            var country = _countryPicker.CountryFromSIM;
            if (country == null)
                ShowToast("Country not found");
            else
                ShowResultActivity(country);
        }

        private void FindByName()
        {
            _countryPicker = new CountryPicker.Builder().With(this).Listener(this).Build();
            var builder = new AlertDialog.Builder(this, R.Style.Base_Theme_MaterialComponents_Dialog_Alert);
            var input = new EditText(this)
            {
                InputType = InputTypes.ClassText | InputTypes.TextVariationPassword
            };

            builder.SetTitle("Country Name")
                .SetView(input)
                .SetCancelable(false)
                .SetPositiveButton(
                    textId: Android.Resource.String.Ok,
                    handler: (sender, args) =>
                    {
                        var country = _countryPicker.GetCountryByName(input.Text);
                        if (country == null)
                            ShowToast("Country not found");
                        else
                            ShowResultActivity(country);
                    }
                ).SetNegativeButton(
                    textId: Android.Resource.String.Cancel,
                    handler: (sender, args) => (sender as IDialogInterface)?.Cancel()
                ).Show();
        }

        private void ShowToast(string msg) => Toast.MakeText(this, msg, ToastLength.Short)!.Show();

        private void ShowResultActivity(Country country)
        {
            using var i = new Intent(this, typeof(ResultActivity));
            i.PutExtra(ResultActivity.BundleKeyCountryName, country.Name);
            i.PutExtra(ResultActivity.BundleKeyCountryCurrency, country.Currency);
            i.PutExtra(ResultActivity.BundleKeyCountryDialCode, country.DialCode);
            i.PutExtra(ResultActivity.BundleKeyCountryIso, country.Code);
            i.PutExtra(ResultActivity.BundleKeyCountryFlagImage, country.Flag);
            StartActivity(i);
        }

        private void ShowPicker()
        {
            var builder = new CountryPicker.Builder().With(this).Listener(this);
            if (_styleSwitch.Checked)
                builder.Style(R.Style.CountryPickerStyle);

            builder.Theme(_themeSwitch.Checked ? CountryPicker.ThemeNew : CountryPicker.ThemeOld)
                .CanSearch(_searchSwitch.Checked)
                .SortBy(_sortBy);

            _countryPicker = builder.Build();

            if (_useBottomSheet.Checked)
                _countryPicker.ShowBottomSheet(this);
            else
                _countryPicker.ShowDialog(this);
        }

        public void OnSelectCountry(Country country) => ShowResultActivity(country);
    }
}