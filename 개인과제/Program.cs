﻿using System.Numerics;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using static 개인과제.Program;

namespace 개인과제
{
    internal class Program
    {
        public interface ICharacter
        {
            int Level { get; }
            string Name { get; }
            string Chad { get; set; }
            int Atk { get; }
            int AtkMax { get; set; }
            int Def { get; set; }
            int Health { get; set; }
            int Money { get; set; }
            bool IsDead { get; }
            void TakeDamage(int damage);
        }

        public interface IMonster
        {
            string Name { get; }
            int Health { get; set; }
            int Atk { get; set; }
            bool IsDead { get; }
            void TakeDamage(int damage);
        }

        public class Monster : IMonster
        {
            public string Name { get; }
            public int Health { get; set; }
            public int Atk { get; set; }
            public Monster(string name, int health, int atk)
            {
                Name = name;
                Health = health;
                Atk = atk;

            }

            public bool IsDead => Health <= 0;
            public void TakeDamage(int damage)
            {
                Health -= damage;
                if (IsDead)
                {
                    Console.WriteLine($"{Name}이 사망하였습니다.");
                }
                else
                {
                    Console.WriteLine($"{Name}이 {damage}의 데미지를 받았습니다. 남은 체력 : {Health}입니다.");
                }
            }

            public class Goblin : Monster
            {
                public Goblin(string name) : base(name, 50, 5) { }
            }

            public class Dragon : Monster
            {
                public Dragon(string name) : base(name, 150, 15) { }
            }

            public class God : Monster
            {
                public God(string name) : base(name, 200, 20) { }
            }


            public class Warrior : ICharacter
            {
                public int Level { get; set; }
                public string Name { get; }
                public string Chad { get; set; }
                public int AtkMax { get; set; }
                public int Def { get; set; }
                public int Health { get; set; }
                public int Money { get; set; }
                public bool IsDead => Health <= 0;
                public int Atk => new Random().Next(AtkMax - 5, AtkMax);

                public Warrior(string name)
                {
                    Level = 1;
                    Name = name;
                    Health = 100;
                    Chad = "전사";
                    AtkMax = 10;
                    Def = 10;
                    Money = 1500;
                }

                public void TakeDamage(int damage)
                {
                    Health -= damage;
                    if (IsDead)
                    {
                        Console.WriteLine($"{Name}님이 사망하였습니다.");
                    }
                    else
                    {
                        Console.WriteLine($"{Name}님이 {damage}의 데미지를 받았습니다. 남은 체력 : {Health}입니다.");
                    }
                }
            }
            public class Ranger : ICharacter
            {
                public int Level { get; set; }
                public string Name { get; }
                public string Chad { get; set; }
                public int AtkMax { get; set; }
                public int Def { get; set; }
                public int Health { get; set; }
                public int Money { get; set; }
                public bool IsDead => Health <= 0;
                public int Atk => new Random().Next(AtkMax - 5, AtkMax);

                public Ranger(string name)
                {
                    Level = 1;
                    Name = name;
                    Health = 100;
                    Chad = "레인저";
                    AtkMax = 15;
                    Def = 5;
                    Money = 1500;
                }
                public void TakeDamage(int damage)
                {
                    Health -= damage;
                    if (IsDead)
                    {
                        Console.WriteLine($"{Name}님이 사망하였습니다.");
                    }
                    else
                    {
                        Console.WriteLine($"{Name}님이 {damage}의 데미지를 받았습니다. 남은 체력 : {Health}입니다.");
                    }
                }
            }
            public interface IInventory
            {
                string Name { get; }
                string Info { get; }

                int Price { get; }
                string Abstract { get; }
                bool IsEquip { get; set; }
                void Use(ICharacter character, List<IInventory> inventory);
                void Buy(ICharacter character, int price, List<IInventory> inventory);
            }

            public interface IWeapon : IInventory
            {
                int AttackBonus { get; }
            }

            public interface IArmor : IInventory
            {
                int DeffenceBonus { get; }
            }


            public class NormalSword : IWeapon
            {
                public string Name => "일반 소드";
                public string Info => "공격력 + 5";
                public int Price => 500;
                public int AttackBonus => 5;
                public string Abstract => "흔히 볼 수 있는 일반적인 검";
                public bool IsEquip { get; set; } = false;

