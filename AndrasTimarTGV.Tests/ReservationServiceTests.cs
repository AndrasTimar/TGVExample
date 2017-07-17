using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
            ReservationRepoMock.Setup(x => x.GetReservationByIdAsync(TestReservation.ReservationId))
                .ReturnsAsync(TestReservation);
            TripServiceMock = new Mock<ITripService>();
            EmailServiceMock = new Mock<IEmailService>();
            ReservationService = new ReservationService(ReservationRepoMock.Object, TripServiceMock.Object, EmailServiceMock.Object);
        }

        [Test]
        public void CanNotDeleteInLastThreeDays()
        {
            TestReservation.Trip.Time = DateTime.Now.AddDays(1);
            Assert.ThrowsAsync<ReservationOutOfTimeframeException>(async () => await ReservationService.DeleteAsync(TestUser, TestReservation.ReservationId),
                "Reservations can not be deleted in the last 3 days!");

            TestReservation.Trip.Time = DateTime.Now.AddDays(2);
            Assert.ThrowsAsync<ReservationOutOfTimeframeException>(async () => await ReservationService.DeleteAsync(TestUser, TestReservation.ReservationId),
                "Reservations can not be deleted in the last 3 days!");

            TestReservation.Trip.Time = DateTime.Now.AddDays(3);
            Assert.ThrowsAsync<ReservationOutOfTimeframeException>(async () => await ReservationService.DeleteAsync(TestUser, TestReservation.ReservationId),
                "Reservations can not be deleted in the last 3 days!");

            TestReservation.Trip.Time = DateTime.Now.AddDays(4);
            Assert.DoesNotThrowAsync(() => ReservationService.DeleteAsync(TestUser, TestReservation.ReservationId));
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

            ReservationRepoMock.Setup(x => x.GetReservationByIdAsync(userTwoReservation.ReservationId))
                .ReturnsAsync(userTwoReservation);

            Assert.ThrowsAsync<InvalidOperationException>(
                () => ReservationService.DeleteAsync(userTwo, TestReservation.ReservationId));
            Assert.ThrowsAsync<InvalidOperationException>(
                () => ReservationService.DeleteAsync(TestUser, userTwoReservation.ReservationId));
            Assert.DoesNotThrowAsync(() => ReservationService.DeleteAsync(TestUser, TestReservation.ReservationId),
                "Trip does not belong to logged in user");
            Assert.DoesNotThrowAsync(() => ReservationService.DeleteAsync(userTwo, userTwoReservation.ReservationId),
                "Trip does not belong to logged in user");
        }

        [Test]
        public async Task DeletionUpdatesTripSeats()
        {
            await ReservationService.DeleteAsync(TestUser, TestReservation.ReservationId);

            TripServiceMock.Verify(x=>x.IncreaseTripSeatsByReservationAsync(TestReservation),Times.Once);       
        }

        [Test]
        public async Task DeletionIsPropagatedToRepo()
        {
            await ReservationService.DeleteAsync(TestUser, TestReservation.ReservationId);
            ReservationRepoMock.Verify(x => x.DeleteReservationAsync(TestReservation), Times.Once);
        }

        [Test]
        public void CanNotDeleteNonexistentTrip()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () => await ReservationService.DeleteAsync(TestUser, 9999),
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

            Assert.ThrowsAsync<ReservationOutOfTimeframeException>(
                () => ReservationService.SaveReservationAsync(TestReservation));
        }
        [Test]
        public void CanNotReserveTooEarly()
        {
            TestTrip = new Trip {
                TripId = 1,
                Time = DateTime.Now.AddDays(15)
            };
            TripServiceMock.Setup(x => x.GetTripByIdAsync(TestReservation.Trip.TripId)).ReturnsAsync(TestTrip);
            TestReservation.Trip = TestTrip;
            Assert.ThrowsAsync<ReservationOutOfTimeframeException>(
                () => ReservationService.SaveReservationAsync(TestReservation));
        }

        [Test]
        public async Task SaveReservationRemovesTripSeats()
        {           
            await ReservationService.SaveReservationAsync(TestReservation);
            TripServiceMock.Verify(x=>x.DecreaseTripSeatsByReservationAsync(TestReservation), Times.Once);
        }

        [Test]
        public async Task SaveReservationPropagatesToRepo()
        {
            await ReservationService.SaveReservationAsync(TestReservation);
            ReservationRepoMock.Verify(x=>x.SaveReservationAsync(TestReservation));
        }

        [Test]
        public async Task SaveReservationSendsEmail() {
            await ReservationService.SaveReservationAsync(TestReservation);
            EmailServiceMock.Verify(x => x.SendMail(TestReservation));
        }
    }

  
}