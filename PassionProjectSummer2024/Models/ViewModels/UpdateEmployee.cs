using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProjectSummer2024.Models.ViewModels
{

    // This View Model helps in  collecting data from two separate entities from two different models and to be used  as one object in different model 
    // Here we are using SelectedEmployee of type  IEnumerable<EmployeeDto> ,DepartmentOPtions of type  IEnumerable<DepartmentDto> and   PositionOptions of type  IEnumerable<PositionDto>
    public class UpdateEmployee
    {
        public EmployeeDto SelectedEmployee { get; set; }

        public IEnumerable<DepartmentDto> DepartmentOptions { get; set; }

        public IEnumerable<PositionDto> PositionOptions { get; set; }

    }
}