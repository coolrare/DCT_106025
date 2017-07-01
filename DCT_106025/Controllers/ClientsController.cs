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
using DCT_106025.Models;
using DCT_106025.ActionFilters;
using System.Web.Http.Results;
using System.Web.Http.Cors;

namespace DCT_106025.Controllers
{
    [EnableCors("*", "*", "*")]
    public class ClientsController : ApiController
    {
        private FabricsEntities db = new FabricsEntities();

        public ClientsController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }

        // GET: api/Clients
        public IQueryable<Client> GetClient()
        {
            return db.Client;
        }

        // GET: api/Clients/5
        [ResponseType(typeof(Client))]
        [Route("clients/{id:int}", Name = "GetClientById")]
        public HttpResponseMessage GetClient(int id)
        {
            Client client = db.Client.Find(id);

            if (client == null)
            {
                return new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            return Request.CreateResponse(HttpStatusCode.OK, client);
        }

        [Route("clients/{id:int}/orders")]
        public IHttpActionResult GetOrdersByClientId(int id)
        {
            var orders = db.Order
                .Where(p => p.ClientId == id);

            if (!orders.Any())
            {
                return RedirectToRoute("GetClientById", new { id = id });
            }

            return Ok(orders);
        }

        [Route("clients/{id:int}/orders/{oid:int}")]
        public IHttpActionResult GetOrdersByClientIdOrderId(int id, int oid)
        {
            var orders = db.Order
                .Where(p => p.ClientId == id && p.OrderId == oid);
            return Ok(orders);
        }

        [Route("clients/{id:int}/orders/{status:alpha:length(1)}")]
        public IHttpActionResult GetOrdersByClientIdOrderStatus(int id, string status)
        {
            var orders = db.Order
                .Where(p => p.ClientId == id && p.OrderStatus == status);
            return Ok(orders);
        }

        [Route("clients/{id:int}/orders/{*odate:datetime}")]
        public IHttpActionResult GetOrdersByClientIdOrderDate(int id, DateTime odate)
        {
            var orders = db.Order
                .Where(p => p.ClientId == id && p.OrderDate > odate);
            return Ok(orders);
        }


        // PUT: api/Clients/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClient(int id, Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != client.ClientId)
            {
                return BadRequest();
            }

            db.Entry(client).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // POST: api/Clients
        [ResponseType(typeof(Client))]
        [Route("clients")]
        [MyException]
        public IHttpActionResult PostClient(Client client)
        {
            db.Client.Add(client);
            db.SaveChanges();

            return CreatedAtRoute("GetClientById", new { id = client.ClientId }, client);
        }

        // DELETE: api/Clients/5
        [ResponseType(typeof(Client))]
        public IHttpActionResult DeleteClient(int id)
        {
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            db.Client.Remove(client);
            db.SaveChanges();

            return Ok(client);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClientExists(int id)
        {
            return db.Client.Count(e => e.ClientId == id) > 0;
        }
    }
}