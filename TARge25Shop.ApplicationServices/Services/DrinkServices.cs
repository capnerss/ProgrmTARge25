using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;

namespace TARge25Shop.ApplicationServices.Services
{
    public class DrinkServices : IDrinkServices
    {
        public async Task<DrinkDto> DrinkResponseDto(DrinkDto dto)
        {
            string urlDrink = $"https://www.thecocktaildb.com/api/json/v1/1/search.php?s={dto.StrDrink}";

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(urlDrink);
                DrinkResponseDto DrinkDto = new JavaScriptSerializer()
                    .Deserialize<DrinkResponseDto>(json);

                dto.IdDrink = DrinkDto.Drinks[0].IdDrink;
                dto.StrDrink = DrinkDto.Drinks[0].StrDrink;
                dto.StrDrinkAlternate = DrinkDto.Drinks[0].StrDrinkAlternate;
                dto.StrTags = DrinkDto.Drinks[0].StrTags;
                dto.StrVideo = DrinkDto.Drinks[0].StrVideo;
                dto.StrCategory = DrinkDto.Drinks[0].StrCategory;
                dto.StrIBA = DrinkDto.Drinks[0].StrIBA;
                dto.StrAlcoholic = DrinkDto.Drinks[0].StrAlcoholic;
                dto.StrGlass = DrinkDto.Drinks[0].StrGlass;
                dto.StrInstructions = DrinkDto.Drinks[0].StrInstructions;
                dto.StrInstructionsES = DrinkDto.Drinks[0].StrInstructionsES;
                dto.StrInstructionsDE = DrinkDto.Drinks[0].StrInstructionsDE;
                dto.StrInstructionsFR = DrinkDto.Drinks[0].StrInstructionsFR;
                dto.StrInstructionsIT = DrinkDto.Drinks[0].StrInstructionsIT;
                dto.StrInstructionsZhHans = DrinkDto.Drinks[0].StrInstructionsZhHans;
                dto.StrInstructionsZhHant = DrinkDto.Drinks[0].StrInstructionsZhHant;
                dto.StrDrinkThumb = DrinkDto.Drinks[0].StrDrinkThumb;

                dto.StrIngredient1 = DrinkDto.Drinks[0].StrIngredient1;
                dto.StrIngredient2 = DrinkDto.Drinks[0].StrIngredient2;
                dto.StrIngredient3 = DrinkDto.Drinks[0].StrIngredient3;
                dto.StrIngredient4 = DrinkDto.Drinks[0].StrIngredient4;
                dto.StrIngredient5 = DrinkDto.Drinks[0].StrIngredient5;
                dto.StrIngredient6 = DrinkDto.Drinks[0].StrIngredient6;
                dto.StrIngredient7 = DrinkDto.Drinks[0].StrIngredient7;
                dto.StrIngredient8 = DrinkDto.Drinks[0].StrIngredient8;
                dto.StrIngredient9 = DrinkDto.Drinks[0].StrIngredient9;
                dto.StrIngredient10 = DrinkDto.Drinks[0].StrIngredient10;
                dto.StrIngredient11 = DrinkDto.Drinks[0].StrIngredient11;
                dto.StrIngredient12 = DrinkDto.Drinks[0].StrIngredient12;
                dto.StrIngredient13 = DrinkDto.Drinks[0].StrIngredient13;
                dto.StrIngredient14 = DrinkDto.Drinks[0].StrIngredient14;
                dto.StrIngredient15 = DrinkDto.Drinks[0].StrIngredient15;

                dto.StrMeasure1 = DrinkDto.Drinks[0].StrMeasure1;
                dto.StrMeasure2 = DrinkDto.Drinks[0].StrMeasure2;
                dto.StrMeasure3 = DrinkDto.Drinks[0].StrMeasure3;
                dto.StrMeasure4 = DrinkDto.Drinks[0].StrMeasure4;
                dto.StrMeasure5 = DrinkDto.Drinks[0].StrMeasure5;
                dto.StrMeasure6 = DrinkDto.Drinks[0].StrMeasure6;
                dto.StrMeasure7 = DrinkDto.Drinks[0].StrMeasure7;
                dto.StrMeasure8 = DrinkDto.Drinks[0].StrMeasure8;
                dto.StrMeasure9 = DrinkDto.Drinks[0].StrMeasure9;
                dto.StrMeasure10 = DrinkDto.Drinks[0].StrMeasure10;
                dto.StrMeasure11 = DrinkDto.Drinks[0].StrMeasure11;
                dto.StrMeasure12 = DrinkDto.Drinks[0].StrMeasure12;
                dto.StrMeasure13 = DrinkDto.Drinks[0].StrMeasure13;
                dto.StrMeasure14 = DrinkDto.Drinks[0].StrMeasure14;
                dto.StrMeasure15 = DrinkDto.Drinks[0].StrMeasure15;

                dto.StrImageSource = DrinkDto.Drinks[0].StrImageSource;
                dto.StrImageAttribution = DrinkDto.Drinks[0].StrImageAttribution;
                dto.StrCreativeCommonsConfirmed = DrinkDto.Drinks[0].StrCreativeCommonsConfirmed;
                dto.DateModified = DrinkDto.Drinks[0].DateModified;
            }
            return dto;
        }
    }
}