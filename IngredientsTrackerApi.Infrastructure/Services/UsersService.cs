﻿using AutoMapper;
using MongoDB.Bson;
using IngredientsTrackerApi.Application.Exceptions;
using IngredientsTrackerApi.Application.IRepositories;
using IngredientsTrackerApi.Application.IServices;
using IngredientsTrackerApi.Application.Models.Dto;
using IngredientsTrackerApi.Application.Pagination;

namespace IngredientsTrackerApi.Infrastructure.Services.Identity;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _repository;

    private readonly IMapper _mapper;

    public UsersService(IUsersRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedList<UserDto>> GetUsersPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetPageAsync(pageNumber, pageSize, cancellationToken);
        var dtos = _mapper.Map<List<UserDto>>(entities);
        var count = await _repository.GetTotalCountAsync(cancellationToken);
        return new PagedList<UserDto>(dtos, pageNumber, pageSize, count);
    }

    public async Task<UserDto> GetUserAsync(string id, CancellationToken cancellationToken)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            throw new InvalidDataException("Provided id is invalid.");
        }

        var entity = await _repository.GetOneAsync(objectId, cancellationToken);
        if (entity == null)
        {
            throw new EntityNotFoundException("User");
        }

        return _mapper.Map<UserDto>(entity);
    }

    public async Task<UserDto> GetUserByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetOneAsync(
            u => u.Email == username || u.Phone == username, cancellationToken);
        if (entity == null)
        {
            throw new EntityNotFoundException("User");
        }

        return _mapper.Map<UserDto>(entity);
    }
}
