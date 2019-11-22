using System;

namespace NetCore3_api.Domain
{
    public class Entity
    {
        /// <summary>
        /// Siguiendo el ejemplo de event_id que es numérico,
        /// se utiliza un int (considerar que no escala en un sistema real)
        /// </summary>
        public long Id { get; set; }
    }
}
