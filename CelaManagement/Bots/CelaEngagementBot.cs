using CelaEngagement.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace CelaEngagement.Bots
{
    public class CelaEngagementBot: TeamsActivityHandler
    {
        private readonly ISPService _spService;
        private readonly ILogger _logger;
        private readonly string _giphyKey;
        private readonly string _giphyUrl;
        public CelaEngagementBot(ISPService spService, ILogger logger, IConfiguration configuration)
        {
            _spService = spService ?? throw new ArgumentNullException(nameof(spService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _giphyKey = configuration["GiphyKey"] ?? throw new ArgumentNullException(nameof(configuration));
            _giphyUrl = configuration["GiphyUrl"] ?? throw new ArgumentNullException(nameof(configuration));

        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            turnContext.Activity.RemoveRecipientMention();
            var text = turnContext.Activity.Text.ToString().ToLower();

            var mention = new Mention
            {
                Mentioned = turnContext.Activity.From,
                Text = $"<at>{XmlConvert.EncodeName(turnContext.Activity.From.Name)}</at>"
            };
            var response = MessageFactory.Text($"Hello {mention.Text}. Here are your top 3 matching results");
            response.Entities = new List<Entity> { mention };
            var spResults = await _spService.GetSPResuts(new Models.SP.SPRequestModel() { 
                SearchQuery = text,
                ApiKey = _giphyKey,
                Rating = Common.Rating.G,
                OffSet ="0",
                SPUrl= _giphyUrl
            });

            foreach(var spResult in spResults.Item2.Take(3))
            {
                var heroCard = CardFactory.CreateHeroCard("", "", "", new string[] { spResult.Images.DownsizedLarge.Url.AbsoluteUri }, new string[] { spResult.Url.AbsoluteUri });
                response.Attachments.Add(heroCard);
            }
            await turnContext.SendActivityAsync(response, cancellationToken);
            await base.OnMessageActivityAsync(turnContext, cancellationToken);
        }
    }
}
