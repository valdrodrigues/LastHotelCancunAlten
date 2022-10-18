using System;
using System.Collections.Generic;
using LastHotelCancunAlten.Domain.Dto;
using LastHotelCancunAlten.Domain.Entity;
using LastHotelCancunAlten.Domain.IApplication;
using LastHotelCancunAlten.Domain.IRepository;

namespace LastHotelCancunAlten.Application
{
    public class ReservationApplication : IReservationApplication
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationApplication(
            IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public void CreateReservation(ReservationDto dto)
        {
            var reservation = new Reservation(
                                dto.Name, 
                                dto.IdCard, 
                                dto.StartDate, 
                                dto.EndDate);

            var isAvailable = _reservationRepository.IsAvailable(dto.StartDate, dto.EndDate);
            reservation.CanCreate(isAvailable);

            _reservationRepository.Create(reservation);
        }

        public Reservation UpdateReservation(ReservationDto dto)
        {
            if (dto.Id != null)
            {
                var reservation = _reservationRepository.GetById(dto.Id);

                if (reservation == null)
                    return null;

                var isAvailable = _reservationRepository.IsAvailable(dto.StartDate, dto.EndDate, dto.Id);
                reservation.CanUpdate(reservation, dto, isAvailable);

                reservation.Update(
                    dto.Name,
                    dto.IdCard,
                    dto.StartDate,
                    dto.EndDate);

                return _reservationRepository.Update(reservation);
            }

            return null;
        }

        public string CancelReservation(string id)
        {
            var reservation = _reservationRepository.GetById(id);

            if (reservation == null)
                return null;

            reservation.CanCancel();

            _reservationRepository.Delete(id);
            return id;
        }

        public Reservation GetReservationById(string id)
        {
            var reservation = _reservationRepository.GetById(id);

            if (reservation == null)
                return null;

            return reservation;
        }

        public List<Reservation> GetReservationByIdCard(string idCard)
        {
            var reservation = _reservationRepository.GetByIdCard(idCard);

            if (reservation == null)
                return null;

            return reservation;
        }

        public bool CheckAvailability(DateTime startDate, DateTime endDate)
        {
            if (startDate == null || startDate == default
                || endDate == null || endDate == default)
            {
                throw new ArgumentNullException($"StartDate and EndDate are required.");
            }

            if (endDate < startDate)
            {
                throw new ArgumentException("EndDate must greater than StartDate.", nameof(endDate));
            }

            if ((endDate - startDate).TotalDays > 3)
            {
                throw new ArgumentException("Stay can't be longer than 3 days.", nameof(endDate));
            }

            if (DateTime.Now.AddDays(30) < startDate)
            {
                throw new ArgumentException("Can't reserve more than 30 days in advance.", nameof(startDate));
            }

            return _reservationRepository.IsAvailable(startDate, endDate);
        }
    }
}
