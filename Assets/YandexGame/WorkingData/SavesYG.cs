
using Assets.Content.Scripts.Player;
using Assets.Content.Scripts.UI;
using Assets.Content.Scripts.UI.Weak;
using System.Collections.Generic;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        public int money = 1;                       // Можно задать полям значения по умолчанию
        public string newPlayerName = "Hello!";
        public bool[] openLevels = new bool[3];

        // Ваши сохранения
        public RewardData RewardData = new() { Rewards = new RewardType[7], AllRewards = 7 };
        public List<int> ItemsEquipment = new List<int>();
        public int OpenUnit = 1;
        public List<InventoryWeakItemType> ItemTypes = new List<InventoryWeakItemType>();
        public List<int> Chests = new List<int>();
        public List<int> Items = new List<int>();
        public float Money;
        public float Score;
        public float NeedScore;
        public float CurrentScore;
        public int CurrentLevel;
        public int NeedLevel = 1;
        public int CurrentIdPlayer;

        public float MaxHealthPlayer = 100;
        public float CurrentHealthPlayer = 100;
        public float DamagePlayer = 50;
        // ...

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            // Допустим, задать значения по умолчанию для отдельных элементов массива

            openLevels[1] = true;
        }
    }
}
