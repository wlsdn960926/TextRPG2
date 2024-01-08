using System.Numerics;
using System.Xml.Linq;

namespace TextRPG2
{
    public class Character
    {
        public string Name { get;}
        public string Job { get; }
        public int Level { get; }
        public int Gold { get; set; }
        public int Hp { get; }
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

            switch(CheckValidInput(1, 3))
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
            }
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
            Console.WriteLine("0. 나가기");
            switch (CheckValidInput(0, 1))
            {
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
            if(!isTypeEquipped.ContainsKey(idx)) isTypeEquipped[idx] = false;
            if (isTypeEquipped[idx] == true && !_items[idx].IsEquipped) return;
            _items[idx].IsEquipped = !_items[idx].IsEquipped;
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


        private static void ShowHighLighterText(string text)
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
            AddItem(new Item("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 0, 9, 0, 300));
            AddItem(new Item("낡은 검", "쉽게 볼 수 있는 낡은 검입니다.", 1, 2, 0, 0, 600));
            AddItem(new Item("스파르타의 창", "스파르타의 전사들이 사용했다는 전설의 창입니다..", 2, 7, 0, 0, 4000));
            AddShopItem(new Item("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 0, 9, 0, 300));
            AddShopItem(new Item("낡은 검", "쉽게 볼 수 있는 낡은 검입니다.", 1, 2, 0, 0, 600));
            AddShopItem(new Item("수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", 2, 0, 5, 0, 1000));
            AddShopItem(new Item("스파르타의 갑옷", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3, 0, 15, 0, 3500));
            AddShopItem(new Item("청동 도끼", "어디선가 사용됐던거 같은 도끼입니다.", 4, 5, 0, 0, 1500));
            AddShopItem(new Item("스파르타의 창", "스파르타의 전사들이 사용했다는 전설의 창입니다.", 5, 7, 0, 0, 4000));

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
