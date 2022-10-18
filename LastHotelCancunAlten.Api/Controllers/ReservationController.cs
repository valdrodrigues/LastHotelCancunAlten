using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using LastHotelCancunAlten.Domain.Dto;
using LastHotelCancunAlten.Domain.IApplication;

namespace LastHotelCancunAlten.Controllers
{
    [Route("reservation")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationApplication _reservationApplication;

        public ReservationController(IReservationApplication reservationApplication)
        {
            _reservationApplication = reservationApplication;
        }

        /// <summary>
        /// Create a new reservation.
        /// </summary>
        /// <param name="dto">dto.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///       "name": "Person name",
        ///       "idCard": "123456AAA",
        ///       "startDate": "2022-11-15",
        ///       "endDate": "2022-11-17"
        ///     }
        ///
        /// </remarks>
        /// <response code="500">Internal server error.</response>
        [HttpPost]
        public IActionResult PostCreateReservation([FromBody] ReservationDto dto)
        {
            try
            {
                _reservationApplication.CreateReservation(dto);

                return StatusCode((int)HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Update a reservation.
        /// </summary>
        /// <param name="dto">dto.</param>
        /// <returns>The updated entity.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT
        ///     {
        ///       "id": "634ab9b17b32288b3420048b",
        ///       "name": "Person name update",
        ///       "idCard": "123456BBB",
        ///       "startDate": "2022-11-15",
        ///       "endDate": "2022-11-17"
        ///     }
        ///
        /// </remarks>
        /// <response code="404">No reservation was found for the provided Id.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPut]
        public IActionResult PutUpdateReservation([FromBody] ReservationDto dto)
        {
            try
            {
                var result = _reservationApplication.UpdateReservation(dto);

                if (result == null)
                    return StatusCode((int)HttpStatusCode.NotFound);

                return StatusCode((int)HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Cancel a reservation.
        /// </summary>
        /// <param name="id">Reservation Id.</param>
        /// <response code="404">No reservation was found for the provided Id.</response>
        /// <response code="500">Internal server error.</response>
        [HttpDelete]
        [Route("cancel/{id}")]
        public IActionResult DeleteCancelReservation(string id)
        {
            try
            {
                var result = _reservationApplication.CancelReservation(id);

                if (result == null)
                    return StatusCode((int)HttpStatusCode.NotFound);

                return StatusCode((int)HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get the reservation by id.
        /// </summary>
        /// <param name="id">Reservation Id.</param>
        /// <returns>The reservation.</returns>
        /// <response code="404">No reservation was found to the provided Id.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetReservationById(string id)
        {
            try
            {
                var result = _reservationApplication.GetReservationById(id);

                if (result == null)
                    return StatusCode((int)HttpStatusCode.NotFound);

                return StatusCode((int)HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get the reservation by idCard.
        /// </summary>
        /// <param name="idCard">IdCard.</param>
        /// <returns>The reservation list for the provided IdCard.</returns>
        /// <response code="500">Internal server error.</response>
        [HttpGet]
        [Route("idcard/{idCard}")]
        public IActionResult GetReservationByIdCard(string idCard)
        {
            try
            {
                var result = _reservationApplication.GetReservationByIdCard(idCard);

                if (result.Count == 0)
                    return StatusCode((int)HttpStatusCode.NotFound);

                return StatusCode((int)HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Check the room availability.
        /// </summary>
        /// <remarks>
        /// Format sample:
        ///
        /// yyyy-mm-dd
        ///
        /// </remarks>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
        /// <returns>Room availability for the specified date range.</returns>
        /// <response code="500">Internal server error.</response>
        [HttpGet]
        [Route("check-availability")]
        public IActionResult GetCheckAvailability([FromQuery] DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = _reservationApplication.CheckAvailability(startDate, endDate);
                return StatusCode((int)HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