                public void Use(ICharacter character, List<IInventory> inventory)
                {
                    if (IsEquip)
                    {
                        Console.WriteLine($"{Name}을(를) 해제합니다.");
                        character.AtkMax -= AttackBonus;
                        IsEquip = false;
                    }
                    else
                    {
                        foreach (var item in inventory)
                        {
                            if (inventory.Any(item => item.IsEquip && item is IWeapon))
                            {
                                Console.WriteLine($"이미{item.Name}을(를) 장착하고 있습니다.");
                                return;
                            }
                        }
                        Console.WriteLine($"{Name}을(를) 장착합니다. 공격력이 5 증가합니다.");
                        character.AtkMax += AttackBonus;
                        IsEquip = true;
                    }
                }

                public void Buy(ICharacter character, int Price, List<IInventory> inventory)
                {

                    if (character.Money >= Price)
                    {
                        character.Money -= Price;
                        inventory.Add(this);
                        Console.WriteLine("----------------------------------------------------");
                        Console.WriteLine($"{Name}을 구매하였습니다.\n총 {Price}골드를 소비하였습니다.");
                    }
                    else
                    {
                        Console.WriteLine("골드가 부족합니다.");
                    }
                }
            }

            public class GoldSword : IWeapon
            {
                public string Name => "골드 소드";
                public string Info => "공격력 + 10";

                public int Price => 1000;
                public int AttackBonus => 10;
                public string Abstract => "금으로 만든 값이 비싼 검";
                public bool IsEquip { get; set; } = false;

                public void Use(ICharacter character, List<IInventory> inventory)
                {
                    if (IsEquip)
                    {
                        Console.WriteLine($"{Name}을(를) 해제합니다.");
                        character.AtkMax -= AttackBonus;
                        IsEquip = false;
                    }
                    else
                    {
                        foreach (var item in inventory)
                        {
                            if (inventory.Any(item => item.IsEquip && item is IWeapon))
                            {
                                Console.WriteLine($"이미{item.Name}을(를) 장착하고 있습니다.");
                                return;
                            }
                        }
                        Console.WriteLine($"{Name}을(를) 장착합니다. 공격력이 5 증가합니다.");
                        character.AtkMax += AttackBonus;
                        IsEquip = true;
                    }

                }

                public void Buy(ICharacter character, int Price, List<IInventory> inventory)
                {

                    if (character.Money >= Price)
                    {
                        character.Money -= Price;
                        inventory.Add(this);
                        Console.WriteLine("----------------------------------------------------");
                        Console.WriteLine($"{Name}을 구매하였습니다.\n총 {Price}골드를 소비하였습니다.");
                    }
                    else
                    {
                        Console.WriteLine("골드가 부족합니다.");
                    }
                }
            }

            public class NormalArmor : IArmor
            {
                public string Name => "일반 갑옷";
                public string Info => "방어력 + 5";
                public int Price => 500;
                public int DeffenceBonus => 5;
                public string Abstract => "흔히 볼 수 있는 일반적인 갑옷";
                public bool IsEquip { get; set; } = false;

                public void Use(ICharacter character, List<IInventory> inventory)
                {
                    if (IsEquip)
                    {
                        Console.WriteLine($"{Name}을(를) 해제합니다.");
                        character.Def -= DeffenceBonus;
                        IsEquip = false;
                    }
                    else
                    {
                        foreach (var item in inventory)
                        {
                            if (inventory.Any(item => item.IsEquip && item is IArmor))
                            {
                                Console.WriteLine($"이미{item.Name}을(를) 장착하고 있습니다.");
                                return;
                            }
                        }
                        Console.WriteLine($"{Name}을(를) 장착합니다. 방어력이 5 증가합니다.");
                        character.Def += DeffenceBonus;
                        IsEquip = true;
                    }
                }
                public void Buy(ICharacter character, int Price, List<IInventory> inventory)
                {
                    if (character.Money >= Price)
                    {
                        character.Money -= Price;
                        inventory.Add(this);
                        Console.WriteLine("----------------------------------------------------");
                        Console.WriteLine($"{Name}을 구매하였습니다.\n총 {Price}골드를 소비하였습니다.");
                    }
                    else
                    {
                        Console.WriteLine("골드가 부족합니다.");
                    }
                }
            }

            public class GoldArmor : IArmor
            {
                public string Name => "골드 갑옷";
                public string Info => "방어력 + 10";
                public int Price => 1000;
                public int DeffenceBonus => 10;
                public string Abstract => "금으로 만든 값이 비싼 갑옷";
                public bool IsEquip { get; set; } = false;

