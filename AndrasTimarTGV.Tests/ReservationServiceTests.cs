using System;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Repositories;
using Moq;
using Xunit;

namespace AndrasTimarTGV.Tests
{
    public class ReservationServiceTests
    {
        [Fact]
        public void CanOnlyDeleteThreeDaysBefore()
        {


            Reservation testReservation = new Reservation()
            {
                Trip = new Trip() {Time = DateTime.Now.AddDays(-1)},
                Seats = 0
            };
          
            Mock<IReservationRepository> repo = new Mock<IReservationRepository>();
            repo.Setup(x => x.GetReservationById(It.IsAny<int>())).Returns(testReservation);      
        }
    }
}
