using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TE.Core.Tourico.Hotel.Dtos;

namespace TE.Core.Tourico.Hotel.ServiceCatlog
{
    public interface IHotelSearchProvider
    {
        HotelAvailabilityProviderRes Search(HotelAvailabilityProviderReq request);

      //  IEnumerable<HotelRateProviderRes> RetrieveHotelRates(HotelPropertyProviderReq request);

       // HotelRateProviderRes RetrieveHotelRateDetails(HotelRateProviderReq request);

    }
}
