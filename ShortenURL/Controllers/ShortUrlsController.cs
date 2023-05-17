using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShortenURL.Data;
using ShortenURL.Models;

namespace ShortenURL.Controllers
{
    public class ShortUrlsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShortUrlsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ShortUrls
        public async Task<IActionResult> Index()
        {
              
            return _context.ShortUrl != null ? 
                          View(await _context.ShortUrl.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ShortUrl'  is null.");
        }

        // GET: ShortUrls/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.ShortUrl == null)
            {
                return NotFound();
            }

            var shortUrl = await _context.ShortUrl
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shortUrl == null)
            {
                return NotFound();
            }

            return View(shortUrl);
        }

        // GET: ShortUrls/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ShortUrls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Url,ShorUrl,Description")] ShortUrl shortUrl)
        {
            if (ModelState.IsValid)
            {
                var request = HttpContext.Request;
                string baseUrl= request.Scheme + "://" +request.Host.Value + "/";
                

                if (shortUrl.ShorUrl == null) { 
                Random random = new Random();
                string characters = "abcdefghijklmnopqrstuvwxyz0123456789_-";

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 9; i++)
                {
                    int randomIndex = random.Next(characters.Length);
                    sb.Append(characters[randomIndex]);
                }

                shortUrl.ShorUrl = sb.ToString();
                }
                
                shortUrl.ShorUrl = baseUrl + shortUrl.ShorUrl;

                shortUrl.Id = Guid.NewGuid();
                _context.Add(shortUrl);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shortUrl);
        }

       /* // GET: ShortUrls/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.ShortUrl == null)
            {
                return NotFound();
            }

            var shortUrl = await _context.ShortUrl.FindAsync(id);
            if (shortUrl == null)
            {
                return NotFound();
            }
            return View(shortUrl);
        }

        // POST: ShortUrls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Url,ShorUrl,Description")] ShortUrl shortUrl)
        {
            if (id != shortUrl.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shortUrl);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShortUrlExists(shortUrl.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(shortUrl);
        }*/

        // GET: ShortUrls/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.ShortUrl == null)
            {
                return NotFound();
            }

            var shortUrl = await _context.ShortUrl
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shortUrl == null)
            {
                return NotFound();
            }

            return View(shortUrl);
        }

        // POST: ShortUrls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.ShortUrl == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ShortUrl'  is null.");
            }
            var shortUrl = await _context.ShortUrl.FindAsync(id);
            if (shortUrl != null)
            {
                _context.ShortUrl.Remove(shortUrl);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ShortUrls/{short_url}
         [Route("/{short_url}")]
         [HttpGet]
         public async Task<IActionResult> GetShortenedURL(string short_url)
         {
            //var shortUrl =  await _context.ShortUrl.FindAsync(short_url);

            var request = HttpContext.Request;
            string baseUrl = request.Scheme + "://" + request.Host.Value + "/";
            short_url =  baseUrl + short_url;

            var url =  await _context.ShortUrl.FirstOrDefaultAsync(u => u.ShorUrl == short_url);
            if (url != null)
             {
                 return Redirect(url.Url);
             }
             else
             {
                return NotFound();
             }
         }


        private bool ShortUrlExists(Guid id)
        {
          return (_context.ShortUrl?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
