using System;

namespace AndrasTimarTGV.Util {
     public class NotEnoughSeatsException : ReservationException {
        public NotEnoughSeatsException() {
        }

        public NotEnoughSeatsException(string message) : base(message) {
        }

        public NotEnoughSeatsException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}