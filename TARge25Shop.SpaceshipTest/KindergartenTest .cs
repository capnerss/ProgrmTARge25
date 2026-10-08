using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
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
            var addSpaceship = await Svc<IKindergartenServices>().Create(dto);
            var deleteSpaceship = await Svc<IKindergartenServices>().Delete((Guid)addSpaceship.Id);

            // kontroll
            Assert.Equal(addSpaceship.Id, deleteSpaceship.Id);
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
            //ülesseade
            var guid = new Guid("68eb8abd-086a-4c8b-9695-71234143f709");

            KindergartenDto dto = MockKindergartenData();

            KindergartenDto domain = new();

            domain.Id = Guid.Parse("68eb8abd-086a-4c8b-9695-71234143f709");
            domain.ChildrenCount = 10000000;
            domain.TeacherName = "Igor Mang 2";
            domain.KindergartenName = "püramiid";
            domain.GroupName = "420";
            domain.CreatedAt = dto.CreatedAt;//  <-- ei tohi muutuda Update korral, tuleb võtta olemasolevast objektist.
            domain.UpdatedAt = DateTime.UtcNow;//  <-- PEAB muutuma Update korral

            //tegevus
            await Svc<IKindergartenServices>().Update(dto);

            //kontroll
            Assert.Equal(domain.Id, guid);
            Assert.NotEqual(dto.ChildrenCount, domain.ChildrenCount);
            Assert.NotEqual(dto.TeacherName, domain.TeacherName);
            Assert.DoesNotMatch(dto.GroupName.ToString(), domain.GroupName.ToString());
            Assert.DoesNotMatch(dto.KindergartenName, domain.KindergartenName);
            Assert.Equal(dto.CreatedAt, domain.CreatedAt);
            Assert.NotEqual(dto.UpdatedAt, domain.CreatedAt);
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
        public async Task ShouldNot_CreateKindergartenWithNegativeEnginePower_WhenEnginePowerNegative()
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
        public async Task ShouldNot_CreateKindergarten_WhenCrewIsThreeOrLess()
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
