using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TE.Core.Tourico.Hotel.Dtos;
using TE.Core.Tourico.Hotel.Handler;
using TE.Core.Tourico.Hotel.ServiceCatlog;

namespace TE.Tourico.Hotel
{
    class TouricoHotelSearchProvider : IHotelSearchProvider, IHotelDetailsProvider
    {
        public HotelInfo RetrieveHotelRates(string hotelCode)
        {
            throw new NotImplementedException();
        }

        public HotelAvailabilityProviderRes Search(HotelAvailabilityProviderReq request)
        {
            var response = new TouricoHotelAvailabilityHandler().Execute(request);
            return response;
        }
    }
}
