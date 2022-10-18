using NUnit.Framework;
using System;
using LastHotelCancunAlten.Domain.Dto;
using LastHotelCancunAlten.Domain.Entity;

namespace LastHotelCancunAlten.Test
{
    public class ReservationTest
    {
        [Test]
        public void Reservation_ValidConstructor_ProperInstantiated()
        {
            // Act
            var dto = CreateDto();
            var reservation = new Reservation(
                dto.Name,
                dto.IdCard,
                dto.StartDate,
                dto.EndDate);

            // Assert
            ReservationConstructorValidator(reservation, dto);
        }

        private void ReservationConstructorValidator(Reservation reservation, ReservationDto dto)
        {
            Assert.AreEqual(dto.Name, reservation.Name);
            Assert.AreEqual(dto.IdCard, reservation.IdCard);
            Assert.AreEqual(dto.StartDate, reservation.StartDate);
            Assert.AreEqual(dto.EndDate, reservation.EndDate);
        }

        private ReservationDto CreateDto()
        {
            return new ReservationDto()
            {
                IdCard = "123456AAA",
                Name = "Valdeci Rodrigues Junior",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2)
            };
        }
    }
}