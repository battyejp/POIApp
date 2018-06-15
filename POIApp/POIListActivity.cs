using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Views;
using POIApp.Common;
using Android.Content;
using Android.Net;
using Newtonsoft.Json;

namespace POIApp
{
    [Activity(Label = "POIApp", MainLauncher = true, Icon = "@mipmap/icon")]
    public class POIListActivity : Activity
    {
        private ListView poiListView;
        private ProgressBar progressBar;
        private PointOfInterestList poiListData;
        private POIListViewAdapter poiListAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.ActionBar);

            SetContentView(Resource.Layout.POIList);
            poiListView = FindViewById<ListView>(Resource.Id.poiListView);
            poiListView.ItemClick += POIClicked;

            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);

            DownloadPoisListAsync();
        }

        private async void DownloadPoisListAsync()
        {
            if (!IsConnected())
            {
                Toast toast = Toast.MakeText(this, "Not conntected to internet. Please check your device network settings.", ToastLength.Short);
                toast.Show();
                return;
            }

            progressBar.Visibility = ViewStates.Visible;
            poiListData = await new POIService().GetPOIListAsync();
            progressBar.Visibility = ViewStates.Gone;

            poiListAdapter = new POIListViewAdapter(this, poiListData.Pois);
            poiListView.Adapter = poiListAdapter;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            // Inflate menus to be shown on ActionBar
            MenuInflater.Inflate(Resource.Menu.POIListViewMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.actionNew:
                    StartActivity(typeof(POIDetailActivity));
                    return true;
                case Resource.Id.actionRefresh:
                    DownloadPoisListAsync();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private bool IsConnected()
        {
            var connectivityManager = (ConnectivityManager)this.GetSystemService(Context.ConnectivityService);
            var activeConnection = connectivityManager.ActiveNetworkInfo;
            return (null != activeConnection && activeConnection.IsConnected);
        }

        protected void POIClicked(object sender, ListView.ItemClickEventArgs e)
        {
            Pois poi = poiListData.Pois[(int)e.Id];

            Intent poiDetailIntent = new Intent(this, typeof(POIDetailActivity));
            string poiJson = JsonConvert.SerializeObject(poi);
            poiDetailIntent.PutExtra("poi", poiJson);
            StartActivity(poiDetailIntent);
        }
    }
}

