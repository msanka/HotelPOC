﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.hotelflowSvc;
using Common.hotelflowReservationSvc;
using System.Text.RegularExpressions;
using System.Globalization;

namespace UI.Controllers
{
    public class BookTouricoController : Controller
    {
        //
        // GET: /BookTourico/

        public ActionResult Index(FormCollection collection)
        {

           return View();
        }

        public ActionResult SearchTourico()
        {
            //AuthHeader
            Common.hotelflowSvc.AuthenticationHeader auth = new AuthenticationHeader();
            auth.LoginName = "Tra105";
            auth.Password = "111111";

            FormCollection collection = TempData["col"] as FormCollection;

            //SearchHotelRequest
            Common.hotelflowSvc.SearchRequest reqCriteria = new SearchRequest();
            reqCriteria.CheckIn = Convert.ToDateTime(collection["checkIn"]);
            reqCriteria.CheckOut = Convert.ToDateTime(collection["checkOut"]);
            reqCriteria.Destination = "YTO";// collection["add"];

            
            reqCriteria.RoomsInformation = new RoomInfo[] {
                                                                new RoomInfo {
                                                                               AdultNum = Convert.ToInt16(collection["ddlTotalGuest"]),
                                                                               ChildAges =  new ChildAge[] { new ChildAge { age = 5 } },
                                                                               ChildNum  = 1
                                                                }};

            HotelFlowClient client = new HotelFlowClient();
            SearchResult sreq = client.SearchHotels(auth, reqCriteria, new Feature[] { new Feature { name = "OriginalImageSize", value = "true" } });

          

            ViewBag.StartDate = reqCriteria.CheckIn;
            ViewBag.EndDate = reqCriteria.CheckOut;
            ViewBag.Adults = Convert.ToInt16(collection["ddlTotalGuest"]);

            ViewBag.Lat = double.Parse(collection["lat"]);
            ViewBag.Lan = double.Parse(collection["lan"]);


            return View("SearchHotel", sreq.HotelList);
        }

        //NOTE: skipped using this method in POC - GetHotelDetailsV3 could be combined with CheckAvailabilityAndPrices to show more details
        public ActionResult GetHotelInfo(FormCollection collection)
        {
            //AuthHeader
            Common.hotelflowSvc.AuthenticationHeader auth = new AuthenticationHeader();
            auth.LoginName = "Tra105";
            auth.Password = "111111";

            var hotelIds = new HotelID[] { new HotelID { id =  Convert.ToInt32(collection["hotelCode"]) } };
          
            HotelFlowClient client = new HotelFlowClient();
            TWS_HotelDetailsV3 sreq = client.GetHotelDetailsV3(auth, hotelIds, new Feature[] { new Feature { name = "OriginalImageSize", value = "true" } });
            client.Close();

            ViewBag.StartDate = collection["startDate"];
            ViewBag.EndDate = collection["endDate"];

            //return View("HotelInfo", sreq);         
            return View("HotelDisplay", sreq);

        }


        // public ActionResult CheckAvailability(int hotelId)
        public ActionResult CheckAvailability(FormCollection collection)
        {
            //AuthHeader
            Common.hotelflowSvc.AuthenticationHeader auth = new AuthenticationHeader();
            auth.LoginName = "Tra105";
            auth.Password = "111111";

            //SearchHotelRequest
            Common.hotelflowSvc.SearchHotelsByIdRequest reqCriteria = new SearchHotelsByIdRequest();
            reqCriteria.HotelIdsInfo = new HotelIdInfo[] {
                                                    new HotelIdInfo { id = Convert.ToInt32(collection["hotelCode"]) }
            };

            reqCriteria.CheckIn =  Convert.ToDateTime(collection["startDate"]);
            reqCriteria.CheckOut = Convert.ToDateTime(collection["endDate"]);

            ViewBag.StartDate = reqCriteria.CheckIn;
            ViewBag.EndDate = reqCriteria.CheckOut;

            reqCriteria.RoomsInformation = new RoomInfo[] {
                                                                new RoomInfo {
                                                                               AdultNum = 1,
                                                                               ChildAges =  new ChildAge[] { new ChildAge { age = 5 } },
                                                                               ChildNum  = 1
                                                                }};

            HotelFlowClient client = new HotelFlowClient();
            SearchResult sreq = client.CheckAvailabilityAndPrices(auth, reqCriteria, new Feature[] { new Feature { name = "OriginalImageSize", value = "true" } });
            client.Close();
           
            
            //sreq.HotelList[0].RoomTypes[0].Occupancies[0]
            
            return View("Available", sreq.HotelList);

        }


