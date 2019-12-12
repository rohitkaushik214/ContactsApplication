using System;
using System.Net.Http;
using System.Web.Http;
using ContactsApplication.Repository;
using ContactsApplication.Domain;
using log4net;

namespace ContactsApplication.Controllers
{
    [RoutePrefix("api/Contacts")]
    public class ContactsController : ApiController
    {
        private IContactsRepository contactsRepository;
        private ILog _log;

        public ContactsController(IContactsRepository contactsRepository, ILog log)
        {
            this.contactsRepository = contactsRepository;
            this._log = log;
        }

        [HttpGet]
        [Route("test")]
        public string Test()
        {
            _log.Info("Test controller hit.");
            return "Contacts controller hit.";
        }

        [HttpGet]
        [Authorize]
        [Route("GetContact/{id}")]
        public IHttpActionResult GetContact(int id)
        {
            var contact = new Contact();
            try
            {
                contact = this.contactsRepository.GetContactById(id);

                if (contact == null)
                {
                    ModelState.AddModelError("request.id", "Invalid ID");

                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error occured in GetContact method:", ex);
                ModelState.AddModelError("request", "Some error has occured.");
                return BadRequest(ModelState);
            }

            return Ok(contact);
        }

        [HttpGet]
        [Authorize]
        [Route("GetAllContacts/{offset?}")]
        public IHttpActionResult GetAllContacts(int offset = 0)
        {
            var contactsResponse = new GetContactsResponse();
            try
            {
                contactsResponse = this.contactsRepository.GetContacts(offset);
                if (contactsResponse.offset == 0 && contactsResponse.Contacts == null)
                {
                    ModelState.AddModelError("request.offset", "Invalid error");
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error occured in GetAllContacts method:", ex);
                ModelState.AddModelError("request", "Some error has occured.");
                return BadRequest(ModelState);
            }

            return Ok(contactsResponse);
        }

        [HttpPost]
        [Authorize]
        [Route("AddContact")]
        public IHttpActionResult AddContact([FromBody]RequestContact contactRequest)
        {
            try
            {
                string result = ValidateContact(contactRequest);
                if (!string.IsNullOrEmpty(result))
                {
                    ModelState.AddModelError("request", result);
                    return BadRequest(ModelState);
                }

                var contact = new Contact();
                contact.FirstName = contactRequest.FirstName;
                contact.LastName = contactRequest.LastName;
                contact.Email = contactRequest.Email;
                contact.PhoneNumber = contactRequest.PhoneNumber;
                contact.Status = true;

                this.contactsRepository.AddContact(contact);
            }
            catch (Exception ex)
            {
                _log.Error("Error occured in AddContact method:", ex);
                ModelState.AddModelError("request", "Some error has occured.");
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("Inactive/{contactId}")]
        public IHttpActionResult ContactInactive(int contactId)
        {
            try
            {
                var contact = this.contactsRepository.GetContactById(contactId);
                UpdateContactStatus(contact, false);
            }
            catch (Exception ex)
            {
                _log.Error("Error occured in ContactInactive method:", ex);
                ModelState.AddModelError("request", "Some error has occured.");
                return BadRequest(ModelState);
            }

            return Ok("Contact marked inactive.");
        }

        [HttpGet]
        [Authorize]
        [Route("Active/{contactId}")]
        public IHttpActionResult ContactActive(int contactId)
        {
            try
            {
                var contact = this.contactsRepository.GetContactById(contactId);
                UpdateContactStatus(contact, true);
            }
            catch (Exception ex)
            {
                _log.Info("Error occured in ContactActive method:", ex);
                ModelState.AddModelError("request", "Some error has occured.");
                return BadRequest(ModelState);
            }

            return Ok("Contact marked active.");
        }


        public void UpdateContactStatus(Contact contact, bool IsActive)
        {
            contact.Status = IsActive;
            this.contactsRepository.UpdateContact(contact);
        }

        public string ValidateContact(RequestContact contact)
        {
            if (string.IsNullOrEmpty(contact.FirstName))
            {
                return "First name cannot be empty.";
            }
            if (string.IsNullOrEmpty(contact.LastName))
            {
                return "Last name cannot be empty.";
            }
            if (string.IsNullOrEmpty(contact.Email))
            {
                return "Email cannot be empty.";
            }
            if (string.IsNullOrEmpty(contact.PhoneNumber))
            {
                return "PhoneNumber cannot be empty.";
            }

            return string.Empty;
        }


        [HttpPost]
        [Authorize]
        [Route("edit/{contactId}")]
        public IHttpActionResult EditContact(int contactId, [FromBody]RequestContact contactRequest)
        {
            try
            {
                var contact = this.contactsRepository.GetContactById(contactId);
                if (contact == null)
                {
                    ModelState.AddModelError("request", "Contact with the provided Id is not present.");
                    return BadRequest(ModelState);
                }
                if (!string.IsNullOrEmpty(contactRequest.LastName))
                    contact.LastName = contactRequest.LastName;
                if (!string.IsNullOrEmpty(contactRequest.PhoneNumber))
                    contact.PhoneNumber = contactRequest.PhoneNumber;
                if (!string.IsNullOrEmpty(contactRequest.FirstName))
                    contact.FirstName = contactRequest.FirstName;
                if (!string.IsNullOrEmpty(contactRequest.Email))
                    contact.Email = contactRequest.Email;
                
                this.contactsRepository.UpdateContact(contact);
            }
            catch (Exception ex)
            {
                _log.Error("Error occured in EditContact method:", ex);
                ModelState.AddModelError("request", "Some error has occured.");
                return BadRequest(ModelState);
            }

            return Ok("Contact updated successfully.");
        }
    }
}
