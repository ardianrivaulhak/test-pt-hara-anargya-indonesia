namespace WebApi.Services;

using AutoMapper;
using BCrypt.Net;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Users;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


public interface IUserService
{
    IEnumerable<User> GetAll(string email);
    User GetById(int id);
    void Create(CreateRequest model);
    void Update(int id, UpdateRequest model);
    void UpdatePassword(int id, ChangePasswordRequest model);
    void Delete(int id);
    User Authenticate(string email, string password);
}

public class UserService : IUserService
{
    private DataContext _context;
    private readonly IMapper _mapper;
    
    public UserService(
        DataContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    

    public User Authenticate(string email, string password)
    {
        var user = _context.Users.SingleOrDefault(x => x.Email == email);

        if (user == null || !BCrypt.Verify(password, user.PasswordHash))
            return null;

        return user;
    }

    public IEnumerable<User> GetAll(string email)
    {
      if (!string.IsNullOrWhiteSpace(email))
       {
        return _context.Users.Where(u => u.Email == email);
       }

        return _context.Users;
    }

    public User GetById(int id)
    {
        return getUser(id);
    }


    public void Create(CreateRequest model)
    {
        if (_context.Users.Any(x => x.Email == model.Email))
            throw new AppException("User with the email '" + model.Email + "' already exists");

        var user = _mapper.Map<User>(model);

        user.PasswordHash = BCrypt.HashPassword(model.Password);

        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void Update(int id, UpdateRequest model)
    {
        var user = getUser(id);

        if (model.Email != user.Email && _context.Users.Any(x => x.Email == model.Email))
            throw new AppException("User with the email '" + model.Email + "' already exists");

        if (!string.IsNullOrEmpty(model.Password))
            user.PasswordHash = BCrypt.HashPassword(model.Password);

        _mapper.Map(model, user);
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void UpdatePassword(int id, ChangePasswordRequest model)
    {

    var user = getUser(id);

    if (!BCrypt.Verify(model.OldPassword, user.PasswordHash))
        throw new AppException("Old password is incorrect");

    if (model.NewPassword != model.ConfirmPassword)
        throw new AppException("New password and confirm password do not match");

    user.PasswordHash = BCrypt.HashPassword(model.NewPassword);

    _context.Users.Update(user);
    _context.SaveChanges();
   } 

    public void Delete(int id)
    {
        var user = getUser(id);
        _context.Users.Remove(user);
        _context.SaveChanges();
    }


    private User getUser(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }
}