using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Views;
using POIApp.Common;

namespace POIApp
{
    [Activity(Label = "POIApp", MainLauncher = true, Icon = "@mipmap/icon")]
    public class POIListActivity : Activity
    {
        private ListView poiListView;
        private ProgressBar progressBar;
        private List<PointOfInterest> poiListData;
        private POIListViewAdapter poiListAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.POIList);
            poiListView = FindViewById<ListView>(Resource.Id.poiListView);
            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);

            DownloadPoisListAsync();
        }

        private async void DownloadPoisListAsync()
        {
            progressBar.Visibility = ViewStates.Visible;
            poiListData = await new POIService().GetPOIListAsync();
            progressBar.Visibility = ViewStates.Gone;

            poiListAdapter = new POIListViewAdapter(this, poiListData);
            poiListView.Adapter = poiListAdapter;
        }
    }
}

