using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Application.DTOs;
using src.Application.Interfaces;
using src.Application.Mappers;

namespace src.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DealsController : Controller
    {
        private readonly ICorreiosPrecoApiService _correiosPriceService;
        private readonly IDealService _dealService;
        private readonly IBidService _bidService;
        private readonly IMessageService _messageService;
        private readonly IDeliveryService _deliveryService;

        public DealsController(ICorreiosPrecoApiService correiosPriceService, IDealService dealService, IBidService bidService,
            IMessageService messageService, IDeliveryService deliveryService)
        {
            this._correiosPriceService = correiosPriceService;
            this._dealService = dealService;
            this._bidService = bidService;
            this._messageService = messageService;
            this._deliveryService = deliveryService;
        }

        [Authorize("JwtOrGoogle")]
        [HttpGet("{dealId}")]
        public async Task<IActionResult> GetDeal([FromRoute] int dealId)
        {
            try {
                return Ok(await this._dealService.Get(dealId));

            }
            catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize("JwtOrGoogle")]
        [HttpPost()]
        public async Task<IActionResult> AddDeal([FromBody] DealDTO dealDTO)
        {
            try {
                return Ok(await this._dealService.Add(dealDTO));
            }
            catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize("JwtOrGoogle")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeal([FromRoute] int id, [FromBody] DealDTO dealDTO)
        {
            try {
                dealDTO.DealId = id;
                return Ok(await this._dealService.Update(dealDTO));
            }
            catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{dealId}/bids/{bidId}")]
        public async Task<IActionResult> GetBid([FromRoute] int dealId, [FromRoute] int bidId)
        {
            try {
                return Ok(await this._bidService.Get(dealId, bidId));

            }
            catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{dealId}/bids")]
        public async Task<IActionResult> GetBidsByDeal([FromRoute] int dealId)
        {
            try {
                return Ok(await this._bidService.GetAllByDeal(dealId));

            }
            catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize("JwtOrGoogle")]
        [HttpPost("{dealId}/bids")]
        public async Task<IActionResult> AddBid([FromRoute] int dealId, [FromBody] BidDTO bidDTO)
        {
            try {
                bidDTO.DealId = dealId;
                return Ok(await this._bidService.Add(bidDTO));
            }
            catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize("JwtOrGoogle")]
        [HttpPut("{dealId}/bids/{bidId}")]
        public async Task<IActionResult> UpdateBid([FromRoute] int dealId, [FromRoute] int bidId, [FromBody] BidDTO bidDTO)
        {
            try {
                bidDTO.BidId = bidId;
                bidDTO.DealId = dealId;
                return Ok(await this._bidService.Update(bidDTO));
            }
            catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{dealId}/messages/{messageId}")]
        public async Task<IActionResult> GetMessage([FromRoute] int dealId, [FromRoute] int messageId)
        {
            try {
                return Ok(await this._messageService.Get(dealId, messageId));

            }
            catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{dealId}/messages")]
        public async Task<IActionResult> GetMessagesByDeal([FromRoute] int dealId)
        {
            try {
                return Ok(await this._messageService.GetAllByDeal(dealId));

            }
            catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize("JwtOrGoogle")]
        [HttpPost("{dealId}/messages")]
        public async Task<IActionResult> AddMessage([FromRoute] int dealId, [FromBody] MessageDTO messageDTO)
        {
            try {
                messageDTO.DealId = dealId;
                return Ok(await this._messageService.Add(messageDTO));
            }
            catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize("JwtOrGoogle")]
        [HttpPut("{dealId}/messages/{messageId}")]
        public async Task<IActionResult> UpdateMessage([FromRoute] int dealId, [FromRoute] int messageId, [FromBody] MessageDTO messageDTO)
        {
            try {
                messageDTO.MessageId = messageId;
                messageDTO.DealId = dealId;
                return Ok(await this._messageService.Update(messageDTO));
            }
            catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{dealId}/deliveries")]
        public async Task<IActionResult> GetDeliveriesByDeal([FromRoute] int dealId)
        {
            try {
                var delivery = await this._deliveryService.Get(dealId);
                return Ok(delivery);
            }
            catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize("JwtOrGoogle")]
        [HttpPost("{dealId}/deliveries")]
        public async Task<IActionResult> AddDelivery([FromRoute] int dealId, [FromBody] int user_id)
        {
            try {
                var delivery = await this._deliveryService.Add(dealId, user_id);
                return Ok(delivery);
            }
            catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}