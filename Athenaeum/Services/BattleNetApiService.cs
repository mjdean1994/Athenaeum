using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Athenaeum.Models;
using WowDotNetAPI;

namespace Athenaeum.Services
{
    public class BattleNetApiService
    {

        public static void UpdateCharacter(int characterId)
        {
            var db = new ApplicationDbContext();

            var character = db.Characters.Find(characterId);
            var armoryCharacter = db.ArmoryCharacters.FirstOrDefault(x => x.CharacterId == characterId);

            var explorer = new WowExplorer(Region.US, Locale.en_US, ConfigurationManager.AppSettings["BattleNetApiKey"]);
            WowDotNetAPI.Models.Character apiCharacter;
            try
            {
                apiCharacter = explorer.GetCharacter("wyrmrest-accord", character.Name,
                    CharacterOptions.GetEverything);
            }
            catch (Exception ex)
            {
                return;
            }

            if (armoryCharacter == null)
            {
                armoryCharacter = new ArmoryCharacter();
                armoryCharacter.CharacterId = characterId;
                db.ArmoryCharacters.Add(armoryCharacter);
            }

            armoryCharacter.AchievementPoints = apiCharacter.AchievementPoints;
            var specs = apiCharacter.Talents.Select(x => x.Spec).ToArray();

            armoryCharacter.PrimarySpec = specs[0].Name;
            armoryCharacter.SecondarySpec = specs[1].Name;

            armoryCharacter.Class = apiCharacter.Class.ToString();

            armoryCharacter.Rating2v2 = Convert.ToInt32(apiCharacter.PvP.Brackets.Bracket2v2.Rating);
            armoryCharacter.Rating3v3 = Convert.ToInt32(apiCharacter.PvP.Brackets.Bracket3v3.Rating);
            armoryCharacter.Rating5v5 = Convert.ToInt32(apiCharacter.PvP.Brackets.Bracket5v5.Rating);
            armoryCharacter.RatingRbg = Convert.ToInt32(apiCharacter.PvP.Brackets.BracketRbg.Rating);

            character.ArmoryCharacterId = armoryCharacter.ArmoryCharacterId;

            db.SaveChanges();
        }
    }
}