namespace Domain.UnitTests
{
    public enum EnumBookingCase
    {
        CheckinCannotBeToday = 0,
        BookingCannotBeMadeMoreThan30DaysInAdvance = 1,
        TheStayCannotExceed3Days = 2,
        CheckinCannotBeGreaterThanCheckout = 3
    }
}
