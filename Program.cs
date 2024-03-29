﻿using System.Numerics;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace TextRPG2
{
    public class Character
    {
        public string Name { get;}
        public string Job { get; }
        public int Level { get; }
        public int Gold { get; set; }
        public int Hp { get; set; }
        public int Atk { get; }
        public int Def { get; }
        public Character(string name, string job, int level, int atk, int def, int hp, int gold)
        {
            Name = name;
            Job = job;
            Level = level;
            Gold = gold;
            Hp = hp;
            Atk = atk;
            Def = def;
        }
    }
    public class Item
    {
        public string Name { get; }
        public string Description { get; }
        public int Type { get; }
        public int Atk { get; }
        public int Def { get; }
        public int Hp { get; }
        public int Gold { get; }
        public bool IsEquipped { get; set; }
        public bool IsPurchased { get; set; }
        public static int ItemCnt = 0;
        public static int ShopItemCnt = 0;

        public Item(string name, string description, int type, int atk, int def, int hp, int gold, bool isEquipped = false)
        {
            Name = name;
            Description = description;
            Type = type;
            Atk = atk;
            Def = def;
            Hp = hp;
            Gold = gold;
            IsEquipped = isEquipped;
            IsPurchased = false;
        }

        public void PrintitemStatDescription(bool withnumber = false, int idx = 0)
        {
            Console.Write("- ");
            if (withnumber)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write("{0}. ", idx);
                Console.ResetColor();
            }
            if (IsEquipped)
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("E");
                Console.ResetColor();
                Console.Write("]");
                Console.Write(PadRightForMixedText(Name, 9));
            }
            else Console.Write(PadRightForMixedText(Name, 12));
            Console.Write(" | ");

            if (Atk != 0) Console.Write($"Atk {(Atk >= 0 ? "+" : "")}{Atk}");
            if (Def != 0) Console.Write($"Def {(Def >= 0 ? "+" : "")}{Def}");
            if (Hp != 0) Console.Write($"Hp {(Hp >= 0 ? "+" : "")}{Hp}");

            Console.Write(" | ");

            Console.WriteLine(Description);
        }
        public void PrintShopitemStatDescription(bool withnumber = false, int idx = 0)
        {
            Console.Write("- ");
            Console.Write(PadRightForMixedText(Name, 15));
            Console.Write(" | ");
            if (Atk != 0) Console.Write($"Atk {(Atk >= 0 ? "+" : "")}{Atk}");
            if (Def != 0) Console.Write($"Def {(Def >= 0 ? "+" : "")}{Def}");
            if (Hp != 0) Console.Write($"Hp {(Hp >= 0 ? "+" : "")}{Hp}");

            Console.Write(" | ");
            Console.Write(PadRightForMixedText(Description, 50));
            Console.Write(" | ");
            if (IsPurchased)
            {
                Console.WriteLine(PadRightForMixedText("구매 완료", 9));
            }
            else
            {
                Console.WriteLine(PadRightForMixedText($"{Gold} G", 9));
            }

        }
        public void PrintBuyShopitemStatDescription(bool withnumber = false, int idx = 0)
        {
            Console.Write("- ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{idx}. ");
            Console.ResetColor();
            Console.Write(PadRightForMixedText(Name, 15));
            Console.Write(" | ");
            if (Atk != 0) Console.Write($"Atk {(Atk >= 0 ? "+" : "")}{Atk}");
            if (Def != 0) Console.Write($"Def {(Def >= 0 ? "+" : "")}{Def}");
            if (Hp != 0) Console.Write($"Hp {(Hp >= 0 ? "+" : "")}{Hp}");

            Console.Write(" | ");
            Console.Write(PadRightForMixedText(Description, 50));
            Console.Write(" | ");
            if (IsPurchased)
            {
                Console.WriteLine(PadRightForMixedText("구매 완료", 9));
            }
            else
            {
                Console.WriteLine(PadRightForMixedText($"{Gold} G", 9));
            }

        }

        public void PrintSellShopitemStatDescription(bool withnumber = false, int idx = 0)
        {
            Console.Write("- ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{idx}. ");
            Console.ResetColor();
            Console.Write(PadRightForMixedText(Name, 15));
            Console.Write(" | ");
            if (Atk != 0) Console.Write($"Atk {(Atk >= 0 ? "+" : "")}{Atk}");
            if (Def != 0) Console.Write($"Def {(Def >= 0 ? "+" : "")}{Def}");
            if (Hp != 0) Console.Write($"Hp {(Hp >= 0 ? "+" : "")}{Hp}");

            Console.Write(" | ");
            Console.Write(PadRightForMixedText(Description, 50));
            Console.Write(" | ");
            Console.WriteLine(PadRightForMixedText($"{Gold*0.85} G", 9));
        }

        public static int GetPrintableLength(string str)
        {
            int length = 0;
            foreach (char c in str)
            {
                if(char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.OtherLetter)
                {
                    length += 2;
                }
                else
                {
                    length += 1;
                }
            }
            return length;
        }

        public static string PadRightForMixedText(string str, int totalLength)
        {
            int currentLength = GetPrintableLength(str);
            int padding = totalLength - currentLength;
            return str.PadRight(str.Length + padding);
        }

    }
    internal class Program
    {
        static Character _player;
        static Item[] _items;
        static Item[] _shopItems;
        static void Main(string[] args)
        {
            GameDataSetting();
            PrintStartLogo();
            StartMenu();
        }

        private static void StartMenu()
        {
            Console.Clear();
            Console.WriteLine("================================================");
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.\n이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine("================================================");
            Console.WriteLine("\n1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전 입장");

            switch(CheckValidInput(1, 4))
            {
                case 1:
                    // 상태 보기
                    StatusMenu();
                    break;
                case 2:
                    // 인벤토리
                    InventoryMenu();
                    break;
                case 3:
                    //상점
                    Shop();
                    break;
                case 4:
                    DungeonEntry();
                    break;
            }
        }
        private static Random random = new Random();
        private static void DungeonEntry()
        {
            Console.Clear();
            ShowHighLighterText("■던전 입장■");
            Console.WriteLine("던전에 입장합니다. 난이도를 선택하세요.");
            Console.WriteLine("\n1. 쉬움 (권장 방어력: 5)");
            Console.WriteLine("2. 보통 (권장 방어력: 11)");
            Console.WriteLine("3. 어려움 (권장 방어력: 17)");
            Console.WriteLine("0. 나가기");

            int dungeonChoice = CheckValidInput(0, 3);

            if (dungeonChoice == 0)
            {
                StartMenu();
            }
            else
            {
                int recommendedDefense = GetRecommendedDefense(dungeonChoice);
                int playerDefense = _player.Def + getSumBonusDef();

                Console.WriteLine($"\n[던전 정보]");
                Console.WriteLine($"- 권장 방어력: {recommendedDefense}");
                Console.WriteLine($"- 내 방어력: {playerDefense}");

                if (playerDefense < recommendedDefense)
                {
                    Console.WriteLine($"\n던전에 도전하기에는 내 방어력이 부족합니다.");
                    Console.WriteLine($"던전 성공 확률: 60%");
                    Console.WriteLine($"던전에 도전하시겠습니까? (Y/N)");

                    if (Console.ReadKey().Key == ConsoleKey.Y)
                    {
                        if (GetRandomNumber(1, 101) <= 60)
                        {
                            FailDungeon(recommendedDefense);
                        }
                        else
                        {
                            Console.WriteLine("\n던전 도전에 성공했습니다!");
                            ClearDungeon(dungeonChoice);
                        }
                    }
                    else
                    {
                        DungeonEntry();
                    }
                } else
                {
                    Console.WriteLine($"\n던전 도전 확률: 100%");
                    Console.WriteLine($"던전에 도전하시겠습니까? (Y/N)");

                    if (Console.ReadKey().Key == ConsoleKey.Y)
                    {
                        Console.WriteLine("\n던전 도전에 성공했습니다!");
                        ClearDungeon(dungeonChoice);
                    }
                    else
                    {
                        DungeonEntry();
                    }
                }
            }
        }
        private static void FailDungeon(int recommendedDefense)
        {
            Console.WriteLine($"\n던전 도전에 실패했습니다. 보상 없음.");
            _player.Hp = Math.Max(0, _player.Hp - GetRandomNumber(10, 31));

            Console.WriteLine($"\n던전 도전 후 상태:");
            Console.WriteLine($"- 현재 체력: {_player.Hp}");
            Console.WriteLine($"계속하려면 아무 키나 누르세요...");
            Console.ReadKey();

            StartMenu();
        }

        private static void ClearDungeon(int dungeonChoice)
        {
            Console.WriteLine($"\n던전 클리어 축하합니다!!");
            int basicReward = GetBasicDungeonReward(dungeonChoice);
            int additionalReward = GetAdditionalReward(_player.Atk);

            int totalReward = basicReward + additionalReward;
            _player.Gold += totalReward;

            Console.WriteLine($"\n[탐험 결과]");
            Console.WriteLine($"- 체력 {100} -> {_player.Hp}");
            Console.WriteLine($"- 보상 {basicReward} G -> {totalReward} G");

            Console.WriteLine($"\n0. 나가기");
            int choice = CheckValidInput(0, 0);
            if (choice == 0)
            {
                StartMenu();
            }
        }

        private static int GetRecommendedDefense(int dungeonChoice)
        {
            switch (dungeonChoice)
            {
                case 1: return 5;
                case 2: return 11;
                case 3: return 17;
                default: return 0;
            }
        }
        private static int GetRandomNumber(int min, int max)
        {
            return random.Next(min, max + 1);
        }

        private static int GetBasicDungeonReward(int dungeonChoice)
        {
            switch (dungeonChoice)
            {
                case 1:
                    return GetRandomNumber(20, 35);
                case 2:
                    return GetRandomNumber(20, 35); 
                case 3:
                    return GetRandomNumber(20, 35); 
                default:
                    return 0;
            }
        }
        private static int GetAdditionalReward(int attackPower)
        {
            // 추가 보상은 공격력의 백분율로 획득 가능 (공격력 * 1.0)
            int percentage = GetRandomNumber(10, 20); // 10에서 20 사이의 랜덤 백분율을 얻습니다.

            int additionalReward = (int)(attackPower * (percentage / 100.0));

            return additionalReward;
        }

        private static void Shop()
        {
            Console.Clear();
            ShowHighLighterText("■상점■");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine("\n[보유 골드]");
            Console.WriteLine("{0} G", _player.Gold);
            Console.WriteLine("\n[아이템 목록]");
            for (int i = 0; i < Item.ShopItemCnt; i++)
            {
                _shopItems[i].PrintShopitemStatDescription(true, i + 1);
            }
            Console.WriteLine("\n1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기");
            switch (CheckValidInput(0, 2))
            {
                case 2:
                    //아이템 판매
                    SellShop();
                    break;
                case 1:
                    //아이템 구매
                    BuyShop();
                    break;
                case 0:
                    // 나가기
                    StartMenu();
                    break;
            }
        }

        private static void SellShop()
        {
            Console.Clear();
            ShowHighLighterText("■상점 - 아이템 판매■");
            Console.WriteLine("판매할 아이템을 선택하세요.");
            Console.WriteLine("\n[보유 골드]");
            Console.WriteLine("{0} G", _player.Gold);
            Console.WriteLine("\n[아이템 목록]");

            for (int i = 0; i < Item.ItemCnt; i++)
            {
                _items[i].PrintSellShopitemStatDescription(true, i + 1);
            }

            Console.WriteLine("\n0. 나가기");

            int selectedItemIndex = CheckValidInput(0, Item.ItemCnt);

            if (selectedItemIndex == 0)
            {
                // 나가기 옵션 선택
                Shop();
            }
            else if (selectedItemIndex >= 1 && selectedItemIndex <= Item.ItemCnt)
            {
                int itemIndex = selectedItemIndex - 1;

                             
                _items[itemIndex].IsEquipped = false;
                _player.Gold += (int)(_items[itemIndex].Gold * 0.85);
                Console.WriteLine($"아이템 '{_items[itemIndex].Name}'을(를) 판매했습니다.");
                Console.WriteLine($"판매 가격으로 {_items[itemIndex].Gold * 0.85}G를 획득했습니다.");

                RemoveItem(itemIndex);
                
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }

            Console.WriteLine("계속하려면 아무 키나 누르세요...");
            Console.ReadKey();

            // 다시 상점으로 이동
            Shop();
        }
        static void RemoveItem(int index)
        {
            for (int i = index; i < Item.ItemCnt - 1; i++)
            {
                _items[i] = _items[i + 1];
            }
            _items[Item.ItemCnt - 1] = null;
            Item.ItemCnt--;
        }

        private static void BuyShop()
        {
            Console.Clear();
            ShowHighLighterText("■상점 - 아이템 구매■");
            Console.WriteLine("구매할 아이템을 선택하세요.");
            Console.WriteLine("\n[보유 골드]");
            Console.WriteLine("{0} G", _player.Gold);
            Console.WriteLine("\n[아이템 목록]");
            //상점에 있는 아이템 목록
            for (int i = 0; i < Item.ShopItemCnt; i++)
            {
                _shopItems[i].PrintBuyShopitemStatDescription(true, i + 1);
            }
            // 사용자 인풋
            int selectedItemIndex = CheckValidInput(0, Item.ShopItemCnt);

            if (selectedItemIndex == 0)
            {
                // 나가기 옵션 선택
                Shop();
            }
            else if (selectedItemIndex >= 1 && selectedItemIndex <= Item.ShopItemCnt)
            {
                int itemIndex = selectedItemIndex - 1;

                if (_shopItems[itemIndex].IsPurchased)
                {
                    Console.WriteLine($"이 아이템 '{_shopItems[itemIndex].Name}'은(는) 이미 구매한 아이템입니다.");
                }
                else if (_player.Gold >= _shopItems[itemIndex].Gold)
                {
                    // 인벤토리에 아이템 추가
                    AddItem(new Item(_shopItems[itemIndex].Name, _shopItems[itemIndex].Description,
                        _shopItems[itemIndex].Type, _shopItems[itemIndex].Atk, _shopItems[itemIndex].Def,
                        _shopItems[itemIndex].Hp, _shopItems[itemIndex].Gold, false));

                    _player.Gold -= _shopItems[itemIndex].Gold;
                    _shopItems[itemIndex].IsPurchased = true;

                    Console.WriteLine($"구매를 완료했습니다.");
                    Console.WriteLine($"골드를 {_shopItems[itemIndex].Gold}G만큼 차감했습니다.");
                    Console.WriteLine($"아이템 '{_shopItems[itemIndex].Name}'을(를) 인벤토리에 추가했습니다.");

                    // 상점에서의 구매 완료 표시
                    Console.WriteLine("상점에서 구매 완료.");
                }
                else
                {
                    Console.WriteLine("골드가 부족합니다.");
                }
            }
            else
            {
                // 유효하지 않은 입력
                Console.WriteLine("잘못된 입력입니다.");
            }

            Console.WriteLine("계속하려면 아무 키나 누르세요...");
            Console.ReadKey();

            // 다시 상점으로 이동
            Shop();
        }

        private static void InventoryMenu()
        {
            Console.Clear();
            ShowHighLighterText(" 인벤토리 ");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("\n[아이템 목록]");

            for(int i = 0;i<Item.ItemCnt;i++)
            {
                _items[i].PrintitemStatDescription();
            }
            Console.WriteLine("\n1. 장착관리");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");
            switch (CheckValidInput(0, 1))
            {
                case 0:
                    // 상태 보기
                    StartMenu();
                    break;
                case 1:
                    // 인벤토리
                    EquipMenu();
                    break;
            }
        }

        private static void EquipMenu()
        {
            Console.Clear() ;
            ShowHighLighterText("■인벤토리 - 장착 관리■");
            Console.WriteLine("보유중인 아이템을 관리 할 수 있습니다.");
            Console.WriteLine("\n[아이템 목록");
            for (int i = 0; i < Item.ItemCnt; i++)
            {
                _items[i].PrintitemStatDescription(true, i+1);
            }
            Console.WriteLine("\n0. 나가기");

            int keyInput = CheckValidInput(0, Item.ItemCnt);

            switch (keyInput)
            {
                case 0:
                    InventoryMenu();
                    break;
                default:
                    ToggleEquipStatus(keyInput - 1);
                    EquipMenu();
                    break;
            }
        }
        static Dictionary<int, bool> isTypeEquipped = new Dictionary<int, bool>();
        private static void ToggleEquipStatus(int idx)
        {
            if (!isTypeEquipped.ContainsKey(_items[idx].Type))
                isTypeEquipped[_items[idx].Type] = false;

            if (_items[idx].IsEquipped)
            {
                _items[idx].IsEquipped = false;
                isTypeEquipped[_items[idx].Type] = false;
                Console.WriteLine($"'{_items[idx].Name}'을(를) 해제했습니다.");
                Console.ReadKey();
            }
            else
            {
                if (isTypeEquipped[_items[idx].Type])
                {
                    Console.WriteLine($"{(_items[idx].Type == 0 ? "갑옷" : "무기")}이(가) 이미 장착되어 있습니다. 해제 후 다시 시도하세요.");
                    Console.ReadKey();
                }
                else
                {
                    _items[idx].IsEquipped = true;
                    isTypeEquipped[_items[idx].Type] = true;
                    Console.WriteLine($"'{_items[idx].Name}'을(를) 장착했습니다.");
                    Console.ReadKey();
                }
            }
        }


        private static void StatusMenu()
        {
            Console.Clear ();

            ShowHighLighterText("■상태 보기■");
            Console.WriteLine("캐릭터의 정보가 표기됩니다.");

            PrintWithHightlights("Lv.", _player.Level.ToString("00"));
            Console.WriteLine("\n{0} ({1})", _player.Name, _player.Job);

            int bonusAtk = getSumBonusAtk();
            PrintWithHightlights("공격력 :", (_player.Atk + bonusAtk).ToString(), bonusAtk > 0 ? string.Format(" (+{0})", bonusAtk): "");
            int bonusDef = getSumBonusDef();
            PrintWithHightlights("방어력 :", (_player.Def + bonusDef).ToString(), bonusDef > 0 ? string.Format(" (+{0})", bonusDef) : "");
            int bonusHp = getSumBonusHp();
            PrintWithHightlights("체력 :", (_player.Hp + bonusHp).ToString(), bonusHp > 0 ? string.Format(" (+{0})", bonusHp) : "");
            PrintWithHightlights("골드 :", _player.Gold.ToString());
            Console.WriteLine("\n0. 뒤로가기");
            Console.WriteLine("");
            switch(CheckValidInput(0, 0))
            {
                case 0:
                    StartMenu();
                    break;
            }
        }

        private static int getSumBonusAtk()
        {
            int sum = 0;
            for (int i = 0; i < Item.ItemCnt; i++)
            {
                if (_items[i].IsEquipped) sum += _items[i].Atk;
            }
            return sum;
        }
        private static int getSumBonusDef()
        {
            int sum = 0;
            for (int i = 0; i < Item.ItemCnt; i++)
            {
                if (_items[i].IsEquipped) sum += _items[i].Def;
            }
            return sum;
        }
        private static int getSumBonusHp()
        {
            int sum = 0;
            for (int i = 0; i < Item.ItemCnt; i++)
            {
                if (_items[i].IsEquipped) sum += _items[i].Hp;
            }
            return sum;
        }


        public static void ShowHighLighterText(string text)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        private static void PrintWithHightlights(string s1, string s2, string s3 = "")
        {
            Console.Write(s1);
            Console.ForegroundColor= ConsoleColor.Yellow;
            Console.Write(s2);
            Console.ResetColor ();
            Console.WriteLine(s3);
        }

        private static int CheckValidInput(int min, int max)
        {
            int keyInput; //tryParse
            bool result;
            do // 일단 한번 실행
            {
                Console.Write("\n원하시는 행동을 입력해주세요 : ");
                result = int.TryParse(Console.ReadLine(), out keyInput);
            } while (result == false || CheckIfValid(keyInput, min, max) == false);

            return keyInput;
        }

        private static bool CheckIfValid(int keyInput, int min, int max)
        {
            if (min <= keyInput && keyInput <= max) return true;
            return false;
        }

        private static void PrintStartLogo()
        {
            Console.WriteLine("======================================================================");
            Console.WriteLine("  ___________________   _____  __________ ___________ _____    ");
            Console.WriteLine(" /   _____/\\______   \\ /  _  \\ \\______   \\__    ___//  _  \\   ");

            Console.WriteLine(" \\_____  \\  |     ___//  /_\\  \\ |       _/  |    |  /  /_\\  \\  ");

            Console.WriteLine(" /        \\ |    |   /    |    \\|    |   \\  |    | /    |    \\ ");

            Console.WriteLine("/_______  / |____|   \\____|__  /|____|_  /  |____| \\____|__  / ");

            Console.WriteLine("        \\/                   \\/        \\/                  \\/  ");

            Console.WriteLine("                                                               ");

            Console.WriteLine("________                     ____                              ");

            Console.WriteLine("\\______ \\   __ __   ____    / ___\\   ____   ____    ____       ");

            Console.WriteLine(" |    |  \\ |  |  \\ /    \\  / /_/  >_/ __ \\ /  _ \\  /    \\      ");

            Console.WriteLine(" |    `   \\|  |  /|   |  \\ \\___  / \\  ___/(  <_> )|   |  \\     ");

            Console.WriteLine("/_______  /|____/ |___|  //_____/   \\___  >\\____/ |___|  /     ");

            Console.WriteLine("        \\/             \\/               \\/             \\/      ");
            Console.WriteLine("======================================================================");
            Console.WriteLine("                         PRESS ANY KEY TO START                       ");
            Console.WriteLine("======================================================================");
            Console.ReadKey();
        }

        private static void GameDataSetting()
        {
            _player = new Character("chad", "전사", 1, 10, 5, 100, 1500);
            _items = new Item[10];
            _shopItems = new Item[10];
            
            AddShopItem(new Item("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 0, 9, 0, 300));
            AddShopItem(new Item("낡은 검", "쉽게 볼 수 있는 낡은 검입니다.", 1, 2, 0, 0, 600));
            AddShopItem(new Item("수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", 0, 0, 5, 0, 1000));
            AddShopItem(new Item("스파르타의 갑옷", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 0, 0, 15, 0, 3500));
            AddShopItem(new Item("청동 도끼", "어디선가 사용됐던거 같은 도끼입니다.", 1, 5, 0, 0, 1500));
            AddShopItem(new Item("스파르타의 창", "스파르타의 전사들이 사용했다는 전설의 창입니다.", 1, 7, 0, 0, 4000));

        }
        static void AddItem(Item item)
        {
            if (Item.ItemCnt == 10) return;
            _items[Item.ItemCnt] = item; // 0개 => 0번 인덱스
            Item.ItemCnt++;
        }
        static void AddShopItem(Item item)
        {
            if (Item.ShopItemCnt == 10) return;
            _shopItems[Item.ShopItemCnt] = item; // 0개 => 0번 인덱스
            Item.ShopItemCnt++;
        }
    }
}
