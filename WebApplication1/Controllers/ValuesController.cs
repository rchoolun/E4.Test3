using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public HttpResponseMessage Get()
        {
            var business = new Business.Business();
            var userList =  business.GetUsers();

            if(userList != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, userList);
            }

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error fetching Students");
        }

        // GET api/values/5
        public HttpResponseMessage Get(string id)
        {
            //parsing the GUID to see if a valid ID has been sent
            if (!Guid.TryParse(id, out var newGuid))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Guid cannot be parsed" + id.ToString());
            }

            var business = new Business.Business();
            var user = business.GetUserById(newGuid);

            if(user != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, user);
            }

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Student With id = " + id.ToString() + " not found");
        }

        // POST api/values
        public HttpResponseMessage Post([FromBody] Student student)
        {
            HttpResponseMessage message;

            if(student != null)
            {
                Status status;
                var business = new Business.Business();

                if(student.Id != null && student.Id != Guid.Empty)
                {
                    status = business.UpdateStudent(student);
                }
                else
                {
                    student.Id = Guid.NewGuid();
                    status = business.AddStudent(student);
                }

                if(status.IsSuccess)
                {
                    message = Request.CreateResponse(HttpStatusCode.Created, student);
                    message.Headers.Location = new Uri(Request.RequestUri + student.Id.ToString());
                    return message;
                }

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, status.Message);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Object cannot be NULL");
        }

        // DELETE api/values/5
        public HttpResponseMessage Delete(string id)
        {
            //parsing the GUID to see if a valid ID has been sent
            if (!Guid.TryParse(id, out var newGuid))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Guid cannot be parsed" + id.ToString());
            }

            var business = new Business.Business();
            var status = business.RemoveStudent(newGuid);

            if (status.IsSuccess)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, status.Message);
        }

    }
}
