
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CelaEngagement.Models.SP;
using System.Net.Http;
using System.Collections.Generic;
using Serilog;

namespace CelaEngagement.Services
{
    public class SPService:ISPService
    {
        private readonly ILogger _logger;
        public SPService(ILogger logger)
        {
            _logger = logger;
        }
        public async Task<Tuple<string, List<Datum>>> GetSPResuts(SPRequestModel spRequestModel)
        {
            var client = new HttpClient();
            var gifs = new List<Datum>();
            try
            {
                var url = SPSearch(spRequestModel);
                _logger.Information("Pulling data from " + url);
                var responseBody = await client.GetAsync(url);
                var response = JsonConvert.DeserializeObject<SPSearchResponse>(await responseBody.Content.ReadAsStringAsync());
                gifs.AddRange(response.Data);

                if(response.Pagination==null || response.Pagination.Count + response.Pagination.Offset == response.Pagination.TotalCount)
                {
                    spRequestModel.OffSet = "0";
                }
                else
                {
                    spRequestModel.OffSet = (response.Pagination.Count + response.Pagination.Offset).ToString();
                }
                _logger.Information(string.Format("Successfully retrieved all results from SP . Total: {0}", gifs.Count.ToString()));

                return Tuple.Create(spRequestModel.OffSet, gifs);
            }
            catch(Exception ex)
            {
                _logger.Error(ex, ex.Message);
                throw;
            }
        }

        private string SPSearch(SPRequestModel spRequestModel)
        {
            var returnURL = string.Format("{0}/gifs/search?api_key={1}", spRequestModel.SPUrl, spRequestModel.ApiKey);
            returnURL = string.Format("{0}&limit={1}", returnURL, "100");
            returnURL = string.Format("{0}&lang={1}", returnURL, "en");
            returnURL = string.Format("{0}&rating={1}", returnURL, Common.Common.ConvertRatingToString(spRequestModel.Rating));
            returnURL = string.Format("{0}&offset={1}", returnURL, spRequestModel.OffSet);
            returnURL = string.Format("{0}&q={1}", returnURL, spRequestModel.SearchQuery);
            return returnURL;
        }
    }

    public interface ISPService
    {
        Task<Tuple<string, List<Datum>>> GetSPResuts(SPRequestModel spRequestModel);
    }

    
}
