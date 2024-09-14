using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Models.Dogma;

namespace ESI.NET.Interfaces.Logic
{
    public interface IDogmaLogic
    {
        /// <summary>
        /// /dogma/attributes/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<int[]>> Attributes(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /dogma/attributes/{attribute_id}/
        /// </summary>
        /// <param name="attribute_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Attribute>> Attribute(int attribute_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /dogma/effects/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<int[]>> Effects(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /dogma/effects/{effect_id}/
        /// </summary>
        /// <param name="effect_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Effect>> Effect(int effect_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /dogma/dynamic/items/{type_id}/{item_id}/
        /// </summary>
        /// <param name="type_id"></param>
        /// <param name="item_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Effect>> DynamicItem(int type_id, long item_id, string eTag = null,
            CancellationToken cancellationToken = default);
    }
}