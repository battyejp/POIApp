using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;

namespace POIApp.Common
{
    public class POIService
    {
        private const string GET_POIS = "http://private-e451d-poilist.apiary-mock.com/com.packt.poiapp/api/poi/pois";
        private const string CREATE_POI = "http://private-e451d-poilist.apiary-mock.com/com.packt.poiapp/api/poi/create";
        private const string DELETE_POI = "http://private-e451d-poilist.apiary-mock.com/com.packt.poiapp/api/poi/delete//{0}";

        public async Task<PointOfInterestList> GetPOIListAsync()
        {
            PointOfInterestList result;
            try
            {
                result = await GET_POIS.GetJsonAsync<PointOfInterestList>();
            }
            catch (Exception ex)
            {
                //TO DO log to remote logging service
                Console.Out.WriteLine("Failed to fetch data. Try again later!");
                return null;
            }

            return result;
        }

        public async Task<String> CreateOrUpdatePOIAsync(Pois poi)
        {
            string result;
            try
            {
                result = await CREATE_POI.PostUrlEncodedAsync(poi).ReceiveString();
            }
            catch (Exception ex)
            {
                //TO DO log to remote logging service
                Console.Out.WriteLine("Failed to update date. Try again later!");
                return null;
            }

            return result;
        }

        public async Task<String> DeletePOIAsync(int poiId)
        {
            string url = string.Format(DELETE_POI, poiId);
            string result;
            try
            {
                result = await url.DeleteAsync().ReceiveString();
            }
            catch (Exception ex)
            {
                //TO DO log to remote logging service
                Console.Out.WriteLine("Failed to delete data. Try again later!");
                return null;
            }

            return result;
        }
    }
}
