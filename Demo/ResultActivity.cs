// Created by Rofiq Setiawan (rofiqsetiawan@gmail.com)

#nullable enable

using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using CheeseBind;
using R = Demo.Resource;

namespace Demo
{
    [Register("id.karamunting.countrypickerdemo.ResultActivity")]
    [Activity]
    public class ResultActivity : AppCompatActivity
    {
        public const string BundleKeyCountryName = "country_name";
        public const string BundleKeyCountryIso = "country_iso";
        public const string BundleKeyCountryDialCode = "dial_code";
        public const string BundleKeyCountryCurrency = "currency";
        public const string BundleKeyCountryFlagImage = "flag_image";

#pragma warning disable 649

        [BindView(R.Id.selected_country_name_text_view)]
        private TextView _countryNameTextView;

        [BindView(R.Id.selected_country_iso_text_view)]
        private TextView _countryIsoCodeTextView;

        [BindView(R.Id.selected_country_dial_code_text_view)]
        private TextView _countryDialCodeTextView;

        [BindView(R.Id.selected_country_currency)]
        private TextView _selectedCountryCurrency;

        [BindView(R.Id.selected_country_flag_image_view)]
        private ImageView _countryFlagImageView;

#pragma warning restore 649

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(R.Layout.activity_result);

            if (SupportActionBar != null)
                SupportActionBar.Title = "";

            Cheeseknife.Bind(this);

            FindViewById<ImageButton>(R.Id.back_button)!.Click += (s, e) => Finish();

            ShowData();
        }

        private void ShowData()
        {
            var bundle = Intent?.Extras;
            if (bundle == null) return;

            _countryNameTextView.Text = bundle.GetString(BundleKeyCountryName);
            _countryIsoCodeTextView.Text = bundle.GetString(BundleKeyCountryIso);
            _countryDialCodeTextView.Text = bundle.GetString(BundleKeyCountryDialCode);
            _selectedCountryCurrency.Text = bundle.GetString(BundleKeyCountryCurrency);
            _countryFlagImageView.SetImageResource(bundle.GetInt(BundleKeyCountryFlagImage));
        }
    }
}