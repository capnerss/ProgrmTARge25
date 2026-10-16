using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;
using Xunit;

namespace TARge25Shop.SpaceshipTest
{
    public class KindergartenTest : TestBase
    {

        [Fact] 
     
        public async Task ShouldNot_AddEmptyKindergarten_WhenResultIsReturned()
        {
            // Ülesseade
            KindergartenDto dto = new KindergartenDto() 
            { 
                GroupName = "X AE a L 12 menuornvöerv",
                ChildrenCount = 67,
                KindergartenName = "lendav taldrik",
                TeacherName = "Juri",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            // tegutsemine
            var result = await Svc<IKindergartenServices>().Create(dto);

            // kontroll
            Assert.NotNull(result);
        }
   
        [Fact]
        public async Task ShouldNot_GetKindergartenByID_WhenIDNotEqual()
        {
            // ülesseade
            Guid wrongGuid = Guid.NewGuid();
            Guid goodGuid = Guid.Parse("ecbc059a-0bca-4df2-aae9-a3211e69185a");

            // tegevus
            await Svc<IKindergartenServices>().DetailAsync(goodGuid);

            // kontroll
            Assert.NotEqual(wrongGuid, goodGuid);
        }
        
        [Fact]
        public async Task Should_GetKindergartenByID_WhenGuidIsEqual()
        {
            // ülessezade
            Guid databaseGuid = Guid.Parse("ecbc059a-0bca-4df2-aae9-a3211e69185a");
            Guid seekGuid = Guid.Parse("ecbc059a-0bca-4df2-aae9-a3211e69185a");

            // tegevus
            await Svc<IKindergartenServices>().DetailAsync(seekGuid);

            // kontroll
            Assert.Equal(databaseGuid, seekGuid);
        }

        [Fact]
        public async Task Should_KindergartenDeletedByID_WhenReturnedResultIsEqual()
        {
            // ülesseade
            KindergartenDto dto = MockKindergartenData();

            // tegevus
            var addkinderGarten = await Svc<IKindergartenServices>().Create(dto);
            var deletekinderGarten = await Svc<IKindergartenServices>().Delete((Guid)addkinderGarten.Id);

            // kontroll
            Assert.Equal(addkinderGarten.Id, deletekinderGarten.Id);
        }

        [Fact]
        public async Task ShouldNot_DeleteKindergartenByID_WhenDidNotDeleteKindergarten()
        {
            //ülesseade
            var dto = MockKindergartenData();

            //tegevus
            var kinderGarten1 = await Svc<IKindergartenServices>().Create(dto);
            var kinderGarten2 = await Svc<IKindergartenServices>().Create(dto);

            var result = await Svc<IKindergartenServices>().Delete((Guid)kinderGarten2.Id);

            //kontroll
            Assert.NotEqual(kinderGarten1.Id, result.Id);
        }
        // test mis kontrollib, et spaceshipi uuendatakse, uute andmete korral
        [Fact]
        public async Task Should_UpdateKindergartenByID_WhenUpdatingData()
        {
            KindergartenDto dto = MockKindergartenData();
            var result = await Svc<IKindergartenServices>().Create(dto);
            Guid guid = (Guid)result.Id;

            Svc<TARge25ShopContext>().ChangeTracker.Clear();

            KindergartenDto domain = new();

            domain.Id = guid;
            domain.ChildrenCount = 10000000;
            domain.TeacherName = "Igor Mang 2";
            domain.KindergartenName = "püramiid";
            domain.GroupName = "420";
            domain.CreatedAt = dto.CreatedAt;//  <-- ei tohi muutuda Update korral, tuleb võtta olemasolevast objektist.
            domain.UpdatedAt = DateTime.UtcNow;//  <-- PEAB muutuma Update korral


            //tegevus
            await Svc<IKindergartenServices>().Update(domain);
            Svc<TARge25ShopContext>().ChangeTracker.Clear();
            var result2 = await Svc<IKindergartenServices>().DetailAsync(guid);

            //kontroll
            Assert.Equal(result.Id, result2.Id);
            Assert.NotEqual(dto.ChildrenCount, result2.ChildrenCount);
            Assert.NotEqual(dto.TeacherName, result2.TeacherName);
            Assert.DoesNotMatch(dto.GroupName, result2.GroupName);
            Assert.DoesNotMatch(dto.KindergartenName, result2.KindergartenName);
            Assert.Equal(dto.CreatedAt,result2.CreatedAt);
            Assert.NotEqual(dto.UpdatedAt, result2.CreatedAt);
        }
        
        [Fact]
        public async Task ShouldNot_UpdateKindergartenByID_WhenNoDataIsUpdated()
        {
            //ülesseade
            KindergartenDto dto = MockKindergartenData();
            var createdSpaceship = await Svc<IKindergartenServices>().Create(dto);

            //tegevus
            KindergartenDto nullDto = MockKindergartenNullData();
            var result = await Svc<IKindergartenServices>().Update(nullDto);

            //kontroll
            Assert.NotEqual(createdSpaceship.Id, result.Id);
        }
        
        [Fact]
        public async Task ShouldNot_CreateKindergartenWithNegativeChildrenCount_WhenChildrenCountNegative()
        {
            //ülesseade
            KindergartenDto dto = MockKindergartenData(true);
            dto.ChildrenCount -= (dto.ChildrenCount * 2);

            //tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            //kontroll
            Assert.True(result.ChildrenCount > 0);
        }

        [Fact]
        public async Task SShouldNot_CreateKindergarten_WhenChildrenCountIsThreeOrLess()
        {
            //ülesseade
            KindergartenDto dto = MockKindergartenData();
            dto.ChildrenCount = 0;
            //tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);
            //kontroll
            Assert.True(result.ChildrenCount > 3);
        }

        [Fact]
        public async Task Should_RemoveKindergartenFromDatabase_WhenKindergartenIsDeleted()
        {
            //ülesseade
            KindergartenDto dto = MockKindergartenData();

            //tegevus
            var createdKindergarten = await Svc<IKindergartenServices>().Create(dto);
            var deletedKindergarten = await Svc<IKindergartenServices>().Delete((Guid)createdKindergarten.Id);
            var result = await Svc<IKindergartenServices>().DetailAsync((Guid)createdKindergarten.Id);

            //kontrollimine
            Assert.Equal(createdKindergarten.Id, deletedKindergarten.Id);
            Assert.Null(result);
        }

        [Fact]
        public async Task ShouldNot_RemoveKindergartenFromDatabase_WhenKindergartenIdIsDifferent()
        {
            var dto = MockKindergartenData();
            var createdKindergarten1 = await Svc<IKindergartenServices>().Create(dto);
            var createdKindergarten2 = await Svc<IKindergartenServices>().Create(dto);
            var deleteResult = await Svc<IKindergartenServices>().Delete((Guid)createdKindergarten2.Id);
            var spaceshipindb = await Svc<IKindergartenServices>().DetailAsync((Guid)createdKindergarten1.Id);

            //kontroll
            Assert.NotNull(spaceshipindb);
            Assert.NotEqual(deleteResult.Id, createdKindergarten1.Id);
            Assert.Equal(createdKindergarten1, spaceshipindb);
        }
		// 1. Kontrollib, et olematu ID-ga lasteaia pärimisel andmebaasist tagastatakse null väärtus.
		[Fact]
		public async Task Should_ReturnNull_WhenKindergartenNotFoundById()
		{
			// ülesseade
			Guid nonExistentGuid = Guid.NewGuid();

			// tegevus
			var result = await Svc<IKindergartenServices>().DetailAsync(nonExistentGuid);

			// kontroll
			Assert.Null(result);
		}

		// 2. Kontrollib, et olematu ID-ga lasteaia kustutamisel ei leita kirjet ning tagastatakse null.
		[Fact]
		public async Task ShouldNot_DeleteKindergarten_WhenIdDoesNotExist()
		{
			// ülesseade
			Guid fakeGuid = Guid.NewGuid();

			// tegevus
			var result = await Svc<IKindergartenServices>().Delete(fakeGuid);

			// kontroll
			Assert.Null(result);
		}

		// 3. Kontrollib, et olematu ID-ga lasteaia uuendamisel viskab EF Core vea DbUpdateConcurrencyException.
		[Fact]
		public async Task ShouldNot_UpdateKindergarten_WhenIdDoesNotExist()
		{
			// ülesseade
			KindergartenDto dto = MockKindergartenData();
			dto.Id = Guid.NewGuid();

			// tegevus ja kontroll
			// EF Core viskab DbUpdateConcurrencyException erindi, kui objekti andmebaasist ei leita
			await Assert.ThrowsAsync<Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException>(async () =>
			{
				await Svc<IKindergartenServices>().Update(dto);
			});
		}

		// 4. Kontrollib, et lasteaia loomisel tühja rühma nimega salvestatakse kirje edukalt tühja sõnega.
		[Fact]
		public async Task Should_CreateKindergarten_WhenGroupNameIsEmpty()
		{
			// ülesseade
			KindergartenDto dto = MockKindergartenData();
			dto.GroupName = string.Empty;

			// tegevus
			var result = await Svc<IKindergartenServices>().Create(dto);

			// kontroll
			Assert.NotNull(result);
			Assert.Equal(string.Empty, result.GroupName);
		}

		// 5. Kontrollib, et lasteaia loomisel tühja asutuse nimega salvestatakse kirje edukalt tühja sõnega.
		[Fact]
		public async Task Should_CreateKindergarten_WhenKindergartenNameIsEmpty()
		{
			// ülesseade
			KindergartenDto dto = MockKindergartenData();
			dto.KindergartenName = string.Empty;

			// tegevus
			var result = await Svc<IKindergartenServices>().Create(dto);

			// kontroll
			Assert.NotNull(result);
			Assert.Equal(string.Empty, result.KindergartenName);
		}

		// 6. Kontrollib, et lasteaia loomisel tühja õpetaja nimega salvestatakse kirje edukalt tühja sõnega.
		[Fact]
		public async Task Should_CreateKindergarten_WhenTeacherNameIsEmpty()
		{
			// ülesseade
			KindergartenDto dto = MockKindergartenData();
			dto.TeacherName = string.Empty;

			// tegevus
			var result = await Svc<IKindergartenServices>().Create(dto);

			// kontroll
			Assert.NotNull(result);
			Assert.Equal(string.Empty, result.TeacherName);
		}

		// 7. Kontrollib, et lasteaia saab edukalt luua piiripealse väärtusega, kui laste arv on täpselt 3.
		[Fact]
		public async Task Should_CreateKindergarten_WhenChildrenCountIsExactlyThree()
		{
			// ülesseade
			KindergartenDto dto = MockKindergartenData();
			dto.ChildrenCount = 3;

			// tegevus
			var result = await Svc<IKindergartenServices>().Create(dto);

			// kontroll
			Assert.NotNull(result);
			Assert.Equal(3, result.ChildrenCount);
		}

		// 8. Kontrollib, et loodud lasteaia pärimisel selle ID järgi vastavad andmed täielikult salvestatule.
		[Fact]
		public async Task Should_GetCorrectKindergarten_WhenQueriedByCreatedId()
		{
			// ülesseade
			KindergartenDto dto = MockKindergartenData();
			var created = await Svc<IKindergartenServices>().Create(dto);

			// tegevus
			var fetched = await Svc<IKindergartenServices>().DetailAsync((Guid)created.Id);

			// kontroll
			Assert.NotNull(fetched);
			Assert.Equal(created.Id, fetched.Id);
			Assert.Equal(created.KindergartenName, fetched.KindergartenName);
			Assert.Equal(created.TeacherName, fetched.TeacherName);
		}

		// 9. Kontrollib, et andmete uuendamisel muutub UpdatedAt ajatempel võrdseks või uuemaks kui CreatedAt.
		[Fact]
		public async Task Should_UpdateTimestamp_WhenKindergartenIsUpdated()
		{
			// ülesseade
			KindergartenDto dto = MockKindergartenData();
			var created = await Svc<IKindergartenServices>().Create(dto);

			Svc<TARge25ShopContext>().ChangeTracker.Clear();

			KindergartenDto updateDto = MockKindergartenData(true);
			updateDto.Id = created.Id;
			updateDto.CreatedAt = created.CreatedAt;

			// tegevus
			await Svc<IKindergartenServices>().Update(updateDto);
			Svc<TARge25ShopContext>().ChangeTracker.Clear();
			var resultInDb = await Svc<IKindergartenServices>().DetailAsync((Guid)created.Id);

			// kontroll
			Assert.NotNull(resultInDb);
			Assert.True(resultInDb.UpdatedAt >= resultInDb.CreatedAt);
		}

		// 10. Kontrollib, et lasteaia andmete uuendamisel jääb algne loomise aeg (CreatedAt) andmebaasis muutmata.
		[Fact]
		public async Task ShouldNot_ModifyCreatedAt_WhenKindergartenIsUpdated()
		{
			// ülesseade
			KindergartenDto dto = MockKindergartenData();
			var created = await Svc<IKindergartenServices>().Create(dto);
			DateTime originalCreatedAt = created.CreatedAt;

			Svc<TARge25ShopContext>().ChangeTracker.Clear();

			KindergartenDto updateDto = MockKindergartenData(true);
			updateDto.Id = created.Id;
			updateDto.CreatedAt = originalCreatedAt; // säilitame algse loomisaja
			updateDto.UpdatedAt = DateTime.UtcNow;

			// tegevus
			await Svc<IKindergartenServices>().Update(updateDto);
			Svc<TARge25ShopContext>().ChangeTracker.Clear();
			var resultInDb = await Svc<IKindergartenServices>().DetailAsync((Guid)created.Id);

			// kontroll
			Assert.NotNull(resultInDb);
			Assert.Equal(originalCreatedAt, resultInDb.CreatedAt);
			Assert.NotEqual(resultInDb.UpdatedAt, resultInDb.CreatedAt);
		}

		// 11. Kontrollib, et ainult rühma nime muutmisel uueneb rühm, kuid lasteaia nimi jääb samaks.
		[Fact]
		public async Task Should_UpdateOnlyGroupName_WhenOnlyGroupChanged()
		{
			// ülesseade
			KindergartenDto dto = MockKindergartenData();
			var created = await Svc<IKindergartenServices>().Create(dto);

			Svc<TARge25ShopContext>().ChangeTracker.Clear();

			KindergartenDto updateDto = new KindergartenDto
			{
				Id = created.Id,
				KindergartenName = created.KindergartenName,
				TeacherName = created.TeacherName,
				ChildrenCount = created.ChildrenCount,
				CreatedAt = created.CreatedAt,
				UpdatedAt = DateTime.UtcNow,
				GroupName = "Täiesti Uus Rühm"
			};

			// tegevus
			await Svc<IKindergartenServices>().Update(updateDto);
			var resultInDb = await Svc<IKindergartenServices>().DetailAsync((Guid)created.Id);

			// kontroll
			Assert.NotNull(resultInDb);
			Assert.Equal("Täiesti Uus Rühm", resultInDb.GroupName);
			Assert.Equal(created.KindergartenName, resultInDb.KindergartenName);
		}

		// 12. Kontrollib, et juba kustutatud lasteaia teistkordsel kustutamisel tagastatakse null, kuna kirjet enam pole.
		[Fact]
		public async Task ShouldNot_DeleteTwice_WhenKindergartenAlreadyDeleted()
		{
			// ülesseade
			KindergartenDto dto = MockKindergartenData();
			var created = await Svc<IKindergartenServices>().Create(dto);

			// tegevus
			var firstDelete = await Svc<IKindergartenServices>().Delete((Guid)created.Id);
			var secondDelete = await Svc<IKindergartenServices>().Delete((Guid)created.Id);

			// kontroll
			Assert.NotNull(firstDelete);
			Assert.Null(secondDelete);
		}

		// 13. Kontrollib, et negatiivse laste arvuga lasteaeda ei looda või määratakse laste arvuks positiivne väärtus.
		[Fact]
		public async Task ShouldNot_CreateKindergarten_WhenChildrenCountIsNegative()
		{
			// ülesseade
			KindergartenDto dto = MockKindergartenData();
			dto.ChildrenCount = -15;

			// tegevus
			var result = await Svc<IKindergartenServices>().Create(dto);

			// kontroll
			Assert.True(result == null || result.ChildrenCount > 0);
		}
		// 14. Kontrollib, et õpetaja nime muutmisel uueneb ainult õpetaja nimi ja teised väljad säilitavad väärtused.
		[Fact]
		public async Task Should_UpdateKindergartenTeacherNameByID_WhenUpdatingTeacherName()
		{
			//

			KindergartenDto dto = MockKindergartenData();
			var result = await Svc<IKindergartenServices>().Create(dto);
			Guid guid = (Guid)result.Id;

			Svc<TARge25ShopContext>().ChangeTracker.Clear();

			KindergartenDto domain = new();

			domain.Id = guid;
			domain.ChildrenCount = dto.ChildrenCount;
			domain.TeacherName = "Name";
			domain.KindergartenName = dto.KindergartenName;
			domain.GroupName = dto.GroupName;
			domain.CreatedAt = dto.CreatedAt;//  <-- ei tohi muutuda Update korral, tuleb võtta olemasolevast objektist.
			domain.UpdatedAt = DateTime.UtcNow;//  <-- PEAB muutuma Update korral

			//tegevus
			await Svc<IKindergartenServices>().Update(domain);
			Svc<TARge25ShopContext>().ChangeTracker.Clear();
			var result2 = await Svc<IKindergartenServices>().DetailAsync(guid);

			//kontroll
			Assert.Equal(result.Id, result2.Id);
			Assert.NotEqual(dto.TeacherName, result2.TeacherName);
			Assert.Equal(dto.ChildrenCount, result2.ChildrenCount);
			//Assert.Equal(dto.Name, result2.Name);
			Assert.Equal(dto.KindergartenName, result2.KindergartenName);
			Assert.Equal(dto.GroupName, result2.GroupName);
			Assert.NotEqual(dto.UpdatedAt, result2.UpdatedAt);
			Assert.True(result2.UpdatedAt > result2.CreatedAt);
		}

		// 15. Kontrollib, et laste arvu muutmisel uueneb ainult laste arv ja muud kirje väljad jäävad samaks.
		[Fact]
		public async Task Should_UpdateKindergartenChildrenCountByID_WhenUpdatingChildrenCount()
		{
			//

			KindergartenDto dto = MockKindergartenData();
			var result = await Svc<IKindergartenServices>().Create(dto);
			Guid guid = (Guid)result.Id;

			Svc<TARge25ShopContext>().ChangeTracker.Clear();

			KindergartenDto domain = new();

			domain.Id = guid;
			domain.ChildrenCount = 100;
			domain.TeacherName = dto.TeacherName;
			domain.KindergartenName = dto.KindergartenName;
			domain.GroupName = dto.GroupName;
			domain.CreatedAt = dto.CreatedAt;//  <-- ei tohi muutuda Update korral, tuleb võtta olemasolevast objektist.
			domain.UpdatedAt = DateTime.UtcNow;//  <-- PEAB muutuma Update korral

			//tegevus
			await Svc<IKindergartenServices>().Update(domain);
			Svc<TARge25ShopContext>().ChangeTracker.Clear();
			var result2 = await Svc<IKindergartenServices>().DetailAsync(guid);

			//kontroll
			Assert.Equal(result.Id, result2.Id);
			Assert.Equal(dto.TeacherName, result2.TeacherName);
			Assert.NotEqual(dto.ChildrenCount, result2.ChildrenCount);
			//Assert.Equal(dto.Name, result2.Name);
			Assert.Equal(dto.KindergartenName, result2.KindergartenName);
			Assert.Equal(dto.GroupName, result2.GroupName);
			Assert.NotEqual(dto.UpdatedAt, result2.UpdatedAt);
			Assert.True(result2.UpdatedAt > result2.CreatedAt);
		}

		private KindergartenDto MockKindergartenNullData()
        {
            return new KindergartenDto
            {
                Id = null,
                GroupName = "",
                ChildrenCount = 0,
                KindergartenName = "",
                TeacherName = "",
                CreatedAt = DateTime.MinValue,
                UpdatedAt = DateTime.MinValue,
            };
        }


        /* üleval testid, all abimeetodid */

        private KindergartenDto MockKindergartenData(bool isOneOrTwo = false)
        {
            if (isOneOrTwo == false)
            {
                return new KindergartenDto
                {
                    GroupName = "X AE a L 12 menuornvöerv",
                    ChildrenCount = 67,
                    KindergartenName = "lendav taldrik",
                    TeacherName = "69",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
            }
            else
            {
                return new KindergartenDto
                {
                    GroupName = "RAKETT69",
                    ChildrenCount = 420,
                    KindergartenName =  "lendav kauss",
                    TeacherName = "999",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
            }
        }

    }
}
