using UnityEngine;

namespace MC.Core
{
    public class StandardGameMode : MonoBehaviour
    {
        public int teamACount, teamBCount;

        public CharacterData[] teamACharacters, teamBCharacters;

        public Transform[] teamASpawnPoints, teamBSpawnPoints;

        public WeaponData defaultWeaponData;

        private void Start()
        {
            var teamA = CreateTeam(teamACount, teamACharacters, teamASpawnPoints);
            var teamB = CreateTeam(teamBCount, teamBCharacters, teamBSpawnPoints);

            foreach (var bot in teamB.friendBots)
            {
                teamA.enemies.Add(bot);
            }

            foreach (var bot in teamA.friendBots)
            {
                teamB.enemies.Add(bot);
            }

        }


        private DecisiveManager CreateTeam(int teammateCount, CharacterData[] characterDatas, Transform[] spawnPoints)
        {
            var decisiveManager = new GameObject("Decisive Manager", typeof(DecisiveManager)).GetComponent<DecisiveManager>();

            for (var i = 0; i < teammateCount; i++)
            {
                var randomCharacter = characterDatas[Random.Range(0, characterDatas.Length)];

                var bot = CreateBot(randomCharacter, "AssaultRifle");

                bot.transform.position = spawnPoints[i].position;
                bot.transform.rotation = spawnPoints[i].rotation;

                decisiveManager.friendBots.Add(bot);
            }

            return decisiveManager;
        }

        private SimpleBot CreateBot(CharacterData characterData, string weapon)
        {
            var bot = new GameObject("Bot", typeof(SimpleBot));

            var simpleBot = bot.GetComponent<SimpleBot>();

            simpleBot.botLogic = ScriptableObject.CreateInstance<DecisiveBotLogic>();

            simpleBot.characterData = characterData;
            simpleBot.currentWeapon = weapon;

            simpleBot.weaponData = Instantiate(defaultWeaponData);

            return simpleBot;
        }
    }
}