                public void Use(ICharacter character, List<IInventory> inventory)
                {
                    if (IsEquip)
                    {
                        Console.WriteLine($"{Name}을(를) 해제합니다.");
                        character.Def -= DeffenceBonus;
                        IsEquip = false;
                    }
                    else
                    {
                        foreach (var item in inventory)
                        {
                            if (inventory.Any(item => item.IsEquip && item is IArmor))
                            {
                                Console.WriteLine($"이미{item.Name}을(를) 장착하고 있습니다.");
                                return;
                            }
                        }
                        Console.WriteLine($"{Name}을(를) 장착합니다. 방어력이 5 증가합니다.");
                        character.Def += DeffenceBonus;
                        IsEquip = true;
                    }
                }
                public void Buy(ICharacter character, int Price, List<IInventory> inventory)
                {
                    if (character.Money >= Price)
                    {
                        character.Money -= Price;
                        inventory.Add(this);
                        Console.WriteLine("----------------------------------------------------");
                        Console.WriteLine($"{Name}을 구매하였습니다.\n총 {Price}골드를 소비하였습니다.");
                    }
                    else
                    {
                        Console.WriteLine("골드가 부족합니다.");
                    }
                }
            }
            public class Shop
            {
                private ICharacter player;
                private List<IInventory> inventory;
                private List<IInventory> store;

                public Shop(ICharacter player, List<IInventory> inventory)
                {
                    this.player = player;
                    this.inventory = inventory;
                    store = new List<IInventory>() { new NormalArmor(), new GoldArmor(), new NormalSword(), new GoldSword() };
                }

                public void Shopping()
                {
                    while (true)
                    {
                        Console.WriteLine("----------------------------------------------------");
                        Console.WriteLine($"소지금: {player.Money}골드");
                        Console.WriteLine("구매할 아이템을 선택하세요.");

                        for (int i = 0; i < store.Count; i++)
                        {
                            string purchase = inventory.Contains(store[i]) ? "이미 구매" : "";
                            Console.WriteLine($"{i + 1}. {store[i].Name} - {store[i].Price}골드 - {store[i].Info} - {store[i].Abstract} - {purchase}");
                        }
                        Console.WriteLine("0. 나가기");
                        Console.Write("구매할 아이템의 번호를 입력하세요: ");
                        string input = Console.ReadLine();

                        if (input == "0")
                        {
                            Console.WriteLine("상점을 나갑니다.");
                            Console.WriteLine();
                            Thread.Sleep(500);
                            Console.Clear();
                            break;
                        }
                        int choice;
                        if (int.TryParse(input, out choice) && choice > 0 && choice <= store.Count)
                        {
                            IInventory selectedItem = store[choice - 1];
                            bool hasItem = inventory.Any(item => item.Name == selectedItem.Name);
                            if (hasItem)
                            {
                                Console.WriteLine("이미 인벤토리에 아이템이 있습니다.");
                            }
                            else
                            {
                                selectedItem.Buy(player, selectedItem.Price, inventory);
                                Console.WriteLine("구매한 아이템이 인벤토리에 추가되었습니다.");
                            }

                        }
                    }
                }
            }

            public class MyItem
            {
                private ICharacter player;
                private List<IInventory> inventory;
                private List<IInventory> equip;

                public MyItem(ICharacter player, List<IInventory> inventory)
                {
                    this.player = player;
                    this.inventory = inventory;
                    equip = new List<IInventory>();
                }
                public void ShowMyItem()
                {
                    while (true)
                    {
                        if (inventory.Count <= 0)
                        {
                            Console.WriteLine("인벤토리에 아이템이 없습니다.");
                        }
                        int choose;
                        Console.WriteLine("인벤토리에 있는 아이템 목록 : ");
                        for (int i = 0; i < inventory.Count; i++)
                        {
                            string equipStatus = inventory[i].IsEquip ? "장착 중" : "미장착";
                            Console.WriteLine($"{i + 1}. {inventory[i].Name} - {equipStatus}");
                        }

                        Console.WriteLine("0. 나가기");
                        Console.Write("장착할 아이템을 입력하세요: ");
                        string input = Console.ReadLine();

                        if (input == "0")
                        {
                            Console.WriteLine("인벤토리를 나갑니다.");
                            Console.Clear();
                            Thread.Sleep(500);
                            return;
                        }

                        if (int.TryParse(input, out choose) && choose > 0 && choose <= inventory.Count)
                        {
                            IInventory selectedItem = inventory[choose - 1];

                            selectedItem.Use(player, inventory);
                        }
                        else
                        {
                            Console.WriteLine("잘못 입력하였습니다. 다시 확인해주세요.");
                        }

                    }

                }

