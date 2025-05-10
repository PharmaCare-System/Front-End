using System.Net.Http.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using PharmaCare.Models;

public class AccountController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        IConfiguration config,
        ILogger<AccountController> logger)
    {
        _config = config;
        _logger = logger;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_config["ApiSettings:BaseUrl"])
        };
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM model, string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (ModelState.IsValid)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Accounts/VLoginAsync", new
                {
                    Username = model.Email,
                    Password = model.Password
                });

                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

                    // Set auth cookie
                    Response.Cookies.Append("AuthToken", loginResponse.Token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = model.RememberMe ? loginResponse.Expiration : null
                    });

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Login failed with status: {StatusCode}, response: {Response}",
                        response.StatusCode, errorContent);
                    ModelState.AddModelError(string.Empty, "Invalid login attempt");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed");
                ModelState.AddModelError(string.Empty, "An error occurred during login");
            }
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var customerResponse = await _httpClient.PostAsJsonAsync("Customers", new
                {
                    Name = model.FullName,
                    Email = model.Email,
                    Phone = model.PhoneNumber
                });

                if (customerResponse.IsSuccessStatusCode)
                {
                    var loginResponse = await _httpClient.PostAsJsonAsync("Accounts/VLoginAsync", new
                    {
                        Username = model.Email,
                        Password = model.Password
                    });

                    if (loginResponse.IsSuccessStatusCode)
                    {
                        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

                        Response.Cookies.Append("AuthToken", loginResult.Token, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict
                        });

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Registration successful but automatic login failed");
                    }
                }
                else
                {
                    var errorContent = await customerResponse.Content.ReadAsStringAsync();
                    _logger.LogError("Registration failed with status: {StatusCode}, response: {Response}",
                        customerResponse.StatusCode, errorContent);
                    ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed");
                ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
            }
        }

        return View(model);
    }

    [HttpPost]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("AuthToken");
        return RedirectToAction("Index", "Home");
    }
}

// DTO classes
public class LoginResponse
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}