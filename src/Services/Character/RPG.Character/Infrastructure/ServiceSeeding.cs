using Microsoft.EntityFrameworkCore;
using RPG.Character.Domain.CharacterAggregate;

namespace RPG.Character.Infrastructure

{
    public class ServiceSeeding
    {
        public async Task SeedAsync(ServiceDbContext context)
        {
            await context.Database.MigrateAsync();

            var ClassTypes = new List<ClassType>()
                {
                    new ClassType
                    {
                        Id = Guid.Parse("09ba84a5-076b-46e3-9d18-2de9033a1040"),
                        Name ="Adventurer",
                    },new ClassType
                    {
                        Id = Guid.Parse("7c576424-e31c-451f-bb51-7075e1593376"),
                        Name ="Warrior",
                    },new ClassType
                    {
                        Id = Guid.Parse("9de072b6-9361-4bb5-92a4-c2facc603a44"),
                        Name ="Mage",
                    },new ClassType
                    {
                        Id = Guid.Parse("3c1ae94b-31d5-4631-b54e-36ffded1ab48"),
                        Name ="Archer",
                    }
                };
            if (!await context.ClassType.AnyAsync())
            {
                await context.ClassType.AddRangeAsync(ClassTypes);
                await context.SaveChangesAsync();
            }

            var RaceTypes = new List<RaceType>()
                {
                    new RaceType
                    {
                        Id = Guid.Parse("a1bfd932-27ed-425c-820b-504a968b97de"),
                        Name ="Human",
                        Description ="Humans are a versatile and resilient race known for their diversity and adaptability. With their wide range of appearances, reflecting various ethnicities and cultures, humans possess the potential to excel in any field they choose. They are quick learners and masters of different professions and skills. Driven by ambition and determination, humans bravely face challenges and unite diverse groups toward common goals. Though lacking extraordinary physical abilities, their adaptability, versatility, and ability to form alliances make them formidable in their own right.",
                    },
                    //new RaceType
                    //{
                    //    Id = Guid.Parse("abfc4ad0-3564-4661-9265-39b867f4f6de"),
                    //    Name ="Goblin",
                    //    Description ="Goblins are small, cunning creatures with a wild appearance and sharp eyes. Despite their lack of physical strength, they make up for it with their agility and cleverness. Expert in stealth and surprise, goblins can navigate complex environments and excel in trap-making. However, their reputation for ruthlessness and deceit precedes them. Though some may find them unpleasant, goblins are survivors who adapt quickly and find ingenious solutions to challenges they face.",
                    //},new RaceType
                    //{
                    //    Id = Guid.Parse("6e3a7c06-55bb-4a5d-860e-5a4670776b5f"),
                    //    Name ="Lyons",
                    //    Description ="Lyons are humanoid creatures, embodying a lion-human hybrid. With a regal and powerful presence, they possess lion-like features such as a fur coat resembling a mane. Lyons exhibit heightened senses, strength, and intelligence. They are natural leaders and protectors, known for their courage and loyalty. Lyons have the ability to reason, engage in diplomacy, and understand social dynamics. They maintain a strong connection to nature, acting as guardians of the wilderness and fostering harmony with animals. This unique race blends the majesty of a lion with human attributes, creating a captivating mix of strength, intelligence, and compassion.",
                    //}
                };
            if (!await context.RaceType.AnyAsync())
            {                
                await context.RaceType.AddRangeAsync(RaceTypes);
                await context.SaveChangesAsync();
            }

            var BaseAttributes = new List<BaseAttributes>()
                {
                    new BaseAttributes
                    {
                        Id = Guid.Parse("9bd6f8f5-b688-4626-8984-b0fb51056988"),
                        Armor =10,
                        MagicalArmor = 8,
                        BaseHealth = 100,
                        Concentration =2,
                        DodgePercentage = 0,
                        CriticRatePercentage = 3,
                        MagicalDamageHigh = 18,
                        MagicalDamageLow = 14,
                        PhisycalDamageHigh = 22,
                        PhisycalDamageLow = 10,
                        ClassTypeId=Guid.Parse("09ba84a5-076b-46e3-9d18-2de9033a1040")
                    },new BaseAttributes
                    {
                        Id = Guid.Parse("b112b92e-6a3b-4ae3-9708-4ccea6f367d7"),
                        Armor =25,
                        MagicalArmor = 15,
                        BaseHealth = 300,
                        Concentration =0,
                        DodgePercentage = 5,
                        CriticRatePercentage = 6,
                        MagicalDamageHigh = 0,
                        MagicalDamageLow = 0,
                        PhisycalDamageHigh = 120,
                        PhisycalDamageLow = 70,
                        ClassTypeId=Guid.Parse("7c576424-e31c-451f-bb51-7075e1593376")
                    },new BaseAttributes
                    {
                        Id = Guid.Parse("f63e9223-504f-420e-a30b-2b8dac13e822"),
                        Armor =17,
                        MagicalArmor = 30,
                        BaseHealth = 200,
                        Concentration = 30,
                        DodgePercentage = 0,
                        CriticRatePercentage = 0,
                        MagicalDamageHigh = 110,
                        MagicalDamageLow = 95,
                        PhisycalDamageHigh = 0,
                        PhisycalDamageLow = 0,
                        ClassTypeId=Guid.Parse("9de072b6-9361-4bb5-92a4-c2facc603a44")
                    },new BaseAttributes
                    {
                        Id = Guid.Parse("ca761232-ed42-11ce-bacd-00aa0057b223"),
                        Armor =10,
                        MagicalArmor = 10,
                        BaseHealth = 50,
                        Concentration = 10,
                        DodgePercentage = 0,
                        CriticRatePercentage = 0,
                        MagicalDamageHigh = 0,
                        MagicalDamageLow = 0,
                        PhisycalDamageHigh = 0,
                        PhisycalDamageLow = 0,
                        RaceTypeId=Guid.Parse("a1bfd932-27ed-425c-820b-504a968b97de")
                    }
                };
            if (!await context.BaseAttributes.AnyAsync())
            {
                await context.BaseAttributes.AddRangeAsync(BaseAttributes);
                await context.SaveChangesAsync();
            }

        }
    }
}