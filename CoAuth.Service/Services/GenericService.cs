using System.Linq.Expressions;
using CoAuth.Core.Entities;
using CoAuth.Core.Repositories;
using CoAuth.Core.Services;
using CoAuth.Core.UnifOfWork;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;

namespace CoAuth.Service.Services;

public class GenericService<TEntity,TDto>:IGenericService<TEntity,TDto> where TEntity:class where TDto:class
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<TEntity> _repository;

    public GenericService(IUnitOfWork unitOfWork, IRepository<TEntity> repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }
    
    
    public async Task<Response<TDto>> GetByIdAsync(int id)
    {
        var item = await _repository.GetByIdAsync(id);

        if (item is null)
        {
            return Response<TDto>.Fail("Id not found", 404, true);
        }

        return Response<TDto>.Success(ObjectMapper.Mapper.Map<TDto>(item), 200);
    }

    public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
    {
        var list = ObjectMapper.Mapper.Map<List<TDto>>(await _repository.GetAll().ToListAsync());
        return Response<IEnumerable<TDto>>.Success(list, 200);
    }

    public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
    {
        var list = _repository.Where(predicate);
        
        //Business kuralları gerekirse buraya gelecek..

        return Response<IEnumerable<TDto>>.Success(
            ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await list.ToListAsync()),200);
    }

    public async Task<Response<TDto>> AddAsync(TDto entity)
    {
        var newEntity = ObjectMapper.Mapper.Map<TEntity>(entity);
        await _repository.AddAsync(newEntity);

        await _unitOfWork.CommitAsync();

        var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);
        
        return Response<TDto>.Success(newDto,200);
    }

    public async Task<Response<NoDataDto>> Remove(int id)
    {
        var isExistEntity = await _repository.GetByIdAsync(id);

        if (isExistEntity is null)
        {
            return Response<NoDataDto>.Fail("Id not found",404,true);
        }
        
        _repository.Remove(isExistEntity);
        await _unitOfWork.CommitAsync();

        return Response<NoDataDto>.Success(204);
    }

    public async Task<Response<NoDataDto>> Update(TDto entity,int id)
    {
        var isExistEntity = await _repository.GetByIdAsync(id);
        
        if (isExistEntity is null)
        {
            return Response<NoDataDto>.Fail("Id not found",404,true);
        }

        var updateEntity = ObjectMapper.Mapper.Map<TEntity>(entity);

        _repository.Update(updateEntity);

        await _unitOfWork.CommitAsync();

        return Response<NoDataDto>.Success(204);




    }
}