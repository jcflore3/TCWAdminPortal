using System.Collections.Generic;
using System.Web.Http;
using TCWAdminPortalWeb.Models;
using TCWAdminPortalWeb.Repository;
using TCWAdminPortalWeb.ViewModels;

namespace TCWAdminPortalWeb.Controllers.Api
{
    public class AgentController : ApiController
    {
        private TCWAdminRepository<Agent> _repository;

        public AgentController()
        {
            _repository = new TCWAdminRepository<Agent>();
        }

        /// <summary>
        /// REST Get of all the Agents
        /// </summary>
        /// <returns>An IEnumerable of all the agents</returns>
        [HttpGet]
        public IEnumerable<AgentViewModel> Get()
        {
            var result = AutoMapperConfig.TCWMapper.Map<IEnumerable<AgentViewModel>>(_repository.GetAll());
            return result;
        }
    }
}
