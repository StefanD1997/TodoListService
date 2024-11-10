using Microsoft.EntityFrameworkCore;
using TodoList.BusinessLogic.Contracts;
using TodoList.Core.Common.Contracts;
using TodoList.Core.Helpers;
using TodoList.Core.Models;
using TodoList.DataAccess.Repositories.Contracts;
using TodoList.DTOs.User;
using TodoList.Migrations;

namespace TodoList.BusinessLogic.Services;

//TODO: Tightly coupled to TokenService
public class UserService(
    IUserRepository userRepository,
    TokenService tokenService,
    ICurrentUser currentUser) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<string?> Login(LoginDTO input)
    {
        var user = await _userRepository.GetFirstOrDefault(x =>
                x.Username == input.Username &&
                x.HashedPassword == MethodHelper.ComputeSHA512Hash(input.Password)
            );

        if (user is null)
        {
            return null;
        }

        var token = tokenService.Generate(user);
        return token;
    }

    public async Task<string> Refresh()
    {
        var user = await _userRepository.GetById(int.Parse(_currentUser.Id));

        var token = tokenService.Generate(user!);
        return token;
    }

    public async Task<bool> ChangePassword(ChangePasswordDTO input)
    {
        var user = await _userRepository.GetFirstOrDefault(x =>
                x.Id == int.Parse(_currentUser.Id) &&
                x.HashedPassword == MethodHelper.ComputeSHA512Hash(input.CurrentPassword)
            );

        if (user is null)
        {
            return false;
        }

        user.HashedPassword = MethodHelper.ComputeSHA512Hash(input.NewPassword);

        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();

        return true;
    }

    public async Task<User?> Create(CreateUserDTO input)
    {
        User? userExists = await _userRepository.GetFirstOrDefault(x =>
                x.Username == input.Username
            );

        if (userExists is not null)
        {
            return null;
        }

        var user = User.CreateCustomer(input.Username);

        await _userRepository.Create(user);
        await _userRepository.SaveChangesAsync();

        return user;
    }

    public async Task<User?> Update(UpdateUserDTO input)
    {
        User? user = await _userRepository.GetById(input.Id);

        if (user is null)
        {
            return null;
        }

        User? userExists = await _userRepository.GetFirstOrDefault(x =>
                x.Username == input.Username
            );

        if (userExists is not null)
        {
            return null;
        }

        user.Username = input.Username;
        user.Role = input.Role;

        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();

        return user;
    }

    public async Task<bool> Delete(int id)
    {
        User? user = await _userRepository.GetById(id);

        if (user is null)
        {
            return false;
        }

        _userRepository.Delete(user);
        await _userRepository.SaveChangesAsync();
        return true;
    }

    public async Task<List<UserDTO>> GetAll()
    {
        List<User> users = await _userRepository.GetAll();

        return users.Select(x => new UserDTO()
        {
            Username = x.Username,
            Id = x.Id,
            Role = x.Role
        }).ToList();

    }
}
