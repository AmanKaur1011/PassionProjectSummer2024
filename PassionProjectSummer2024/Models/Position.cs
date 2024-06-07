using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PassionProjectSummer2024.Models
{
    public class Position
    {
        [Key]
        public  int PositionId { get; set; }

         public string PositionTitle { get; set; }

        public decimal HourlyWage { get; set; }




    }
    // Simplified version of the  Position Class to access data easily in the api and mvc controllers
    public class PositionDto
    {
        public int PositionId { get; set; }

        public string PositionTitle { get; set; }

        public decimal HourlyWage { get; set; }
    }
}