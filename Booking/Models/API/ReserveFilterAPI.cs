namespace Booking.Models.API
{
    public class ReserveFilterAPI
    {
        public string UserEmail { get; set; }
        public string EventDate { get; set; }
        public int BuildingId { get; set; }
        public int RoomId { get; set; }

        public ReserveFilterAPI() { }
    }
}
