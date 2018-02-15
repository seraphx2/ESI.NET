using System.Collections.Generic;
using System.Threading.Tasks;
using ESI.NET.Models.Dogma;

namespace ESI.NET.Logic.Interfaces
{
    public interface IDogmaLogic
    {
        Task<ApiResponse<Attribute>> Attribute(int attribute_id);
        Task<ApiResponse<List<int>>> Attributes();
        Task<ApiResponse<Effect>> Effect(int effect_id);
        Task<ApiResponse<List<int>>> Effects();
    }
}