using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using AndrasTimarTGV.Controllers;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AndrasTimarTGV.Tests
{
    public class ReservationControllerTests
    {
        [Fact]
        public void CanOnlyDeleteThreeDaysBefore()
        {
            int reservationId = 2;
            ICollection<Reservation> testReservations = new List<Reservation>();
            var fakeUser = new Mock<AppUser>();
            Mock<IUserService> userServiceMock = new Mock<IUserService>();
            Mock<IReservationService> resServiceMock = new Mock<IReservationService>();
            userServiceMock.Setup(x => x.FindAppUserByName(It.IsAny<string>())).Returns(fakeUser.Object);
            fakeUser.Setup(x => x.UserName).Returns("TestUser");
            fakeUser.Setup(x => x.Reservations).Returns(testReservations);
        
            Reservation earlyReservation = new Reservation
            {
                Trip = new Trip
                {
                    Time = DateTime.Now.AddDays(-2)                    
                },
                User = fakeUser.Object
            };  
                  
            resServiceMock.Setup(x => x.GetReservationsById(reservationId)).Returns(earlyReservation);

            ReservationController resController = new ReservationController(resServiceMock.Object, null,
                userServiceMock.Object);

            resController.Delete(reservationId);
            resServiceMock.Verify(x => x.Delete(earlyReservation), Times.Never);            
        }
    }
}
