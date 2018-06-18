
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net;
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
            RequestWindowFeature(WindowFeatures.ActionBar);

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
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.POIDetailMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.actionSave:
                    SavePOI();
                    return true;
                case Resource.Id.actionDelete:
                    DeletePOI();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            base.OnPrepareOptionsMenu(menu);
            // Disable delete for a new POI
            if (_poi.Id <= 0)
            {
                IMenuItem item = menu.FindItem(Resource.Id.actionDelete);
                //disable menu item
                item.SetEnabled(false);

                //hide menu items
                item.SetVisible(false);
            }
            return true;
        }

        protected void SavePOI()
        {
            bool errors = false;
            if (String.IsNullOrEmpty(_nameEditText.Text))
            {
                _nameEditText.Error = "Name cannot be empty";
                errors = true;
            }
            else
            {
                _nameEditText.Error = null;
            }


            double? tempLatitude = null;
            if (!String.IsNullOrEmpty(_latEditText.Text))
            {
                try
                {
                    tempLatitude = Double.Parse(_latEditText.Text);
                    if ((tempLatitude > 90) | (tempLatitude < -90))
                    {
                        _latEditText.Error = "Latitude must be a decimal value between -90 and 90";
                        errors = true;
                    }
                    else
                    {
                        _latEditText.Error = null;
                    }
                }
                catch
                {
                    _latEditText.Error = "Latitude must be valid decimal number";
                    errors = true;
                }
            }

            double? tempLongitude = null;
            if (!String.IsNullOrEmpty(_longEditText.Text))
            {
                try
                {
                    tempLongitude = Double.Parse(_longEditText.Text);
                    if ((tempLongitude > 180) | (tempLongitude < -180))
                    {
                        _longEditText.Error = "Longitude must be a decimal value between -180 and 180";
                        errors = true;
                    }
                    else
                    {
                        _longEditText.Error = null;
                    }
                }
                catch
                {
                    _longEditText.Error = "Longitude must be valid decimal number";
                    errors = true;
                }
            }

            if (errors)
            {
                return;
            }

            _poi.Name = _nameEditText.Text;
            _poi.Description = _descrEditText.Text;
            _poi.Address = _addrEditText.Text;
            //_poi.Latitude = tempLatitude;
            //_poi.Longitude = tempLongitude;

            CreateOrUpdatePOIAsync(_poi);
        }

        private async void CreateOrUpdatePOIAsync(Pois poi)
        {
            POIService service = new POIService();
            if (!IsConnected())
            {
                Toast toast = Toast.MakeText(this, "Not conntected to internet. Please check your device network settings.", ToastLength.Short);
                toast.Show();
                return;
            }

            string response = await service.CreateOrUpdatePOIAsync(_poi);
            if (!string.IsNullOrEmpty(response))
            {
                Toast toast = Toast.MakeText(this, String.Format("{0} saved.", _poi.Name), ToastLength.Short);
                toast.Show();
                Finish();
            }
            else
            {
                Toast toast = Toast.MakeText(this, "Something went Wrong!", ToastLength.Short);
                toast.Show();
            }
        }

        private bool IsConnected()
        {
            var connectivityManager = (ConnectivityManager)this.GetSystemService(Context.ConnectivityService);
            var activeConnection = connectivityManager.ActiveNetworkInfo;
            return (null != activeConnection && activeConnection.IsConnected);
        }

        private async void DeletePOIAsync()
        {
            POIService service = new POIService();
            if (!IsConnected())
            {
                Toast toast = Toast.MakeText(this, "Not conntected to internet. Please check your device network settings.", ToastLength.Short);
                toast.Show();
                return;
            }

            string response = await service.DeletePOIAsync(_poi.Id);
            if (!string.IsNullOrEmpty(response))
            {
                Toast toast = Toast.MakeText(this, String.Format("{0} deleted.", _poi.Name), ToastLength.Short);
                toast.Show();

                Finish();
            }
            else
            {
                Toast toast = Toast.MakeText(this, "Something went Wrong!", ToastLength.Short);
                toast.Show();
            }
        }

        protected void DeletePOI()
        {
            AlertDialog.Builder alertConfirm = new AlertDialog.Builder(this);
            alertConfirm.SetTitle("Confirm delete");
            alertConfirm.SetCancelable(false);
            alertConfirm.SetPositiveButton("OK", ConfirmDelete);
            alertConfirm.SetNegativeButton("Cancel", delegate { });
            alertConfirm.SetMessage(String.Format("Are you sure you want to delete {0}?", _poi.Name));
            alertConfirm.Show();

        }

        protected void ConfirmDelete(object sender, EventArgs e)
        {
            DeletePOIAsync();
        }
    }
}
