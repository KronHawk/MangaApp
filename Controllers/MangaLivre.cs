using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace MangaLivreWrapper.Controllers
{
    public class MangaLivre : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public MangaLivre(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("{Url}")]
        public async Task<IActionResult> GetHtml([FromQuery] string Url)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var html = await httpClient.GetStringAsync(Url);

                return Ok(html);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro na busca de Html: {ex.Message}");
            }
        }

        [HttpGet("TryGetPage")]
        public async Task<IActionResult> GetPages([FromQuery] string Url)
        {
            try
            {
                ChromeOptions options = new ChromeOptions();
                options.AddArgument("--headless");

                IWebDriver driver = new ChromeDriver(options);
                driver.Navigate().GoToUrl(Url);
                IWebElement botao = driver.FindElement(By.ClassName("page-next"));

                botao.Click();
                await Task.Delay(TimeSpan.FromSeconds(2));
                string uphtml = driver.PageSource;
                driver.Quit();

                return Ok(uphtml);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }
    }
}
