
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using POIApp.Common;

namespace POIApp
{
    [Activity(Label = "POIDetailActivity")]
    public class POIDetailActivity : Activity
    {
        private Pois _poi;
        private EditText _nameEditText;
        private EditText _descrEditText;
        private EditText _addrEditText;
        private EditText _latEditText;
        private EditText _longEditText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.POIDetail);
            _nameEditText = FindViewById<EditText>(Resource.Id.nameEditText);
            _descrEditText = FindViewById<EditText>(Resource.Id.descrEditText);
            _addrEditText = FindViewById<EditText>(Resource.Id.addrEditText);
            _latEditText = FindViewById<EditText>(Resource.Id.latEditText);
            _longEditText = FindViewById<EditText>(Resource.Id.longEditText);

            if (Intent.HasExtra("poi"))
            {
                string poiJson = Intent.GetStringExtra("poi");
                _poi = JsonConvert.DeserializeObject<Pois>(poiJson);
            }
            else
            {
                _poi = new Pois();
            }

            UpdateUI();
        }

        protected void UpdateUI()
        {
            _nameEditText.Text = _poi.Name;
            _descrEditText.Text = _poi.Description;
            _addrEditText.Text = _poi.Address;
            _latEditText.Text = _poi.Latitude.ToString();
            _longEditText.Text = _poi.Longitude.ToString();
        }
    }
}
