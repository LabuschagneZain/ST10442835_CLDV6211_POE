using ManagerApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerApp.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Event
        public async Task<IActionResult> Index(int? eventTypeId, int? venueId, DateTime? startDate, DateTime? endDate, bool? venueAvailable)
        {
            // Populate dropdowns
            ViewBag.EventTypes = new SelectList(await _context.EventTypes.ToListAsync(), "EventTypeId", "TypeName", eventTypeId);
            ViewBag.Venues = new SelectList(await _context.Venues.ToListAsync(), "VenueId", "VenueName", venueId);

            ViewBag.VenueAvailability = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "All", Value = "" },
                new SelectListItem { Text = "Available", Value = "true" },
                new SelectListItem { Text = "Unavailable", Value = "false" }
            }, "Value", "Text", venueAvailable?.ToString().ToLower());

            // Build query
            var query = _context.Events
                .Include(e => e.Venue)
                .Include(e => e.EventType)
                .AsQueryable();

            if (eventTypeId.HasValue && eventTypeId.Value != 0)
                query = query.Where(e => e.EventTypeId == eventTypeId.Value);

            if (venueId.HasValue && venueId.Value != 0)
                query = query.Where(e => e.VenueId == venueId.Value);

            if (startDate.HasValue)
                query = query.Where(e => e.EventDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(e => e.EventDate <= endDate.Value);

            if (venueAvailable.HasValue)
                query = query.Where(e => e.Venue != null && e.Venue.Availability == venueAvailable.Value);

            var events = await query.ToListAsync();

            return View(events);
        }

        // GET: Event/Create
        public IActionResult Create()
        {
            // Populate dropdown lists before showing the form
            ViewBag.Venues = _context.Venues.ToList();
            ViewBag.EventTypes = _context.EventTypes.ToList();
            return View();
        }

        // POST: Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Event created successfully.";
                return RedirectToAction(nameof(Index));
            }

            // If ModelState is invalid, repopulate dropdown lists to avoid null reference error
            ViewBag.Venues = _context.Venues.ToList();
            ViewBag.EventTypes = _context.EventTypes.ToList();
            return View(@event);
        }

        // GET: Event/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events
                .Include(e => e.Venue)
                .Include(e => e.EventType)
                .Include(e => e.Bookings)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (@event == null) return NotFound();

            return View(@event);
        }

        // GET: Event/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return NotFound();

            ViewBag.Venues = new SelectList(_context.Venues, "VenueId", "VenueName", @event.VenueId);
            ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "TypeName", @event.EventTypeId);

            return View(@event);
        }

        // POST: Event/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,EventName,EventDate,Description,VenueId,EventTypeId")] Event @event)
        {
            if (id != @event.EventId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Event updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Events.Any(e => e.EventId == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Venues = new SelectList(_context.Venues, "VenueId", "VenueName", @event.VenueId);
            ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "TypeName", @event.EventTypeId);

            return View(@event);
        }

        // GET: Event/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events
                .Include(e => e.Venue)
                .Include(e => e.EventType)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (@event == null) return NotFound();

            return View(@event);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return NotFound();

            var isBooked = await _context.Bookings.AnyAsync(b => b.EventId == id);
            if (isBooked)
            {
                TempData["ErrorMessage"] = "Cannot delete event because it has existing bookings.";
                return RedirectToAction(nameof(Index));
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Event deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
