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
    }
}