                public void UpdateMyStatus()
                {
                    int totalAtkBonus = inventory.Where(item => item.IsEquip && item is IWeapon).Sum(item => (item as IWeapon).AttackBonus);
                    int totalDefBonus = inventory.Where(item => item.IsEquip && item is IArmor).Sum(item => (item as IArmor).DeffenceBonus);
                    Console.WriteLine($"공격력: {player.AtkMax} ({totalAtkBonus})");
                    Console.WriteLine($"방어력: {player.Def} ({totalDefBonus})");
                }
            }



            public class MainLobby
            {
                private ICharacter player;
                private MyItem myItem;
                private Shop shop;
                private List<IInventory> inventory;
                private Dictionary<string, Stage> stageDictionary;
                private IMonster monster;

                public MainLobby(ICharacter player, List<IInventory> inventory)
                {
                    this.player = player;
                    myItem = new MyItem(player, inventory);
                    Goblin monster1 = new Goblin("Goblin");
                    Dragon monster2 = new Dragon("Dragon");
                    God monster3 = new God("God");
                    shop = new Shop(player, inventory);
                    this.inventory = inventory;

                    stageDictionary = new Dictionary<string, Stage>
                    {
                        { "1", new Stage(player, monster1, "쉬운 던전") },
                        { "2", new Stage(player, monster2,"일반 던전") },
                        { "3", new Stage(player, monster3, "어려운 던전")}
                    };


                }

                public void Resting()
                {
                    while (true)
                    {
                        Console.WriteLine("----------------------------------------------------");
                        Console.WriteLine("200 골드를 내시면 편안한 휴식을 취할 수 있습니다.");
                        Console.WriteLine("휴식 효과: 체력 20회복");
                        Console.WriteLine($"소지 골드: {player.Money}\n현재 체력: {player.Health}");
                        Console.WriteLine("1. 휴식하기\n0. 나가기\n번호를 입력해주세요: ");
                        string input = Console.ReadLine();
                        if (input == "1")
                        {
                            if (player.Money >= 200)
                            {
                                int beforeHealth = player.Health;
                                player.Money -= 200;
                                player.Health += 20;
                                int recoverHealth = player.Health - beforeHealth;
                                if (player.Health >= 100) player.Health = 100;
                                Console.WriteLine("----------------------------------------------------");
                                Console.WriteLine("휴식중....");
                                Thread.Sleep(3000);
                                Console.WriteLine($"200 골드를 사용하여 체력 {recoverHealth} 회복했습니다.");
                                Console.WriteLine($"남은 골드: {player.Money} 골드");
                            }
                            else
                            {
                                Console.WriteLine("골드가 부족합니다.");
                            }
                        }
                        else if (input == "0")
                        {
                            Thread.Sleep(500);
                            Console.Clear();
                            return;
                        }
                        else
                        {
                            Console.WriteLine("잘못된 입력입니다.");
                        }
                    }
                }

                public void ShowInfo()
                {
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("----------------------------------------------------");
                        Console.WriteLine($"플레이어 정보:\nLevel: {player.Level}\n이름: {player.Name}\n직업: {player.Chad}\n체력: {player.Health}");
                        myItem.UpdateMyStatus();

                        Console.WriteLine("0. 나가기");
                        string input = Console.ReadLine();

                        if (input == "0")
                        {
                            Thread.Sleep(500);
                            Console.Clear();
                            return;
                        }
                    }
                }

                public void StageSelect()
                {
                    Console.Clear();
                    Console.WriteLine("던전에 입장하였습니다.");
                    Console.WriteLine("던전의 난이도를 선택해주세요.");
                    Console.WriteLine("1. 쉬운 던전 (방어력 5 이상 권장)");
                    Console.WriteLine("2. 일반 던전 (방어력 11이상 권장)");
                    Console.WriteLine("3. 어려운 던전 (방어력 15이상 권장)");
                    string res = Console.ReadLine();

                    if (stageDictionary.TryGetValue(res, out Stage stage))
                    {
                        if (res == "1" && player.Def < 5)
                        {
                            monster.Atk += 10;
                            stage.Start();
                        }
                        else
                        {
                            stage.Start();
                        }
                    }
                }

