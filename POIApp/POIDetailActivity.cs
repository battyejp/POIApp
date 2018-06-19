
using Android.App;
using Android.OS;
using POIApp.Common;

namespace POIApp
{
    [Activity(Label = "POIDetailActivity")]
    public class POIDetailActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.POIDetail);

            var detailFragment = new POIDetailFragment
            {
                Arguments = new Bundle()
            };

            if (Intent.HasExtra("poi"))
            {
                string poiJson = Intent.GetStringExtra("poi");
                detailFragment.Arguments.PutString("poi", poiJson);
            }

            FragmentTransaction ft = FragmentManager.BeginTransaction();
            ft.Add(Resource.Id.poiDetailLayout, detailFragment);
            ft.Commit();
        }
    }
}
