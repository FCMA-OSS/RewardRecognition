using FCMA.RewardRecognition.Core.Models;
using FCMA.RewardRecognition.Common.ContextProvider;
using FCMA.RewardRecognition.Infrastructure.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FCMA.RewardRecognition.Core.Entities;
using FCMA.RewardRecognition.Web.Controllers;

namespace FCMA.RewardRecognition.Web.API
{
    //[Authorize]
    public class RewardController : ApiController
    {
        private readonly IRewardRecognitionService _rewardRecognitionService;
        private readonly IRewardUserService _rewardUserService;
        private readonly IContextService _httpContextService;
        public RewardController(IRewardRecognitionService rewardRecognitionService, IRewardUserService rewardUserService, IContextService httpContextService)
        {
            _rewardRecognitionService = rewardRecognitionService;
            _rewardUserService = rewardUserService;
            _httpContextService = httpContextService;
        }
                
        [HttpGet]
        public async Task<IHttpActionResult> RewardTypes()
        {
            try
            {
                var result = _rewardRecognitionService.ListRewardTypes();

                return Ok(await result);
            }
            catch
            {

                return BadRequest("RewardTypes could not be found.");
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> RewardReasons()
        {
            try
            {
                var result = _rewardRecognitionService.ListRewardReasons();

                return Ok(await result);
            }
            catch
            {
                return BadRequest("RewardReasons could not be found.");
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> RewardStatuses()
        {
            try
            {
                var result = _rewardRecognitionService.ListRewardStatuses();

                return Ok(await result);
            }
            catch
            {
                return BadRequest("RewardStatuses could not be found.");
            }
        }
        
        [HttpGet]
        public RewardUserModel GetCurrentUser()
        {
            try
            {
                return _rewardUserService.GetCurrentUser(this.User);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public IHttpActionResult AllUsersList()
        {
            try
            {
                var result = _rewardUserService.RewardUsers;

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult GetPendingRewards(string supervisor)
        {
            try
            {
                if (ModelState.IsValid)
                { 
                var result = _rewardRecognitionService.GetPendingRewards(supervisor);

                return Ok(result);
                }
                else
                {
                    return BadRequest("invalid model state");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IHttpActionResult> InsertReward([FromBody]List<Reward> rewards)
        {
            try
            {
                var result = _rewardRecognitionService.InsertRewardAsync(rewards);

                return Ok(await result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [ValidateModel]
        public async Task<IHttpActionResult> UpdateRewardStatusAsync([FromUri]int id, [FromUri]string statusCode)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = _rewardRecognitionService.UpdateRewardStatusAsync(id, statusCode);

                return Ok(await result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetRewardById([FromUri]int id)
        {
            try
            {
                var result = _rewardRecognitionService.GetRewardAsync(id);

                return Ok(await result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetApprovedRewardById([FromUri]int id)
        {
            try
            {
                var result = _rewardRecognitionService.GetApprovedRewardAsync(id);

                return Ok(await result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult GetMyRewards()
        {
            try
            {
                string username = _httpContextService.GetUserName().Split('\\')[1];
                var result = _rewardRecognitionService.GetMyRewards(username);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult getRecentRewards()
        {
            try
            {
                var result = _rewardRecognitionService.getRecentRewards();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult GetMyTeamRewards()
        {
            try
            {
                string username = _httpContextService.GetUserName().Split('\\')[1];
                var result = _rewardRecognitionService.GetMyTeamRewards(username);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
    }
}