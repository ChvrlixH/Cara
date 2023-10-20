using Cara.Core.Entities;
using Cara.DataAccess.Repositories.Interfaces;
using Cara.WebUI.Areas.Admin.ViewModels.Address;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cara.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class AddressController : Controller
    {
        private readonly IAddressRepository _repository;

        public AddressController(IAddressRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetAllAsync());
        }

        public async Task<IActionResult> Detail(int id)
        {
            var addressItem = await _repository.GetAsync(id);
            if (addressItem == null) { return NotFound(); }
            return View(addressItem);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddressVM addressVM)
        {
            if (!ModelState.IsValid)
            {
                return View(addressVM);
            }

            if (addressVM.IsActive)
            {
                var activeaddress = await _repository.GetActiveAddressAsync();
                if (activeaddress != null)
                {
                    activeaddress.IsActive = false;
                }
            }

            Address newAddress = new Address
            {
                HeadTitle = addressVM.HeadTitle,
                Title = addressVM.Title,
                SubTitle = addressVM.SubTitle,
                Map = addressVM.Map,
                Email = addressVM.Email,
                Phone = addressVM.Phone,
                Time = addressVM.Time,
                IsActive = addressVM.IsActive
            };

            _repository.CreateAsync(newAddress);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) { return NotFound(); }
            Address address = await _repository.GetAsync(id);
            if (address == null) { return NotFound(); }

            AddressUpdateVM addressUpdateVM = new()
            {
				HeadTitle = address.HeadTitle,
				Title = address.Title,
				SubTitle = address.SubTitle,
				Map = address.Map,
				Email = address.Email,
				Phone = address.Phone,
				Time = address.Time,
				IsActive = address.IsActive
			};

            return View(addressUpdateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, AddressUpdateVM addressUpdateVM)
        {
			if (id != addressUpdateVM.Id) return BadRequest();
			if (!ModelState.IsValid) return View(addressUpdateVM);
            Address address = await _repository.GetAsync(id);
            if (address == null) { return NotFound(); }

            if (addressUpdateVM.IsActive)
            {
                var activeaddress = await _repository.GetActiveAddressAsync();
                if (activeaddress != null)
                {
                    activeaddress.IsActive = false;
                }
            }

            address.HeadTitle = addressUpdateVM.HeadTitle;
            address.Title = addressUpdateVM.Title;
            address.SubTitle = addressUpdateVM.SubTitle;
            address.Map = addressUpdateVM.Map;
            address.Email = addressUpdateVM.Email;
            address.Phone = addressUpdateVM.Phone;
            address.Time = addressUpdateVM.Time;
            address.IsActive = addressUpdateVM.IsActive;

			_repository.Update(address);
			await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) { return NotFound(); }
            Address address = await _repository.GetAsync(id);
            if (address == null) { return NotFound(); }
            return View(address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null) { return NotFound(); }
            Address address = await _repository.GetAsync(id);
            if (address == null) { return NotFound(); }

            _repository.Delete(address);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
