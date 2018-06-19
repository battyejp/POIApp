
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using POIApp.Common;

namespace POIApp
{
    public class POIListFragment : ListFragment
    {
        private ProgressBar progressBar;
        private PointOfInterestList poiListData;
        private POIListViewAdapter poiListAdapter;
        private Activity activity;

        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
            this.activity = activity;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.POIListFragment, container, false);

            progressBar = view.FindViewById<ProgressBar>(Resource.Id.progressBar);

            SetHasOptionsMenu(true);

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();
            DownloadPoisListAsync();
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            // Inflate menus to be shown on ActionBar
            inflater.Inflate(Resource.Menu.POIListViewMenu, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.actionNew:
                    Intent intent = new Intent(activity, typeof(POIDetailActivity));
                    StartActivity(intent);
                    return true;
                case Resource.Id.actionRefresh:
                    DownloadPoisListAsync();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            Pois poi = poi = poiListData.Pois[position];

            Intent poiDetailIntent = new Intent(activity, typeof(POIDetailActivity));
            string poiJson = JsonConvert.SerializeObject(poi);
            poiDetailIntent.PutExtra("poi", poiJson);
            StartActivity(poiDetailIntent);
        }

        private async void DownloadPoisListAsync()
        {
            if (!IsConnected())
            {
                Toast toast = Toast.MakeText(activity, "Not conntected to internet. Please check your device network settings.", ToastLength.Short);
                toast.Show();
                return;
            }

            progressBar.Visibility = ViewStates.Visible;
            poiListData = await new POIService().GetPOIListAsync();
            progressBar.Visibility = ViewStates.Gone;

            poiListAdapter = new POIListViewAdapter(activity, poiListData.Pois);
            this.ListAdapter = poiListAdapter;
        }

        private bool IsConnected()
        {
            var connectivityManager = (ConnectivityManager)activity.GetSystemService(Context.ConnectivityService);
            var activeConnection = connectivityManager.ActiveNetworkInfo;
            return (null != activeConnection && activeConnection.IsConnected);
        }
    }
}
