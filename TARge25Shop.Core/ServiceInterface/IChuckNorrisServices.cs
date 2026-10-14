using System;
using System.Collections.Generic;
using System.Text;
using TARge25Shop.Core.Dto;
using System.Threading.Tasks;

namespace TARge25Shop.Core.ServiceInterface
{
    public interface IChuckNorrisServices
    {
        Task<ChuckNorrisRootDto> ChuckNorrisResultHttp();
    }
}
