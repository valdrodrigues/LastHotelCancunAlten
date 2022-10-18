using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace LastHotelCancunAlten.Domain.Dto
{
    public class ReservationDto
    {
#nullable enable
        public string? Id { get; set; }
#nullable disable

        [Required]
        public string Name { get; set; }

        [Required]
        public string IdCard { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }

    public class ReservationDtoValidator : AbstractValidator<ReservationDto>
    {
        public ReservationDtoValidator()
        {
            RuleFor(p => p.Name)
                .Length(3, 100)
                .WithMessage("ReservationDto - Name is required. (Lenth min 3, max 100)");

            RuleFor(p => p.IdCard)
                .Length(1,20)
                .WithMessage("ReservationDto - IdCard is required. (Lenth min 1, max 20)");

            RuleFor(p => p)
                .Must(p => p.StartDate != default && p.StartDate != null)
                .WithMessage("ReservationDto - StartDate is required.");

            RuleFor(p => p)
                .Must(p => p.EndDate != default && p.EndDate != null)
                .WithMessage("ReservationDto - EndDate is required.");

            RuleFor(p => p)
                .Must(p => p.EndDate > p.StartDate)
                .WithMessage("ReservationDto - EndDate must greater than StartDate.");

            RuleFor(p => p)
                .Must(p => (p.EndDate - p.StartDate).TotalDays < 3)
                .WithMessage("ReservationDto - Stay can't be longer than 3 days.");

            RuleFor(p => p)
                .Must(p => DateTime.Now.AddDays(30) > p.StartDate)
                .WithMessage("ReservationDto - Can't reserve more than 30 days in advance.");
        }
    }
}
