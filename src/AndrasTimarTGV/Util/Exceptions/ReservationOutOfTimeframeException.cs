using System;

namespace AndrasTimarTGV.Util
{
    public class ReservationOutOfTimeframeException : ReservationException
    {
        public ReservationOutOfTimeframeException()
        {
            
        }
        public ReservationOutOfTimeframeException(string message) : base(message) {

        }
        public ReservationOutOfTimeframeException(string message, Exception inner): base(message,inner) {

        }
    }
}
