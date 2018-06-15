using Android.App;
using Android.Widget;
using Android.OS;

namespace POIApp
{
    [Activity(Label = "POIApp", MainLauncher = true, Icon = "@mipmap/icon")]
    public class POIListActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.POIList);
        }
    }
}

