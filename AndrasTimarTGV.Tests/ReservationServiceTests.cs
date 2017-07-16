using System;
using System.Collections.Generic;
using System.Text;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Repositories;
using AndrasTimarTGV.Models.Services;
using AndrasTimarTGV.Util;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace AndrasTimarTGV.Tests
{
    [TestFixture]
    public class ReservationServiceTests
    {
        private Mock<IReservationRepository> ReservationRepoMock;
        private ReservationService ReservationService;
        private Mock<ITripService> TripServiceMock;
        private Reservation TestReservation;
        private AppUser TestUser;
        private Trip TestTrip;
        private Mock<IEmailService> EmailServiceMock;

        [SetUp]
        public void SetUp()
        {
            TestTrip = new Trip() {TripId = 1, Time = DateTime.Now.AddDays(5), FreeBusinessPlaces = 10};
            TestUser = new AppUser
            {
                Email = "test@test.te"
            };
            TestReservation = new Reservation()
            {
                ReservationId = 1,
                Trip = TestTrip,
                Seats = 3,
                User = TestUser,
                TravelClass = TravelClass.Business,
            };

            ReservationRepoMock = new Mock<IReservationRepository>();
            ReservationRepoMock.Setup(x => x.GetReservationById(TestReservation.ReservationId))
                .Returns(TestReservation);
            TripServiceMock = new Mock<ITripService>();
            EmailServiceMock = new Mock<IEmailService>();
            ReservationService = new ReservationService(ReservationRepoMock.Object, TripServiceMock.Object, EmailServiceMock.Object);
        }

        [Test]
        public void CanNotDeleteInLastThreeDays()
        {
            for (int i = 1; i < 4; i++)
            {
                TestReservation.Trip.Time = DateTime.Now.AddDays(i);
                Assert.Throws<ReservationOutOfTimeframeException>(
                    () => ReservationService.Delete(TestUser, TestReservation.ReservationId),
                    "Reservations can not be deleted in the last 3 days!");
            }
            TestReservation.Trip.Time = DateTime.Now.AddDays(4);
            Assert.DoesNotThrow(() => ReservationService.Delete(TestUser, TestReservation.ReservationId));
        }

        [Test]
        public void UserCanOnlyDeleteOwnReservations()
        {
            AppUser userTwo = new AppUser();
            Reservation userTwoReservation = new Reservation()
            {
                ReservationId = 2,
                Trip = new Trip() {Time = DateTime.Now.AddDays(5), FreeBusinessPlaces = 10},
                Seats = 0,
                User = userTwo,
                TravelClass = TravelClass.Business,
            };

            ReservationRepoMock.Setup(x => x.GetReservationById(userTwoReservation.ReservationId))
                .Returns(userTwoReservation);

            Assert.Throws<InvalidOperationException>(
                () => ReservationService.Delete(userTwo, TestReservation.ReservationId));
            Assert.Throws<InvalidOperationException>(
                () => ReservationService.Delete(TestUser, userTwoReservation.ReservationId));
            Assert.DoesNotThrow(() => ReservationService.Delete(TestUser, TestReservation.ReservationId),
                "Trip does not belong to logged in user");
            Assert.DoesNotThrow(() => ReservationService.Delete(userTwo, userTwoReservation.ReservationId),
                "Trip does not belong to logged in user");
        }

        [Test]
        public void DeletionUpdatesTripSeats()
        {
            ReservationService.Delete(TestUser, TestReservation.ReservationId);

            TripServiceMock.Verify(x=>x.IncreaseTripSeatsByReservation(TestReservation),Times.Once);       
        }

        [Test]
        public void DeletionIsPropagatedToRepo()
        {
            ReservationService.Delete(TestUser, TestReservation.ReservationId);
            ReservationRepoMock.Verify(x => x.Delete(TestReservation), Times.Once);
        }

        [Test]
        public void CanNotDeleteNonexistentTrip()
        {
            Assert.Throws<InvalidOperationException>(() => ReservationService.Delete(TestUser, 9999),
                "Trip does not exist");
        }

        [Test]
        public void CanNotReserveForPast()
        {
            Trip testTrip = new Trip
            {
                TripId = 1,
                Time = DateTime.Now.AddDays(-1)
            };
            TestReservation.Trip = testTrip;

            Assert.Throws<ReservationOutOfTimeframeException>(
                () => ReservationService.SaveReservation(TestReservation));
        }
        [Test]
        public void CanNotReserveTooEarly()
        {
            TestTrip = new Trip {
                TripId = 1,
                Time = DateTime.Now.AddDays(15)
            };
            TripServiceMock.Setup(x => x.GetTripById(TestReservation.Trip.TripId)).Returns(TestTrip);
            TestReservation.Trip = TestTrip;
            Assert.Throws<ReservationOutOfTimeframeException>(
                () => ReservationService.SaveReservation(TestReservation));
        }

        [Test]
        public void SaveReservationRemovesTripSeats()
        {           
            ReservationService.SaveReservation(TestReservation);
            TripServiceMock.Verify(x=>x.DecreaseTripSeatsByReservation(TestReservation), Times.Once);
        }

        [Test]
        public void SaveReservationPropagatesToRepo()
        {
            ReservationService.SaveReservation(TestReservation);
            ReservationRepoMock.Verify(x=>x.SaveReservation(TestReservation));
        }

        [Test]
        public void SaveReservationSendsEmail() {
            ReservationService.SaveReservation(TestReservation);
            EmailServiceMock.Verify(x => x.SendMail(TestReservation));
        }
    }

  
}