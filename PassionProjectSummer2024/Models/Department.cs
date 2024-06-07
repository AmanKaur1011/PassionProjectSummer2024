﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassionProjectSummer2024.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public string DepartmentManager {  get; set; }
        // Not mapped because  it is not added into the databse but is still there in the class in order to find the number of employees in the department dynamically whenever a new employee is added or deleted from the department
        [NotMapped]
         public int NoOfEmployees { get
            { using (var context = new ApplicationDbContext())
                {
                    return context.Employees.Count(e => e.DepartmentId == this.DepartmentId);
                }
            }
        }






   // Simplified version of the Department Class to access data easily in the api and mvc controllers
    }
    public class DepartmentDto
    {
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public string DepartmentManager { get; set; }

        public int NoOfEmployees { get; set; }
    }
}