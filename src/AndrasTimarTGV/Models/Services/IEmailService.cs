using AndrasTimarTGV.Models.Entities;

namespace AndrasTimarTGV.Models.Services
{
    public interface IEmailService
    {
        void SendMail(Reservation reservation);
    }
}