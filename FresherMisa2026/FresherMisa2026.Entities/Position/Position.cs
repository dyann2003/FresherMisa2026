using FresherMisa2026.Entities.Extensions;
using System;
using System.ComponentModel.DataAnnotations;

namespace FresherMisa2026.Entities.Position
{
    [ConfigTable("position", false, "PositionCode")]
    public class Position : BaseModel
    {
        [Key]
        public Guid PositionID { get; set; }

        public string PositionCode { get; set; }

        public string PositionName { get; set; }
    }
}