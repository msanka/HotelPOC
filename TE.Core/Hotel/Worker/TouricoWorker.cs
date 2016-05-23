using System;
using TE.ThirdPartyProviders.Tourico.TouricoHotelSvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TE.Core.Tourico.Hotel
{
    public class TouricoWorker : IDisposable
    {

        private HotelFlowClient _hotelflowClient;


        public TouricoWorker()
        {

        }
        
        public SearchResult Execute(SearchRequest sReq)
        {
            //build authorization header
            var authHeader = new AuthenticationHeader();

            //default features
            var features = new Feature[]{ };
            
            var sreq = this.Execute(authHeader, sReq, features);
  
            return sreq;
        }

        public SearchResult Execute(AuthenticationHeader authHeader, SearchRequest sReq, Feature[] features)
        {
            //instantiate HotelFlowClient
            _hotelflowClient = new HotelFlowClient();

            //search Hotels
            var sreq = _hotelflowClient.SearchHotels(authHeader, sReq, features);

            //close instance
            _hotelflowClient.Close();
                        
            return sreq;
        }

        public void Dispose()
        {
            _hotelflowClient = null;
        }
               

    }
}