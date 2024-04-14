using AutoMapper;
using IngredientsTrackerApi.Application.IRepositories;
using IngredientsTrackerApi.Application.IServices;
using IngredientsTrackerApi.Application.Models.Dto;
using IngredientsTrackerApi.Application.Pagination;

namespace IngredientsTrackerApi.Infrastructure.Services.Identity;

public class RolesService : IRolesService
{
    private readonly IRolesRepository _repository;

    private readonly IMapper _mapper;

    public RolesService(IRolesRepository repository, IMapper mapper)
    {
        this._repository = repository;
        this._mapper = mapper;
    }

    public async Task<PagedList<RoleDto>> GetRolesPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var entities = await this._repository.GetPageAsync(pageNumber, pageSize, cancellationToken);
        var count = await this._repository.GetTotalCountAsync(cancellationToken);
        var dtos = this._mapper.Map<List<RoleDto>>(entities);

        return new PagedList<RoleDto>(dtos, pageNumber, pageSize, count);
    }
}