        // public ActionResult BookHotel(int hotelId,int hotelroomTypeId, Decimal occPubPrice)
        public ActionResult BookHotel(FormCollection collection)
        {
            //AuthHeader
            Common.hotelflowSvc.AuthenticationHeader auth = new AuthenticationHeader();
            auth.LoginName = "Tra105";
            auth.Password = "111111";

            //SearchHotelRequest
            Common.hotelflowSvc.BookV3Request reqCriteria = new BookV3Request();
            reqCriteria.RecordLocatorId = 0;
            reqCriteria.HotelId = Convert.ToInt32(collection["hotelCode"]); 
            reqCriteria.HotelRoomTypeId = Convert.ToInt32(collection["hotelroomTypeId"]);

            reqCriteria.CheckIn = Convert.ToDateTime(collection["startDate"]);
            reqCriteria.CheckOut = Convert.ToDateTime(collection["endDate"]);

            reqCriteria.RoomsInfo = new RoomReserveInfo[]
            {
                new RoomReserveInfo {  AdultNum = 1,
                                       ChildAges =  new ChildAge[] { new ChildAge { age = 5 } },
                                       ChildNum  = 1 ,
                                       ContactPassenger = new ContactPassenger { FirstName = "Jack", LastName = "Rowling" }
                                    }
            };

            reqCriteria.Currency = "CAD";             
            reqCriteria.PaymentType = PaymentTypes.Obligo;
            reqCriteria.RequestedPrice = Convert.ToDecimal(collection["occPubPrice"]);

            ViewBag.HotelId = reqCriteria.HotelId;
            ViewBag.HotelRoomTypeId = reqCriteria.HotelRoomTypeId;

            HotelFlowClient client = new HotelFlowClient();

            RGInfoResults sreq;

            try
            {

                ////Cancellation Policies
                //HotelPolicyType1 cancelresp = client.GetCancellationPolicies(AuthenticationHeader: auth, nResId: 0, hotelId: 16455, hotelRoomTypeId: 14411, productId: 0, dtCheckIn: Convert.ToDateTime("2016-05-25"), dtCheckOut: Convert.ToDateTime("2016 - 05 - 28"));
                //RoomTypePolicyType1 pol = cancelresp.RoomTypePolicy;
                //pol.CancelPolicy[].PenaltyDescription;
                //client.Close();
                //CostAmendRequest k = new CostAmendRequest;
                //AmendRequest r = new AmendRequest();

                //ReservationsServiceSoapClient clnt = new ReservationsServiceSoapClient();
                
                
                sreq = client.BookHotelV3(AuthenticationHeader: auth,request: reqCriteria, features: new Feature[] { new Feature { name = "OriginalImageSize", value = "true" } });
                
            }
            catch (System.ServiceModel.FaultException ex)
            {
                return View("PriceChange", ex);                   
            }
            finally
            {
                client.Close();
            }

            return View("Confirmation", sreq.ResGroup);

            

        }

        public static Decimal ExtractDecimalFromString(string str)
        {

            Regex digits = new Regex(@"^\D*?((-?(\d+(\.\d+)?))|(-?\.\d+)).*");
            Match mx = digits.Match(str);
            //Console.WriteLine("Input {0} - Digits {1} {2}", str, mx.Success, mx.Groups);

            return mx.Success ? Convert.ToDecimal(mx.Groups[1].Value) : 0;
        }

        public ActionResult BookHotelFinal(FormCollection collection)
        {
            //AuthHeader
            Common.hotelflowSvc.AuthenticationHeader auth = new AuthenticationHeader();
            auth.LoginName = "Tra105";
            auth.Password = "111111";

            //SearchHotelRequest
            Common.hotelflowSvc.BookV3Request reqCriteria = new BookV3Request();
            reqCriteria.RecordLocatorId = 0;
            reqCriteria.HotelId = Convert.ToInt32(collection["hotelCode"]);
            reqCriteria.HotelRoomTypeId = Convert.ToInt32(collection["hotelroomTypeId"]);

            reqCriteria.CheckIn = Convert.ToDateTime("2016-04-22"); // Convert.ToDateTime(collection["checkIn"]);
            reqCriteria.CheckOut = Convert.ToDateTime("2016-04-24");//Convert.ToDateTime(collection["checkOut"]);


            reqCriteria.RoomsInfo = new RoomReserveInfo[]
            {
                new RoomReserveInfo {  AdultNum = 1,
                                       ChildAges =  new ChildAge[] { new ChildAge { age = 5 } },
                                       ChildNum  = 1 ,
                                       ContactPassenger = new ContactPassenger { FirstName = "Jack", LastName = "Rowling" }
                                    }
            };

            reqCriteria.Currency = "CAD";
            reqCriteria.PaymentType = PaymentTypes.Obligo;
                   
            reqCriteria.RequestedPrice = ExtractDecimalFromString(collection["message"]);
           
            ViewBag.HotelId = reqCriteria.HotelId;
            ViewBag.HotelRoomTypeId = reqCriteria.HotelRoomTypeId;

            HotelFlowClient client = new HotelFlowClient();

            RGInfoResults sreq;

            try
            {
                sreq = client.BookHotelV3(auth, reqCriteria, new Feature[] { new Feature { name = "OriginalImageSize", value = "true" } });

            }
            catch (System.ServiceModel.FaultException ex)
            {
                return View("PriceChange", ex);
            }
            finally
            {
                client.Close();
            }

            return View("Confirmation", sreq.ResGroup);

        }
    }
}
