using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using PassionProjectSummer2024.Models;

namespace PassionProjectSummer2024.Controllers
{
    public class PositionDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// This method lists the number of positions in the database
        /// </summary>
        /// <returns>An array of position objects</returns>
        /// <example>
        /// // GET: api/PositionData/ListPositions =>
        /// [{"PositionId":6,"PositionTitle":"L1- Warehouse Associate","HourlyWage":30.00,"PreviousPositionId":1}]
        /// OR using command prompt 
        /// curl https://localhost:44355/api/PositionData/ListPositions =>
        /// [{"PositionId":6,"PositionTitle":"L1- Warehouse Associate","HourlyWage":30.00,"PreviousPositionId":1}]
        /// </example>


        // GET: api/PositionData/ListPositions
        [System.Web.Http.HttpGet]
        [ResponseType(typeof(PositionDto))]
        public IHttpActionResult ListPositions()
        {
            List<Position> Positions = db.Positions.ToList();
            List<PositionDto> PositionDtos = new List<PositionDto>();

            Positions.ForEach(p => PositionDtos.Add(new PositionDto()
            {
                PositionId = p.PositionId,
                PositionTitle = p.PositionTitle,
                HourlyWage = p.HourlyWage,
               
            }));
            return Ok(PositionDtos);
        }


        /// <summary>
        /// This method provides/fetch  the information about a particular position from the database
        /// </summary>
        /// <param name="id"> id refres to the PositionId of a Position whose information is requested</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A Position in the system matching up to the  PositionId primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        ///  // GET: api/PositinData/FindPosition/2=> [{"PositionId":2,"PositionTitle":"L2- Process Assistant","HourlyWage":35.00,"PreviousPositionId":1}],
        ///  OR using command prompt
        ///  curl https://localhost:44355/api/PositionData/FindPosition/2 =>
        ///  [{"EmployeeId":19,"FirstName":"Amanpreet","LastName":"Kaur","HireDate":"2024-05-28T04:00:00","DepartmentId":3,"DepartmentName":"Outbound Pack","PositionId":6,"PositionTitle":"L2- Process Assistant","PreviousDepartmentId":3,"PreviousPositionId":0},
        /// </example>

        // GET: api/PositionData/FindPosition/5
        [System.Web.Http.HttpGet]
        [ResponseType(typeof(PositionDto))]
        public IHttpActionResult FindPosition(int id)
        {
            Position position = db.Positions.Find(id);
            PositionDto positionDto = new PositionDto()
            {
                PositionId = position.PositionId,
                PositionTitle = position.PositionTitle,
                HourlyWage = position.HourlyWage,
            };
            if (position == null)
            {
                return NotFound();
            }

            return Ok(positionDto);
        }

        /// <summary>
        /// This method updates the infomation about the current position in the database
        /// </summary>
        /// <param name="id"> The id of a position whose information needs to be updated</param>
        /// <param name="position">JSON FORM DATA of a Position </param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>>curl -d @position.json -H "Content-type:application/json"  https://localhost:44355/api/PositionData/UpdatePosition/9 => updates the informatio of a position with PositionId =9 with the updated informtion listed in the position.json file
        /// POST: api/PositionData/UpdatePosition/9
        /// FORM DATA: Position JASON Object
        /// </example>


        // PUT: api/PositionData/UpdatePosition/5
        [System.Web.Http.HttpPost]
        [ResponseType(typeof(void))]
        [System.Web.Http.Authorize]
        public IHttpActionResult UpdatePosition(int id, Position position)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != position.PositionId)
            {
                return BadRequest();
            }

            db.Entry(position).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PositionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        /// <summary>
        /// This method adds the new position into the database
        /// </summary>
        /// <param name="position"> JSON FORM DATA of an Position</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Position ID, Position Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>>curl -d @position.json -H "Content-type:application/json"  https://localhost:44355/api/PositionData/AddPosition => adds the new position object listed in the position.json file 
        /// POST: api/PositionData/AddPosition
        /// FORM DATA: Position JSON Object
        /// </example>

        // POST: api/PositionData/AddPosition
        [System.Web.Http.HttpPost]
        [ResponseType(typeof(Position))]
        [System.Web.Http.Authorize]
        public IHttpActionResult AddPosition(Position position)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // adds the  position into the database
            db.Positions.Add(position);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = position.PositionId }, position);
        }


        /// <summary>
        /// This method deletes the specific  position from the database by providing the id of a position as a parameter 
        /// </summary>
        /// <param name="id">The id of a position to be deleted</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example> Post: api/PositionData/DeletePosition/8  => deletes the position from the database having id = 8
        /// FORM DATA: (empty)
        /// curl -d ""  https://localhost:44355/api/PositionData/DeletePosition/8 =>deletes the position from the database having id = 8
        /// </example>
        // DELETE: api/PositionData/DeletePosition/5
        [System.Web.Http.HttpPost]
        [ResponseType(typeof(Position))]
        [System.Web.Http.Authorize]
        public IHttpActionResult DeletePosition(int id)
        {
            Position position = db.Positions.Find(id);
            if (position == null)
            {
                return NotFound();
            }

            db.Positions.Remove(position);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PositionExists(int id)
        {
            return db.Positions.Count(e => e.PositionId == id) > 0;
        }
    }
}