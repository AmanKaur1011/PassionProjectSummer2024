using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProjectSummer2024.Models.ViewModels
{
    // This View Model helps in  collecting data from two separate entities from two different models and to be used  as one object in different model 
    // Here we are using PositionDto and IEnumerable<EmployeeDto>
    public class DetailsPosition
    {
        public PositionDto SelectedPosition { get; set; }

        public IEnumerable<EmployeeDto> RelatedEmployees { get; set; }

    }
}