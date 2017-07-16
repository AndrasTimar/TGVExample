using System;
using System.Collections.Generic;
using System.Text;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Repositories;
using AndrasTimarTGV.Models.Services;
using Moq;
using NUnit.Framework;

namespace AndrasTimarTGV.Tests
{
    [TestFixture]
    class TripServiceTests
    {
        const int FreePlaces = 10;
        const int Seats = 3;
        private TripService TripService;
        private Reservation TestReservation;
        private Mock<ITripRepository> TripRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            TestReservation = new Reservation {
                Trip = new Trip {
                    FreeBusinessPlaces = FreePlaces,
                    FreeEconomyPlaces = FreePlaces               
                },
                Seats = Seats
            };
            TripRepositoryMock = new Mock<ITripRepository>();
            TripService = new TripService(TripRepositoryMock.Object);

        }

        [Test]
        public void DecreaseSeatTest()
        {
            TestReservation.TravelClass = TravelClass.Business;                    
            TripService.DecreaseTripSeatsByReservation(TestReservation);
            Assert.AreEqual(FreePlaces - Seats, TestReservation.Trip.FreeBusinessPlaces);

            TripRepositoryMock.Verify(x => x.UpdateTripSeats(TestReservation.Trip), Times.Once);

            TripRepositoryMock.ResetCalls();

            TestReservation.TravelClass = TravelClass.Economy;
            TripService.DecreaseTripSeatsByReservation(TestReservation);
            Assert.AreEqual(FreePlaces - Seats, TestReservation.Trip.FreeEconomyPlaces);

            TripRepositoryMock.Verify(x => x.UpdateTripSeats(TestReservation.Trip), Times.Once);
        }

        [Test]
        public void IncreaseSeatTest()
        {            
            TestReservation.TravelClass = TravelClass.Business;
            TripService.IncreaseTripSeatsByReservation(TestReservation);
            Assert.AreEqual(FreePlaces + Seats, TestReservation.Trip.FreeBusinessPlaces);

            TripRepositoryMock.Verify(x => x.UpdateTripSeats(TestReservation.Trip), Times.Once);

            TripRepositoryMock.ResetCalls();

            TestReservation.TravelClass = TravelClass.Economy;
            TripService.IncreaseTripSeatsByReservation(TestReservation);
            Assert.AreEqual(FreePlaces + Seats, TestReservation.Trip.FreeEconomyPlaces);

            TripRepositoryMock.Verify(x => x.UpdateTripSeats(TestReservation.Trip), Times.Once);
        }       
    }
}
