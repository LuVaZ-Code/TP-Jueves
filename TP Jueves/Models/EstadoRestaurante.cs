namespace TP_Jueves.Models
{
    /// <summary>
    /// Estado del restaurante en el sistema.
    /// </summary>
    public enum EstadoRestaurante
    {
        /// <summary>
        /// Restaurante creado pero sin configuración completa (sin mesas o turnos).
        /// </summary>
        EnConfiguracion = 0,

        /// <summary>
        /// Restaurante activo y aceptando reservas.
        /// </summary>
        Activo = 1,

        /// <summary>
        /// Restaurante pausado temporalmente (no acepta nuevas reservas).
        /// </summary>
        Pausado = 2,

        /// <summary>
        /// Restaurante cerrado permanentemente.
        /// </summary>
        Cerrado = 3
    }
}
