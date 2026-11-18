using DalilAcademy.Core.DTOs;
using static DalilAcademy.Core.DTOs.InstructorDto;

namespace DalilAcademy.Core.Service
{
    public interface IInstructorService
    {
        Task<PagedResult<InstructorResponseDto>> GetPagedResult(int page, int pageSize);
        Task<InstructorResponseDto> GetByIdAsync(int id);
        Task UpdateAsync(int id, UpdateInstructorDto dto);
        Task DeleteAsync(int id);
    }
}
