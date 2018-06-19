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
        public static bool isDualMode = false; 

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.ActionBar);

            SetContentView(Resource.Layout.POIList);

            var detailsLayout = FindViewById(Resource.Id.poiDetailLayout);

            isDualMode = detailsLayout != null && detailsLayout.Visibility == ViewStates.Visible;
        }
    }
}

