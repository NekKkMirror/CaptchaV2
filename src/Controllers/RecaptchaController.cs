using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/v1/recaptcha")]
[ApiController]
public class RecaptchaController : ControllerBase
{
  private readonly RecaptchaService _recaptchaService;

  public RecaptchaController(RecaptchaService recaptchaService)
  {
    _recaptchaService = recaptchaService;
  }

  [HttpPost]
  [Route("verify-recaptcha")]
  public async Task<IActionResult> VerifyRecaptcha([FromForm] string recaptchaResponse)
  {
    bool isHuman = await _recaptchaService.VerifyRecaptchaAsync(recaptchaResponse);
    if (isHuman)
    {
      return Ok("Verification successful.");
    }
    return BadRequest("Verification failed.");
  }
}
