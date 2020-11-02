using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HomeAPI.Backend.Data;
using HomeAPI.Backend.Models.News;
using HomeAPI.Backend.Providers.News;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeAPI.Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NewsController : ControllerBase
	{
		private readonly IMapper mapper;

		private readonly IRssFeedProvider feedProvider;
		private readonly IAsyncRepository<NewsFeedSubscription> subscriptionRepository;

		public NewsController(IMapper mapper, IRssFeedProvider feedProvider, IAsyncRepository<NewsFeedSubscription> subscriptionRepository)
		{
			this.mapper = mapper;
			this.feedProvider = feedProvider;
			this.subscriptionRepository = subscriptionRepository;
		}

		[HttpGet("feeds")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<List<NewsFeedSubscription>>> GetNewsFeedSubscriptions()
		{
			var feeds = await subscriptionRepository.GetAllAsync();

			if (feeds == null)
			{
				return NotFound();
			}

			return Ok(feeds);
		}

		[HttpGet("feeds/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<NewsFeedSubscription>> GetNewsFeedSubscription(int id)
		{
			var feed = await subscriptionRepository.GetAsync(id);

			if (feed == null)
			{
				return NotFound();
			}

			return Ok(feed);
		}

		[HttpPost("feeds")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<List<NewsFeedSubscription>>> AddNewsFeedSubscription([FromBody] NewsFeedSubscriptionUpdateDTO feedSubscriptionDTO)
		{
			if (feedSubscriptionDTO == null)
			{
				return BadRequest();
			}

			NewsFeedSubscription feedSubscription = mapper.Map<NewsFeedSubscription>(feedSubscriptionDTO);
			await subscriptionRepository.AddAsync(feedSubscription);

			return CreatedAtAction(nameof(AddNewsFeedSubscription), new { id = feedSubscription.Id }, feedSubscription);
		}

		[HttpPut("feeds/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<NewsFeedSubscription>> UpdateNewsFeedSubscription(int id, [FromBody] NewsFeedSubscriptionUpdateDTO feedSubscriptionDTO)
		{
			if (feedSubscriptionDTO == null)
			{
				return BadRequest();
			}

			NewsFeedSubscription feedSubscription = mapper.Map<NewsFeedSubscription>(feedSubscriptionDTO);
			feedSubscription.Id = id;

			await subscriptionRepository.UpdateAsync(feedSubscription);

			return Ok(feedSubscription);
		}

		[HttpDelete("feeds/{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeleteNewsFeedSubscription(int id)
		{
			var feedSubscription = await subscriptionRepository.GetAsync(id);
			if (feedSubscription == null)
			{
				return NotFound();
			}

			await subscriptionRepository.RemoveAsync(feedSubscription);

			return NoContent();
		}
	}
}