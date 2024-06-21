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
    public class PositionController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PositionController()
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
        /// This method communicate with the Position Data api and get the list of positions and show them on the webpage 
        /// </summary>
        /// <returns>
        /// Returns  a view with the list of positions
        /// </returns>
        /// <example>  GET: Position/List => List View (with the list of  positions)
        /// </example>

        // GET: Position/List
        [HttpGet]
        public ActionResult List()
        {
            string url = "PositionData/ListPositions";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<PositionDto> positions = response.Content.ReadAsAsync<IEnumerable<PositionDto>>().Result;
            //Debug.WriteLine("Number of  positions received : ");
            //Debug.WriteLine(positions.Count());
            return View(positions);
           
        }

        /// <summary>
        /// This method communicate with the FindPosition method in the  position data api , get the infomartion about the particular  position and show it on the webpage 
        /// </summary>
        /// <param name="id">The id of a position whose information  is requested </param>
        /// <returns>
        /// Returns  a view with the information about a particular Position
        /// </returns>
        /// <example>  GET: Position/Details/5 => Details View( The details of a requested position with the position id of 5)
        /// </example>

        // GET: Position/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            // fetching an information about the particular position

            DetailsPosition ViewModel = new DetailsPosition();
            string url = "PositionData/FindPosition/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            PositionDto selectedPosition = response.Content.ReadAsAsync<PositionDto>().Result;

            Debug.WriteLine("Position received : ");
            Debug.WriteLine(selectedPosition.PositionTitle);
            ViewModel.SelectedPosition= selectedPosition;

            //showcase information about  employees related to this  position
            url = "EmployeeData/ListEmployeesForPosition/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<EmployeeDto> relatedEmployees = response.Content.ReadAsAsync<IEnumerable<EmployeeDto>>().Result;
            ViewModel.RelatedEmployees = relatedEmployees;

            return View(ViewModel);
            
        }
        // GET: Position/Error => Directs to the Error View showing the Error message
        public ActionResult Error()
        {
            return View();
        }

        // GET: Position/New -> Directs to the New View  prompting the user to add information about  the new position
        [Authorize]
        public ActionResult New()
        {
            return View();
        }

        /// <summary>
        /// This method communicate with the AddPosition method in the PositionData api , pass on the new position's infomation to this method to create a new position in the database
        /// </summary>
        /// <param name="position">The  Department object with new position's information </param>
        /// <returns>
        /// if the infromation is processed successfully redirects to the List View 
        /// else directs to the Error View page 
        /// </returns>
        /// <example>  POST: Position/Create
        /// FORM DATA: Position JASON Object
        /// </example>

        // POST: Position/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Position position)
        {
            GetApplicationCookie();
            // Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(position.PositionTitle);


            string url = "PositionData/AddPosition";


            string jsonpayload = jss.Serialize(position);

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


        //---------Change position of a employee to associate it with the new Position--------------  

        /// <summary>
        /// This method communicates with the AssociateEmployeeToPosition Method in the Employee data  api  and assign  the employees  to  the particular  position by changing their current position Id
        /// </summary>
        /// <param name="id">The Position Id </param>
        /// <param name="EmployeeId">The employee Id of an employee who is  assigned   to  the  position</param>
        /// <returns>Directs it  to the Same Edit page for the Same Position wtih the Employee being upadted/Assigned into the Position </returns>
        /// <example>
        /// POST : Position/Associate/5/6=> The employee with the employee id of  6 being assigned to  the  position with  position id of 5
        /// </example>
        //POST: Position/Associate/{PositionId}{EmployeeId}

        [HttpPost]
        [Authorize]
        public ActionResult Associate(int id, int EmployeeId)
        {
            GetApplicationCookie();
            Debug.WriteLine("Employee Id" + EmployeeId);
            Debug.WriteLine("Position Id" + id);
            //call to FindEmployee Function
            // string URL = "EmployeeData/FindEmployee/" + EmployeeId;
            // HttpResponseMessage Response = client.GetAsync(URL).Result;
            // EmployeeDto SelectedEmployee = Response.Content.ReadAsAsync<EmployeeDto>().Result;

            // SelectedEmployee.PreviousDepartmentId = SelectedEmployee.DepartmentId;


            //call our api to associate animal with keeper
            string url = "EmployeeData/AssociateEmployeeToPosition/" + EmployeeId + "/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Edit/" + id);
            }
            else
            {
                return RedirectToAction("Error");
            }


        }


        //---------Change  Position of an employee to unassociate it with the  current Position and assign it again to  the previous  Position--------------  

        /// <summary>
        /// This method communicates with the UnAssociateEmployeeToPosition Method in the Position data api  and  remove the employees associatd  with  the current position by changing their current  position Id to it's stored previous  position id 
        /// </summary>
        /// <param name="id">The Position Id </param>
        /// <param name="EmployeeId">The employee Id of an employee who is unassigned with the Position</param>
        /// <returns>Directs it  to the Same Edit page for the Same Position wtih the Employee being updated/ unassigned with  the Position</returns>
        /// <example>
        /// GET : Position/UnAssociate/5/6=> The employee with the employee id of  6 being unassigned  from  the Position with  Position id of 5
        /// </example>
        //Get: Position/UnAssociate/{id}?EmployeeId={EmployeeId}

        [HttpGet]
        [Authorize]
        public ActionResult UnAssociate(int id, int EmployeeId)
        {
            GetApplicationCookie();
            // Debug.WriteLine("Employee Id" + EmployeeId);
            Debug.WriteLine("Employee Id" + EmployeeId);
            Debug.WriteLine("Position Id" + id);
            //call our api to unassociate employee with  Position
            string url = "EmployeeData/UnAssociateEmployeeToPosition/" + EmployeeId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("Edit/" + id);
            }
            else
            {
                return RedirectToAction("Error");
            }


        }


        /// <summary>
        /// This method communicates with the PositionData api ,  fetch the  stored informtaion of a Position and  directs  it to the edit page  where it can be updated
        /// </summary>
        /// <param name="id"> The id of a position </param>
        /// <returns>
        ///  GET: Position/Edit/5 => dircets it to the Edit View with the previous information of a position showing on the page 
        /// </returns>


        // GET: Position/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
           
            UpdatePosition ViewModel = new UpdatePosition();
            //the existing position information
            string url = "PositionData/FindPosition/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PositionDto selectedPosition = response.Content.ReadAsAsync<PositionDto>().Result;
           ViewModel.SelectedPosition = selectedPosition;
            //showcase information about the related employees  with  this position
            url = "EmployeeData/ListEmployeesForPosition/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<EmployeeDto> relatedEmployees = response.Content.ReadAsAsync<IEnumerable<EmployeeDto>>().Result;
            ViewModel.RelatedEmployees = relatedEmployees;


            // showcase infomation about the employees not related to the  position
            url = "EmployeeData/ListEmployeesNotHoldingPosition/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<EmployeeDto> UnrelatedEmployees = response.Content.ReadAsAsync<IEnumerable<EmployeeDto>>().Result;
            ViewModel.UnrelatedEmployees = UnrelatedEmployees;
            return View(ViewModel);
            
        }

        /// <summary>
        /// This method communicates with UpdatePosition  method in the PositionData api, pass on the updatd information about a position and update it in the database
        /// </summary>
        /// <param name="id"> The id of  a position </param>
        ///<param name="position"> The Position object with the updated information </param>
        ///<returns>
        ///if the informtion is processed succssfully redircts to the List View 
        ///else it  directs to the Error View
        /// </returns>
        /// <example>
        ///  POST: Position/Update/5   - > update the position with position id of 5 with the updated information by communicating wih the UpdatePosition method in the PositionData Api and redirects it to the List view 
        /// FORM DATA - Position JASON  Object
        /// </example>

        // POST: Position/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Position position)
        {
            GetApplicationCookie();
            // Debug.WriteLine("Update function accessed");
            string url = "PositionData/UpdatePosition/" + id;
            //Debug.WriteLine("id :" +id);
            //Debug.WriteLine("positionid :" + position.PositionId);
            string jsonpayload = jss.Serialize(position);
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
        /// This method communicates with the PositionData Api and redirects to the DeleteConfirm View where it confirms with user  before deleting a  position
        /// </summary>
        /// <param name="id">The id of position which  is requested to be deleted from the database</param>
        /// <returns>
        ///  Directs to DeleteConfrim View  prompting user to confirm the deletion of a position
        /// </returns>
        /// <example>
        /// GET: Position/DeleteConfirm/5 - >   Directs to DeleteConfrim View prompting to confirm the deletion of position with position id of 5
        /// </example>


        // GET: Position/DeleteConfirm/5
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "PositionData/FindPosition/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PositionDto selectedPosition = response.Content.ReadAsAsync<PositionDto>().Result;
            return View(selectedPosition);
        }

        /// <summary>
        /// This method communicates with the DeletePosition mehtod in the  PositionData Api and Deletes the particular position from the database
        /// </summary>
        /// <param name="id">The id of a Position to be deleted </param>
        /// <returns>
        ///if the informtion is processed succssfully redircts to the List View 
        ///else it  directs to the Error View
        /// </returns>
        /// <example>
        /// POST: Position/Delete/5 => deletes a  position with Position id 5 by communicating  with the DeletePosition method in the PositionData api and redirects to the List View  
        /// </example>

        // POST: Position/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
            string url = "PositionData/DeletePosition/" + id;
            HttpContent content = new StringContent("");
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
    }
}
