using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;

namespace POIApp
{
    public class POIListViewAdapter : BaseAdapter<PointOfInterest>
    {
        private readonly Activity context;
        private List<PointOfInterest> poiListData;

        public POIListViewAdapter(Activity _context, List<PointOfInterest> _poiListData)
             : base()
        {
            this.context = _context;
            this.poiListData = _poiListData;
        }

        public override PointOfInterest this[int index]
        {
            get
            {
                return poiListData[index];
            }
        }

        public override int Count
        {
            get
            {
                return poiListData.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;

            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.POIListItem, null);
            }

            PointOfInterest poi = this[position];
            view.FindViewById<TextView>(Resource.Id.nameTextView).Text = poi.Name;

            if (string.IsNullOrEmpty(poi.Address))
            {
                view.FindViewById<TextView>(Resource.Id.addrTextView).Visibility = ViewStates.Gone;
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.addrTextView).Text = poi.Address;
            }

            /*var imageView = view.FindViewById<ImageView>(Resource.Id.poiImageView);
            if (!string.IsNullOrEmpty(poi.Address))
            {
                Koush.UrlImageViewHelper.SetUrlDrawable(imageView, poi.Image, Resource.Drawable.ic_placeholder);
            }*/
                 

            return view;

        }
    }
}
