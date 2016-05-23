using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TE.Core.Tourico.Hotel;
using TE.ThirdPartyProviders.Tourico.TouricoHotelSvc;



namespace TouricoTest
{
    [TestClass]
    public class TouricoUnitTests
    {
        [TestMethod]
        public void PerformSearch()
        {
            //Arrange
            var authenticationHdr = new AuthenticationHeader();
            authenticationHdr.LoginName = "Tra105";
            authenticationHdr.Password = "111111";

            var searchRequest = new SearchRequest();
            searchRequest.Destination = "YTO";
            searchRequest.CheckIn = DateTime.Now.AddDays(5);
            searchRequest.CheckOut = DateTime.Now.AddDays(10);
            searchRequest.RoomsInformation =
                new RoomInfo[] {
                                                                new RoomInfo {
                                                                               AdultNum = 2
                                                                               //, ChildAges =  new ChildAge[] { new ChildAge { age = 5 } }
                                                                               //, ChildNum  = 1
                                                                }};

            var features = new Feature[] { new Feature { name = "OriginalImageSize", value = "true" } };

            //Act
            var touricoWorker = new TouricoWorker();
            SearchResult resp = touricoWorker.Execute(authenticationHdr, searchRequest, features);
            touricoWorker.Dispose();
                 

            //Assert
            Assert.IsTrue(resp.HotelList.GetLength(0) > 0);


        }

    }
}
