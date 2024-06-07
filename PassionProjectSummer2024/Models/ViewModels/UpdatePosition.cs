using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProjectSummer2024.Models.ViewModels
{

    // This View Model helps in  collecting data from two separate entities from two different models and to be used  as one object in different model 
    // Here we are using SelectedPosition of type  IEnumerable<PositionDto> ,UnrealtedEmployees of type  IEnumerable<EmployeeDto> and  RelatedEmployees of type  IEnumerable<EmployeeDto>
    public class UpdatePosition
    {
        public PositionDto SelectedPosition { get; set; }

        public IEnumerable<EmployeeDto> RelatedEmployees { get; set; }

        public IEnumerable<EmployeeDto> UnrelatedEmployees { get; set; }


    }
}