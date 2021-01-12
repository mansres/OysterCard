using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OysterCardSystem.Core;

namespace OysterCardSystem.Tests.CoreTest
{
    /// <summary>
    /// Test cases for Journey JourneyFare calculations
    /// </summary>
    [TestClass]
    public class JourneyFareTest
    {
        [TestMethod]
        [ExpectedException(typeof(FareException), "You don't have enough balance!")]
        public void TestValidadeBusException()
        {
            SmartCard card = new SmartCard(JourneyFare.BUS_FARE - 1f);
            JourneyFare fare = new JourneyFare();
            fare.Validate(Transport.BUS, card);
        }

        [TestMethod]
        [ExpectedException(typeof(FareException), "You don't have enough balance!")]
        public void TestValidateTubeFareException()
        {

            SmartCard card = new SmartCard(JourneyFare.MAX_TUBE_FARE - 1f);
            JourneyFare fare = new JourneyFare();
            fare.Validate(Transport.TUBE, card);
        }

        [TestMethod]
        public void TestChargeMaxTube()
        {
            SmartCard card = new SmartCard(JourneyFare.MAX_TUBE_FARE);
            JourneyFare fare = new JourneyFare();
            fare.ChargeMax(Transport.TUBE, card);
            Assert.AreEqual(0f, card.GetBalance());
        }

        [TestMethod]
        public void TestChargeMaxBus()
        {
            SmartCard card = new SmartCard(JourneyFare.BUS_FARE);
            JourneyFare fare = new JourneyFare();
            fare.ChargeMax(Transport.BUS, card);
            Assert.AreEqual(0f, card.GetBalance());
        }

        [TestMethod]
        public void TestChargeBus()
        {
            SmartCard card = new SmartCard(JourneyFare.BUS_FARE);
            JourneyFare fare = new JourneyFare();
            Journey busJourney = new Journey(fare);
            busJourney.SetStartPoint(Transport.BUS, null, card);
            busJourney.SetEndPoint(null);
            fare.Charge(Transport.BUS, busJourney, card);
            Assert.AreEqual(0f, card.GetBalance());
        }

        [TestMethod]
        public void TestChargeTubeZoneOne()
        {
            SmartCard card = new SmartCard(JourneyFare.MAX_TUBE_FARE);
            JourneyFare fare = new JourneyFare();
            Journey tubeJourney = new Journey(fare);
            tubeJourney.SetStartPoint(Transport.TUBE, new Zone(Station.HOLBORN), card);
            tubeJourney.SetEndPoint(new Zone(Station.EARLS_COURT));
            Assert.AreEqual(JourneyFare.MAX_TUBE_FARE - JourneyFare.ZONE_ONE_FARE, card.GetBalance());
        }

        [TestMethod]
        public void TestChargeTubeAnyZoneOutSideZoneOne()
        {
            SmartCard card = new SmartCard(JourneyFare.MAX_TUBE_FARE);
            JourneyFare fare = new JourneyFare();
            Journey tubeJourney = new Journey(fare);
            tubeJourney.SetStartPoint(Transport.TUBE, new Zone(Station.HAMMERSMITH), card);
            tubeJourney.SetEndPoint(new Zone(Station.EARLS_COURT));
            Assert.AreEqual(JourneyFare.MAX_TUBE_FARE - JourneyFare.ANY_ZONE_OUTSIDE_ZONE_ONE_FARE, card.GetBalance());
        }

        [TestMethod]
        public void TestChargeTubeTwoInZoneOne()
        {
            SmartCard card = new SmartCard(JourneyFare.MAX_TUBE_FARE);
            JourneyFare fare = new JourneyFare();
            Journey tubeJourney = new Journey(fare);
            tubeJourney.SetStartPoint(Transport.TUBE, new Zone(Station.HAMMERSMITH), card);
            tubeJourney.SetEndPoint(new Zone(Station.HOLBORN));
            Assert.AreEqual(JourneyFare.MAX_TUBE_FARE - JourneyFare.ANY_TWO_ZONES_INC_ZONE_ONE_FARE, card.GetBalance());
        }

        [TestMethod]
        public void TestChargeTubeTwoExcludingZoneOne()
        {
            SmartCard card = new SmartCard(JourneyFare.MAX_TUBE_FARE);
            JourneyFare fare = new JourneyFare();
            Journey tubeJourney = new Journey(fare);
            tubeJourney.SetStartPoint(Transport.TUBE, new Zone(Station.HAMMERSMITH), card);
            tubeJourney.SetEndPoint(new Zone(Station.WIMBLEDON));
            Assert.AreEqual(JourneyFare.MAX_TUBE_FARE - JourneyFare.ANY_TWO_ZONES_EXC_ZONE_ONE_FARE, card.GetBalance());
        }

        [TestMethod]
        public void TestChargeTubeThreeZones()
        {
            SmartCard card = new SmartCard(JourneyFare.MAX_TUBE_FARE);
            JourneyFare fare = new JourneyFare();
            Journey tubeJourney = new Journey(fare);
            tubeJourney.SetStartPoint(Transport.TUBE, new Zone(Station.HOLBORN), card);
            tubeJourney.SetEndPoint(new Zone(Station.WIMBLEDON));
            Assert.AreEqual(JourneyFare.MAX_TUBE_FARE - JourneyFare.ANY_THREE_ZONES_FAIR, card.GetBalance());
        }

        [TestMethod]
        public void TestPositiveJourney()
        {
            SmartCard card = new SmartCard(JourneyFare.MAX_TUBE_FARE);
            JourneyFare fare = new JourneyFare();
            Journey tubeJourney = new Journey(fare);
            tubeJourney.SetStartPoint(Transport.TUBE, new Zone(Station.HOLBORN), card);
            tubeJourney.SetEndPoint(new Zone(Station.WIMBLEDON));
            Assert.AreEqual(JourneyFare.MAX_TUBE_FARE - JourneyFare.ANY_THREE_ZONES_FAIR, card.GetBalance());
        }

        [TestMethod]
        public void TestUserRealtimeJourney()
        {
            SmartCard card = new SmartCard();
            card.AddMoney(30); // Loading a card with £30 

            Assert.AreEqual(30, Math.Abs(card.GetBalance()));

            Journey journeyHolbornToEarlsCourt = new Journey(new JourneyFare());
            journeyHolbornToEarlsCourt.SetStartPoint(Transport.TUBE, new Zone(Station.HOLBORN), card);
            journeyHolbornToEarlsCourt.SetEndPoint(new Zone(Station.EARLS_COURT));
            //Card Balance after first journey (Tube Holborn to Earl’s Court)  is £27.5
            Assert.AreEqual(27.5, Math.Abs(card.GetBalance()));

            Journey journeyBusEarlToChelsea = new Journey(new JourneyFare());
            journeyBusEarlToChelsea.SetStartPoint(Transport.BUS, null, card);
            journeyBusEarlToChelsea.SetEndPoint(null);
            //Card Balance after second journey (328 bus from Earl’s Court to Chelsea) is £25.7
            Assert.AreEqual(25.7f, Math.Abs(card.GetBalance()));

            Journey journeyEarlsToHammersmith = new Journey(new JourneyFare());
            journeyEarlsToHammersmith.SetStartPoint(Transport.TUBE, new Zone(Station.EARLS_COURT), card);
            journeyEarlsToHammersmith.SetEndPoint(new Zone(Station.HAMMERSMITH));
            //Card Balance after third journey (Tube Earl’s court to Hammersmith) is £23.7
            Assert.AreEqual(23.7f, Math.Abs(card.GetBalance()));

        }
    }
}
