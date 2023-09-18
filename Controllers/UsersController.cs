namespace WebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Users;
using WebApi.Services;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private IUserService _userService;
    private IMapper _mapper;

    public UsersController(
        IUserService userService,
        IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpPost("authenticate")]
    public IActionResult Authenticate(AuthenticateRequest model)
    {
        var user = _userService.Authenticate(model.Email, model.Password);

        if (user == null)
            return BadRequest(new { message = "Email or password is incorrect" });

        return Ok(user);
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
       return Ok(new { message = "Logout successful" });
    }

    [HttpGet]
    public IActionResult GetAll(string email)
    {
        var users = _userService.GetAll(email);
        return Ok(users);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var user = _userService.GetById(id);
        return Ok(user);
    }

    [HttpPost]
    public IActionResult Create(CreateRequest model)
    {
        _userService.Create(model);
        return Ok(new { message = "User created" });
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateRequest model)
    {
        _userService.Update(id, model);
        return Ok(new { message = "User updated" });
    }

    [HttpPut("{id}/changepassword")]
    public IActionResult ChangePassword(int id, ChangePasswordRequest model)
    {
        _userService.UpdatePassword(id, model);
        return Ok(new { message = "Password updated successfully" });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _userService.Delete(id);
        return Ok(new { message = "User deleted" });
    }

}