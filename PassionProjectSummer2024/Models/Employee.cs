using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassionProjectSummer2024.Models
{
    public class Employee
    {
        [Key]
       public int EmployeeId { get; set; }

       public string FirstName { get; set; }

       public string LastName { get; set; }

       public DateTime HireDate { get; set; }

      [ForeignKey("Department")]
      public int DepartmentId { get; set; }
      public virtual Department Department { get; set; }

        [ForeignKey("Position")]
        public int PositionId { get; set; }
        public virtual Position Position { get; set; }

       
        public int PreviousDepartmentId { get; set; }
        public int PreviousPositionId { get; set; }

    }
    // Simplified version of the Employee Class to access data easily in the api and mvc controllers
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime HireDate { get; set; }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public int PositionId { get; set; }
        public string PositionTitle { get; set; }
        public int PreviousDepartmentId { get; set; }
        public int PreviousPositionId { get; set; }

    }

}