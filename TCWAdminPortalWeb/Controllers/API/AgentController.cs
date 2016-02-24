using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using TCWAdminPortalWeb.Models;
using TCWAdminPortalWeb.Repository;
using TCWAdminPortalWeb.ViewModels;
using System.Linq;

namespace TCWAdminPortalWeb.Controllers.Api
{
    [EnableCors(origins: "http://localhost:8080,http://texascrosswayrealty.com", headers: "*", methods: "*")]
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
            var results = AutoMapperConfig.TCWMapper.Map<IEnumerable<AgentViewModel>>(_repository.GetAll());
            // return only the Agents that are flagged as enabled
            return results.Where(x => x.Enabled);
        }
    }
}
