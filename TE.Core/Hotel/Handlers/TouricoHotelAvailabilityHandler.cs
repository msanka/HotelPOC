
using System;
using TE.Core.Tourico.Hotel.Dtos;
using TE.Core.ServiceCatalogues.HotelCatalog.Enums;
using TE.ThirdPartyProviders.Tourico.TouricoHotelSvc;

namespace TE.Core.Tourico.Hotel.Handler
{
    public class TouricoHotelAvailabilityHandler : TouricoHandlerBase<HotelAvailabilityProviderReq, HotelAvailabilityProviderRes>
    {
        public override HotelAvailabilityProviderRes Execute(HotelAvailabilityProviderReq request)
        {
            if (request.LocationType == LocationTypes.Unknown)
            {
                throw new ArgumentNullException(nameof(request.LocationType));
            }
            if (request.CheckInDate <= DateTime.Today)
            {
                throw new ArgumentOutOfRangeException(nameof(request.CheckInDate));
            }
            if (request.CheckInDate >= request.CheckOutDate)
            {
                throw new ArgumentOutOfRangeException(nameof(request.CheckOutDate));
            }
            if (request.TotalAdults < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(request.TotalAdults));
            }

            //convert request dto to Tourico search request
            SearchRequest TouricoHotelRequest = ConvertToTouricoHotelSearchRequest(request);

            HotelAvailabilityProviderRes hotelSearchResults;

            //Call worker here:
            TouricoWorker tworker = new TouricoWorker();
            SearchResult resp = tworker.Execute(TouricoHotelRequest);

            //convert response to HotelAvailabilityProviderRes
            hotelSearchResults = this.ConvertToProviderResponse(resp, request);

            return hotelSearchResults;
        }

        private HotelAvailabilityProviderRes ConvertToProviderResponse(SearchResult touricoResponse, HotelAvailabilityProviderReq request)
        {
            throw new NotImplementedException();
        }

        private SearchRequest ConvertToTouricoHotelSearchRequest(HotelAvailabilityProviderReq request)
        {
            throw new NotImplementedException();
        }
    }
}