                public void Info()
                {
                    Console.WriteLine();
                    Console.WriteLine("----------------------------------------------------");
                    Console.WriteLine($"{player.Name}님 안녕하세요. 스파르타 마을에 오신 것을 환영합니다.\n당신의 직업은 {player.Chad}입니다.");
                    Console.WriteLine("마을에서는 던전에 들어가기 전에 여러 활동을 할 수 있습니다.");
                    Console.WriteLine();
                    Console.Write("\n1. 상태창 보기\n2. 인벤토리 보기\n3. 상점\n4. 던전 입장\n5. 휴식하기(체력회복)\n번호를 입력해주세요 : ");
                    string inputInfo = Console.ReadLine();

                    if (inputInfo == "1")
                    {
                        Thread.Sleep(500);
                        Console.Clear();
                        ShowInfo();
                    }
                    else if (inputInfo == "2")
                    {
                        Thread.Sleep(500);
                        Console.Clear();
                        myItem.ShowMyItem();
                    }
                    else if (inputInfo == "3")
                    {
                        Thread.Sleep(500);
                        Console.Clear();
                        Console.Clear();
                        shop.Shopping();
                    }
                    else if (inputInfo == "4")
                    {
                        Thread.Sleep(500);
                        Console.Clear();
                        StageSelect();
                    }
                    else if (inputInfo == "5")
                    {
                        Thread.Sleep(500);
                        Console.Clear();
                        Resting();
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("----------------------------------------------------");
                        Console.WriteLine("잘못된 입력입니다");
                    }
                }
            }
            public class Stage
            {
                private ICharacter player;
                private IMonster monster;
                private int defenseRecommend;
                private string stageName;
                private Random random = new Random();

                public Stage(ICharacter player, IMonster monster, string stageName)
                {
                    this.player = player;
                    this.monster = monster;
                    this.stageName = stageName;
                    defenseRecommend = stageName switch
                    {
                        "쉬운 던전" => 5,
                        "일반 던전" => 11,
                        "어려운 던전" => 17
                    };
                }
                public void Start()
                {
                    Console.WriteLine("-------------------------------------");
                    Console.WriteLine("던전에 입장합니다.");
                    Thread.Sleep(1000);
                    Console.WriteLine("....");

                    if (player.Def < defenseRecommend && random.Next(100) < 40)
                    {
                        Console.WriteLine("방어력이 낮아 플레이어가 던전 입구에서 사망하였습니다.");
                        Console.WriteLine("던전 클리어 실패!!");
                        return;
                    }

                    Console.WriteLine($"{player.Name} VS {monster.Name}");
                    Thread.Sleep(1000);


                    while (!player.IsDead && !monster.IsDead)

                    {
                        Console.WriteLine();
                        Console.WriteLine($"{player.Name}의 공격!");
                        monster.TakeDamage(player.AtkMax);
                        Thread.Sleep(1000);

                        if (monster.IsDead)
                        {
                            Console.WriteLine($"{monster.Name} 처치 완료! 던전 클리어!");
                            Console.WriteLine($"남은 체력: {player.Health}");
                            Console.WriteLine("자동으로 마을로 복귀합니다.");
                            Thread.Sleep(3000);
                            Console.Clear();
                            return;
                        }

                        Console.WriteLine();
                        Console.WriteLine($"{monster.Name}의 반격!");
                        player.TakeDamage(monster.Atk);
                        Thread.Sleep(1000);
                    }

                    if (player.IsDead)
                    {
                        Console.WriteLine("플레이어 사망.... 던전 실패!!!");
                        Console.WriteLine("자동으로 마을로 복귀합니다.");
                        Thread.Sleep(3000);
                        Console.Clear();
                    }
                }

                private void StageClear(ICharacter player, IMonster monster)
                {
                    if (monster is Monster)
                    {
                        Console.WriteLine("스테이지 클리어");
                    }
                }
            }
            static void Main(string[] args)
            {
                Console.WriteLine("----------------------------------------------------");
                Console.Write("안녕하세요! 스파르타 RPG 게임을 시작합니다!\n플레이어의 이름을 설정하세요 : ");
                string playerName = Console.ReadLine();

                ICharacter player = null;

                while (player == null)
                {
                    Console.WriteLine("직업을 선택하세요");
                    Console.WriteLine("1. 전사 (Warrior) (공격력: 10  방어력: 10)");
                    Console.WriteLine("2. 레인저 (Ranger)(공격력: 15  방어력:  5)");
                    Console.WriteLine("번호를 입력해주세요");
                    string input = Console.ReadLine();

                    if (input == "1")
                    {
                        player = new Warrior($"{playerName}");
                        Console.Clear();
                        Thread.Sleep(500);
                    }

                    else if (input == "2")
                    {
                        player = new Ranger($"{playerName}");
                        Console.Clear();
                        Thread.Sleep(500);
                    }

                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("----------------------------------------------------");
                        Console.WriteLine("잘못된 입력입니다. 다시 선택해주세요");
                    }
                }
                List<IInventory> playerInventory = new List<IInventory>();
                MainLobby mainLobby = new MainLobby(player, playerInventory);

                while (true)
                {
                    mainLobby.Info();
                }


            }
        }
    }
}


