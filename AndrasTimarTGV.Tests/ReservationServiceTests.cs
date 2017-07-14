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
    public class ReservationServiceTests {
        [Test]
        public void CanOnlyDeleteThreeDaysBefore() {
            AppUser testUser = new AppUser();

            Reservation testReservation = new Reservation() {
                Trip = new Trip() { Time = DateTime.Now.AddDays(1), FreeBusinessPlaces = 10},
                Seats = 0,
                User = testUser,
                TravelClass =  TravelClass.Business,               
            };

            Mock<IReservationRepository> repo = new Mock<IReservationRepository>();
            repo.Setup(x => x.GetReservationById(It.IsAny<int>())).Returns(testReservation);
            Mock<ITripService> tripService = new Mock<ITripService>();
            ReservationService reservationService = new ReservationService(repo.Object, tripService.Object, null);

            Assert.Throws<TooLateReservationException>(() => reservationService.Delete(testUser, 1));

            testReservation.Trip.Time = DateTime.Now.AddDays(2);
            Assert.Throws<TooLateReservationException>(() => reservationService.Delete(testUser, 1));
            

            testReservation.Trip.Time = DateTime.Now.AddDays(3);
            Assert.Throws<TooLateReservationException>(() => reservationService.Delete(testUser, 1));

        }
    }
}
