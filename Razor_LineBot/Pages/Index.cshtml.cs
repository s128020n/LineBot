using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace Razor_LineBot.Pages
{
    public class IndexModel : PageModel
    {
        public string sNote = "";
        private readonly IConfiguration _config;
        public IndexModel(IConfiguration config)
        {
            _config = config;
        }


        [BindProperty]
        public string txbToken { get; set; }
        [BindProperty]
        public string txbUserId { get; set; }
        [BindProperty]
        public string txbMessage { get; set; }
        [BindProperty]
        public int txbStickerPkgId { get; set; }
        [BindProperty]
        public int txbStickerStkId { get; set; }
        public void OnGet()
        {
            //get configuration from appsettings.json
            var token = _config.GetSection("LINE-Bot-Setting:channelAccessToken");
            var AdminUserId = _config.GetSection("LINE-Bot-Setting:adminUserID");
            txbToken = token.Value;
            txbUserId = AdminUserId.Value;
            txbMessage = "";
            return;
        }

        public void OnPostSendMessage()
        {
            if (string.IsNullOrEmpty(txbToken) || string.IsNullOrEmpty(txbUserId) || string.IsNullOrEmpty(txbMessage))
            {
                sNote = "Cannot sent the message due to missing data.";
            }
            else
            {
                var bot = new isRock.LineBot.Bot(txbToken);

                try
                {
                    var ret = bot.PushMessage(txbUserId, txbMessage);
                    sNote = "Success !";
                }
                catch (Exception ex)
                {
                    this.sNote = " The token / The user ID was not  found.";
                }
            }
        }
        public void OnPostSendSticker()
        {
            if (string.IsNullOrEmpty(txbToken) || string.IsNullOrEmpty(txbUserId))
            {
                sNote = "Cannot sent the message due to missing data.";
            }
            else
            {
                var bot = new isRock.LineBot.Bot(txbToken);

                try
                {
                    var ret = bot.PushMessage(txbUserId, new isRock.LineBot.StickerMessage(txbStickerPkgId, txbStickerStkId));
                    sNote = "Success !";
                }
                catch (Exception ex)
                {
                    this.sNote = " The token / The user ID or The sticker ID was not  found .";
                }
            }
        }
    }
}
