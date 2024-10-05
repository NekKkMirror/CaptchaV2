using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DotNetEnv;

public class RecaptchaService
{
  private readonly HttpClient _httpClient;
  private readonly string? _recaptchaSecretKey;
  private readonly string? RecaptchaVerifyUrl;

  public RecaptchaService(HttpClient httpClient)
  {
    _httpClient = httpClient;
    DotNetEnv.Env.Load();
    _recaptchaSecretKey = Environment.GetEnvironmentVariable("RECAPTCHA_SECRET_KEY");
    RecaptchaVerifyUrl = Environment.GetEnvironmentVariable("RECAPTCHA_VERIFY_URL");

    if (string.IsNullOrEmpty(_recaptchaSecretKey) || string.IsNullOrEmpty(RecaptchaVerifyUrl))
    {
      throw new InvalidOperationException("Environment variables for reCAPTCHA are not set.");
    }
  }

  public async Task<bool> VerifyRecaptchaAsync(string recaptchaResponse)
  {
    if (string.IsNullOrEmpty(recaptchaResponse))
    {
      throw new ArgumentException("Recaptcha response cannot be null or empty.", nameof(recaptchaResponse));
    }

    HttpResponseMessage response = await _httpClient.PostAsync($"{RecaptchaVerifyUrl}?secret={_recaptchaSecretKey}&response={recaptchaResponse}", null);
    string jsonResponse = await response.Content.ReadAsStringAsync();
    RecaptchaResponse? recaptchaResult = JsonSerializer.Deserialize<RecaptchaResponse>(jsonResponse);

    if (recaptchaResult == null)
    {
      throw new InvalidOperationException("Failed to deserialize reCAPTCHA response.");
    }

    return recaptchaResult.success;
  }
}

public class RecaptchaResponse
{
  [JsonPropertyName("success")]
  public bool success { get; set; }

  [JsonPropertyName("challenge_ts")]
  public string? challengeTimestamp { get; set; }

  [JsonPropertyName("hostname")]
  public string? hostname { get; set; }

  [JsonPropertyName("error-codes")]
  public string[]? errorCodes { get; set; }
}
