using System;
using System.Collections.Generic;
using System.Text;
using TARge25Shop.Core.Dto;

namespace TARge25Shop.Core.ServiceInterface
{
    public interface IDrinkServices
    {
        Task<DrinkDto> DrinkResponseDto(DrinkDto dto);
    }
}
