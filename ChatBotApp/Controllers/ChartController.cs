using Microsoft.AspNetCore.Mvc;
using ChatBotApp.Services;
using ChatBotApp.Data;
using ChatBotApp.Models;

namespace ChatBotApp.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly ChatService _chatService;
        private readonly AppDbContext _context;

        public ChatController(ChatService chatService, AppDbContext context)
        {
            _chatService = chatService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Chat([FromBody] string message)
        {
            var botResponse = await _chatService.GetBotResponse(message);

            var chat = new ChatMessage
            {
                UserMessage = message,
                BotResponse = botResponse,
                CreatedDate = DateTime.Now
            };

            _context.ChatMessages.Add(chat);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                user = message,
                bot = botResponse
            });
        }
    }
}
