using System;
using System.Collections.Generic;
using LastHotelCancunAlten.Domain.Entity;

namespace LastHotelCancunAlten.Domain.IRepository
{
    public interface IReservationRepository
    {
        Reservation GetById(string id);

        List<Reservation> GetByIdCard(string idCard);

        void Create(Reservation entity);

        Reservation Update(Reservation entity);

        void Delete(string id);

        bool IsAvailable(DateTime startDate, DateTime endDate, string id = null);
    }
}
