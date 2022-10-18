using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.ComponentModel.DataAnnotations;
using LastHotelCancunAlten.Domain.Dto;

namespace LastHotelCancunAlten.Domain.Entity
{
    public class Reservation
    {
        public Reservation(
            string name,
            string idCard,
            DateTime startDate,
            DateTime endDate)
        {
            CommonValidation(
                name,
                idCard,
                startDate,
                endDate);

            Name = name;
            IdCard = idCard;
            StartDate = startDate;
            EndDate = endDate;
            BookingDate = DateTime.Now;
        }

        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        [BsonElement("Name")]
        public string Name { get; private set; }

        [Required]
        [BsonElement("IdCard")]
        public string IdCard { get; private set; }

        [Required]
        [BsonElement("BookingDate")]
        public DateTime BookingDate { get; private set; }

        [Required]
        [BsonElement("StartDate")]
        public DateTime StartDate { get; private set; }

        [Required]
        [BsonElement("EndDate")]
        public DateTime EndDate { get; private set; }

        public void Update(
            string name,
            string idCard,
            DateTime startDate,
            DateTime endDate)
        {
            CommonValidation(
                name, 
                idCard, 
                startDate, 
                endDate);

            Name = name;
            IdCard = idCard;
            StartDate = startDate;
            EndDate = endDate;
        }

        public void CanCreate(bool isAvailable)
        {
            IsAvailable(isAvailable);
        }

        public void CanUpdate(Reservation reservation, ReservationDto dto, bool isAvailable)
        {
            if (reservation.StartDate.Date != dto.StartDate.Date
                    || reservation.EndDate.Date != dto.EndDate)
            {
                IsAvailable(isAvailable);
            }
        }

        public void CanCancel()
        {
            if (StartDate <= DateTime.Now)
            {
                throw new ArgumentException("Can't cancel a past reservation.");
            }
        }

        private void IsAvailable(bool isAvailable)
        {
            if (!isAvailable || StartDate <= DateTime.Now)
            {
                throw new ArgumentException("Date range is not available.");
            }
        }

        private void CommonValidation(
            string name,
            string idCard,
            DateTime startDate,
            DateTime endDate)
        {
            NameValidator(name);
            IdCardValidator(idCard);
            DateValidator(startDate, endDate);
        }

        private void NameValidator(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name), "Name is required.");
            }

            if (name.Length < 3 || name.Length > 100)
            {
                throw new FormatException("Name lenth min 3, max 100.");
            }
        }

        private void IdCardValidator(string idCard)
        {
            if (string.IsNullOrEmpty(idCard))
            {
                throw new ArgumentNullException(nameof(idCard), "IdCard is required.");
            }

            if (idCard.Length < 1 || idCard.Length > 20)
            {
                throw new FormatException("IdCard lenth min 1, max 20.");
            }
        }

        private void DateValidator(DateTime startDate, DateTime endDate)
        {
            DateNullOrDefaultValidator(startDate, nameof(startDate));
            DateNullOrDefaultValidator(endDate, nameof(endDate));

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
        }

        private void DateNullOrDefaultValidator(DateTime date, string paramName)
        {
            if (date == null || date == default)
            {
                throw new ArgumentNullException(paramName, $"{paramName} is required.");
            }
        }
    }
}
