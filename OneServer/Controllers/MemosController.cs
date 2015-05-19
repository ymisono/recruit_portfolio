using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using OneServer.Models;

namespace OneServer.Controllers
{
    public class MemosController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Memos
        public IQueryable<Memo> GetMemos()
        {
            return db.Memos;
        }

        // GET: api/Memos/5
        [ResponseType(typeof(Memo))]
        public async Task<IHttpActionResult> GetMemo(int id)
        {
            Memo memo = await db.Memos.FindAsync(id);
            if (memo == null)
            {
                return NotFound();
            }

            return Ok(memo);
        }

        // PUT: api/Memos/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMemo(int id, Memo memo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != memo.Id)
            {
                return BadRequest();
            }

            db.Entry(memo).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemoExists(id))
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

        // POST: api/Memos
        [ResponseType(typeof(Memo))]
        public async Task<IHttpActionResult> PostMemo(Memo memo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Memos.Add(memo);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = memo.Id }, memo);
        }

        // DELETE: api/Memos/5
        [ResponseType(typeof(Memo))]
        public async Task<IHttpActionResult> DeleteMemo(int id)
        {
            Memo memo = await db.Memos.FindAsync(id);
            if (memo == null)
            {
                return NotFound();
            }

            db.Memos.Remove(memo);
            await db.SaveChangesAsync();

            return Ok(memo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MemoExists(int id)
        {
            return db.Memos.Count(e => e.Id == id) > 0;
        }
    }
}