using System;
using System.Collections.Generic;
using LastHotelCancunAlten.Domain.Dto;
using LastHotelCancunAlten.Domain.Entity;

namespace LastHotelCancunAlten.Domain.IApplication
{
    public interface IReservationApplication
    {
        void CreateReservation(ReservationDto dto);

        Reservation UpdateReservation(ReservationDto dto);

        string CancelReservation(string id);

        Reservation GetReservationById(string id);

        List<Reservation> GetReservationByIdCard(string idCard);

        bool CheckAvailability(DateTime startDate, DateTime endDate);
    }
}
