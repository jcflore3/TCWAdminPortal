using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using TCWAdminPortalWeb.Models;
using TCWAdminPortalWeb.Repository;
using TCWAdminPortalWeb.ViewModels;

namespace TCWAdminPortalWeb.Controllers.Api
{
    [EnableCors(origins: "http://localhost:8080,http://texascrosswayrealty.com", headers: "*", methods: "*")]
    public class ContactInfoController : ApiController
    {
        private TCWAdminRepository<ContactInfo> _repository;

        public ContactInfoController()
        {
            _repository = new TCWAdminRepository<ContactInfo>();
        }

        /// <summary>
        /// REST Get the current contact info
        /// </summary>
        /// <returns>The current contact info record in the table</returns>
        [HttpGet]
        public ContactInfoViewModel Get()
        {
            var result = AutoMapperConfig.TCWMapper.Map<ContactInfoViewModel>(_repository.GetFirst());
            return result;
        }
    }
}
