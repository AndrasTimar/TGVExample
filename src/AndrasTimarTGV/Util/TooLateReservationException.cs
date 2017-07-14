using System;

namespace AndrasTimarTGV.Util
{
    public class TooLateReservationException : Exception
    {
        public TooLateReservationException()
        {
            
        }
        public TooLateReservationException(string message) : base(message) {

        }
        public TooLateReservationException(string message, Exception inner): base(message,inner) {

        }
    }
}
