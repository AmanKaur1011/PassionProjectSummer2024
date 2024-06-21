using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using PassionProjectSummer2024.Models;
using PassionProjectSummer2024.Models.ViewModels;

namespace PassionProjectSummer2024.Controllers
{
    public class EmployeeController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static EmployeeController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44355/api/");
        }





        /// <summary>
        /// Grabs the authentication cookie sent to this controller.
        /// For proper WebAPI authentication, you can send a post request with login credentials to the WebAPI and log the access token from the response. The controller already knows this token, so we're just passing it up the chain.
        /// 
        /// Here is a descriptive article which walks through the process of setting up authorization/authentication directly.
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/individual-accounts-in-web-api
        /// </summary>
        private void GetApplicationCookie()
        {
            string token = "";
            //HTTP client is set up to be reused, otherwise it will exhaust server resources.
            //This is a bit dangerous because a previously authenticated cookie could be cached for
            //a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }



        /// <summary>
        /// This method communicate with the employee data api and get the list of employees and show them on the webpage 
        /// </summary>
        /// <returns>
        /// Returns  a view with the list of employees
        /// </returns>
        /// <example>  GET: Employee/List => List View (with the list of employees)
        /// </example>

        [HttpGet]
        public ActionResult List()
        {
            string url = "EmployeeData/ListEmployees";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<EmployeeDto> employees = response.Content.ReadAsAsync<IEnumerable<EmployeeDto>>().Result;
            //Debug.WriteLine("Number of employees received : ");
            //Debug.WriteLine(employees.Count());



            return View(employees);
        }

        /// <summary>
        /// This method communicate with the FindEmployee method in the employee data api , get the infomartion about the particular employee and show it on the webpage 
        /// </summary>
        /// <param name="id">The id of an employee whose information  is requested </param>
        /// <returns>
        /// Returns  a view with the information about a particular employee
        /// </returns>
        /// <example>  GET: Employee/Details/5 => Details View( The details of a requested employee)
        /// </example>


        
        [HttpGet]
        public ActionResult Details(int id)
        {
            string url = "EmployeeData/FindEmployee/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            EmployeeDto selectedEmployee = response.Content.ReadAsAsync<EmployeeDto>().Result;
            Debug.WriteLine("employee received : ");
            Debug.WriteLine(selectedEmployee.FirstName);




            return View(selectedEmployee);
        }

        /// <summary>
        /// This method communicate with the employee data api and get the departments options and position options  and  show them a webpage where a new employee can be created
        /// </summary>
        /// <returns>
        /// Returns  a view which prompts to create a new employee
        /// </returns>
        /// <example>  GET: Employee/New => New View => (this webpage gives a form with an empty input fields where new user's information can be filled)
        /// </example>

        [Authorize]
        public ActionResult New()
        {
            AddEmployee ViewModel= new AddEmployee();
            //information about all departments in the system.
            //GET api/DepartmentData/ListDepartments
            string url = "DepartmentData/ListDepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> departmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            ViewModel.DepartmentOptions = departmentOptions;

            //information about the positions in the system
            url = "PositionData/ListPositions";
            response = client.GetAsync(url).Result;
            IEnumerable<PositionDto> positonOptions = response.Content.ReadAsAsync<IEnumerable<PositionDto>>().Result;
            ViewModel.PositionOptions= positonOptions;
            return View(ViewModel);

        }

        /// <summary>
        /// This method communicate with the AddEmployee method in the employee data api , pass on the new employer's infomation to this method to create a new employee in the database
        /// </summary>
        /// <param name="employee">The  Employee object with new employee's information </param>
        /// <returns>
        /// if the infromation is processed successfully redirects to the List View 
        /// else directs to the Error View page 
        /// </returns>
        /// <example>  POST: Employee/Create
        /// FORM DATA: Employee JASON Object
        /// </example>

        [HttpPost]
        [Authorize]
        public ActionResult Create(Employee employee)
        {
            GetApplicationCookie();
           // Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(employee.FirstName);
            
       
            string url = "EmployeeData/AddEmployee";


            string jsonpayload = jss.Serialize(employee);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

           
        }
        // GET : Employee/Error - > Directs to the Error View showing the Error message
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// This method communicates with the employee data api ,  fetch the  stored informtaion of an employee and  directs  it to the edit page  where it can be updated
        /// </summary>
        /// <param name="id"> The id of an employee </param>
        /// <returns>
        ///  GET: Employee/Edit/5 => Edit View with the previous information of an employee showing on the page 
        /// </returns>

        // GET: Employee/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            UpdateEmployee ViewModel = new UpdateEmployee();

            //the existing employee information
            string url = "EmployeeData/FindEmployee/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            EmployeeDto SelectedEmployee = response.Content.ReadAsAsync<EmployeeDto>().Result;
            ViewModel.SelectedEmployee = SelectedEmployee;
            SelectedEmployee.PreviousDepartmentId= SelectedEmployee.DepartmentId;
           SelectedEmployee.PreviousPositionId = SelectedEmployee.PositionId;
            Debug.WriteLine("Previous Position Id:" + SelectedEmployee.PreviousPositionId);

            // Departments to choose from when updating this employee
            url = "DepartmentData/ListDepartments";
            response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            ViewModel.DepartmentOptions = DepartmentOptions;

            // Positions to choose from when updating this employee
            url = "PositionData/ListPositions";
            response = client.GetAsync(url).Result;
            IEnumerable<PositionDto> PositionOptions = response.Content.ReadAsAsync<IEnumerable<PositionDto>>().Result;
            ViewModel.PositionOptions = PositionOptions;

            return View(ViewModel);
        }

        /// <summary>
        /// This method communicates with UpdateEmployee  method in the employee data api, pass on the updatd information about an employee and update it in the database
        /// </summary>
        /// <param name="id"> The id of an employee </param>
        ///<param name="employee"> The employee object with the updated information </param>
        ///<returns>
        ///if the informtion is processed succssfully redircts to the List View 
        ///else it  directs to the Error View
        /// </returns>
        /// <example>
        ///  POST: Employee/Update/5   - > update an employee with employee id of 5 with the updated information by communicating wih the UpdateEmployee method in the Employee data Api and redirects it to the List view 
        /// FORM DATA - Employee JASON  Object
        /// </example>

        // POST: Employee/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Employee employee)
        {

            GetApplicationCookie();
            // Debug.WriteLine("Update function accessed");
            string url = "EmployeeData/UpdateEmployee/" + id;
            //Debug.WriteLine("id :" +id);
            //Debug.WriteLine("employeeid :" + employee.EmployeeId);
            string jsonpayload = jss.Serialize(employee);
            //Debug.WriteLine(jsonpayload);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
           // Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <summary>
        /// This method communicates with the Employee Data Api and redirects to the DeleteConfirm View where it confirms with user  before deleting an employee
        /// </summary>
        /// <param name="id">The id of en employee who is requested to be deleted from te database</param>
        /// <returns>
        ///  Directs to DeleteConfrim View  prompting user to confirm the deletion of an employee
        /// </returns>
        /// <example>
        /// GET: Employee/Delete/5 - >   Directs to DeleteConfrim View 
        /// </example>


        // GET: Employee/Delete/5
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "EmployeeData/FindEmployee/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            EmployeeDto selectedEmployee = response.Content.ReadAsAsync<EmployeeDto>().Result;
            return View(selectedEmployee);
        }

        /// <summary>
        /// This method communicates with the DeleteEmployee mehtod in the  Employee Data Api and  Deletes the particular  employee from the database
        /// </summary>
        /// <param name="id">The id of an employee to be deleted </param>
        /// <returns>
        ///if the informtion is processed succssfully redircts to the List View 
        ///else it  directs to the Error View
        /// </returns>
        /// <example>
        /// POST: Employee/Delete/5 => deletes an employee with employee id 5 by communicating  with the DeleteEmployee method in the Employee data api and redirects to the List View  
        /// </example>


        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
            string url = "EmployeeData/DeleteEmployee/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType= "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;


            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
