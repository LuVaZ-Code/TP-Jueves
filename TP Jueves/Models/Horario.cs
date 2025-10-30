namespace TP_Jueves.Models
{
    /// <summary>
    /// Fixed restaurant time slots as in UML.
    /// Each enum value corresponds to a slot label used in UI and logic.
    /// </summary>
    public enum Horario
    {
        H12_1330,   // 12:00 - 13:30
        H1330_15,   // 13:30 - 15:00
        H15_1630,   // 15:00 - 16:30
        H20_22,     // 20:00 - 22:00
        H22_00,     // 22:00 - 00:00
        H00_02      // 00:00 - 02:00
    }
}
