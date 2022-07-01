using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Twinstar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static IWebDriver Driver { get; set; }

        public string[] RaceAll = { "Human", "Dwarf", "Night elf", "Gnome", "Draenei", "Worgen", "Orc", "Undead", "Tauren", "Troll", "Goblin", "Blood elf" };
        public string[] Classes = { "Warrior", "Paladin", "Hunter", "Rogue", "Priest", "Shaman", "Mage", "Warlock", "Druid" };

        public bool[] Warrior = { true, true, true, true, true, true, true, true, true, true, true, true };
        public bool[] Paladin = { true, true, false, false, true, false, false, false, true, false, false, true };
        public bool[] Hunter = { true, true, true, false, true, true, true, true, true, true, true, true };
        public bool[] Rogue = { true, true, true, true, false, true, true, true, false, true, true, true };
        public bool[] Priest = { true, true, true, true, true, true, false, true, true, true, true, true };
        public bool[] Shaman = { false, true, false, false, true, false, true, false, true, true, true, false };
        public bool[] Mage = { true, true, true, true, true, true, true, true, false, true, true, true };
        public bool[] Warlock = { true, true, false, true, false, true, true, true, false, true, true, true };
        public bool[] Druid = { false, false, true, false, false, true, false, false, true, true, false, false };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (apollo1.IsChecked == false && apollo3.IsChecked == false)
            {
                MessageBox.Show("Realm must be choosen");
                return;
            }

            bool tradeForSelected = false;
            string user = username.Text;
            string pass = password.Password;
            string character = characterName.Text != string.Empty ? string.Concat(characterName.Text[0].ToString().ToUpper(), characterName.Text.AsSpan(1).ToString().ToLower()) : string.Empty;
            List<string> selectedClasses = new();
            List<bool> collection = new();

            if (warrior.IsChecked == true)
            {
                collection.AddRange(Warrior);
                selectedClasses.Add(Classes[0]);
                tradeForSelected = true;
            }
            if (paladin.IsChecked == true)
            {
                collection.AddRange(Paladin);
                selectedClasses.Add(Classes[1]);
                tradeForSelected = true;
            }
            if (hunter.IsChecked == true)
            {
                collection.AddRange(Hunter);
                selectedClasses.Add(Classes[2]);
                tradeForSelected = true;
            }
            if (rogue.IsChecked == true)
            {
                collection.AddRange(Rogue);
                selectedClasses.Add(Classes[3]);
                tradeForSelected = true;
            }
            if (priest.IsChecked == true)
            {
                collection.AddRange(Priest);
                selectedClasses.Add(Classes[4]);
                tradeForSelected = true;
            }
            if (shaman.IsChecked == true)
            {
                collection.AddRange(Shaman);
                selectedClasses.Add(Classes[5]);
                tradeForSelected = true;
            }
            if (mage.IsChecked == true)
            {
                collection.AddRange(Mage);
                selectedClasses.Add(Classes[6]);
                tradeForSelected = true;
            }
            if (warlock.IsChecked == true)
            {
                collection.AddRange(Warlock);
                selectedClasses.Add(Classes[7]);
                tradeForSelected = true;
            }
            if (druid.IsChecked == true)
            {
                collection.AddRange(Druid);
                selectedClasses.Add(Classes[8]);
                tradeForSelected = true;
            }

            if (!tradeForSelected)
            {
                MessageBox.Show("Choose the class you want to trade your character for.");
                return;
            }

            ChromeOptions _options = new();

            _options.AddArguments("--no-default-browser-check",
                "--no-first-run",
                "--disable-popup-blocking",
                "--start-maximized"
                );

            Driver = new ChromeDriver(_options)
            {
                Url = "https://www.twinstar.cz/manager/Login.aspx"
            };

            new FindBy(By.Id("ctl00_HeaderLoginView_ctl05_UserName")).WaitForElementToBeClickable().SendKeys(user);
            new FindBy(By.Id("ctl00_HeaderLoginView_ctl05_Password")).WaitForElementToBeClickable().SendKeys(pass);
            new FindBy(By.Id("ctl00_HeaderLoginView_ctl05_LoginBtn")).WaitForElementToBeClickable().Click();

            if (apollo1.IsChecked == true)
            {
                Driver.Navigate().GoToUrl("https://www.twinstar.cz/manager/ChangeServer.ashx?ID=9");
            }
            else if (apollo3.IsChecked == true)
            {
                Driver.Navigate().GoToUrl("https://www.twinstar.cz/manager/ChangeServer.ashx?ID=17");
            }

            Driver.Navigate().GoToUrl("https://www.twinstar.cz/manager/Auction/Create.aspx");

            if (new FindBy(By.ClassName("tmessages_error")).GetElement().Displayed)
            {
                Driver.Quit();
                MessageBox.Show("Your account is locked for auction. Please unlock it first.");
                return;
            }

            new FindBy(By.XPath("/html/body/form/div[3]/div[2]/div[4]/div/div[2]/div[3]/div/table/tbody/tr[1]/td[3]/div/a[1]")).GetElement().Click();
            new FindBy(By.XPath($"//a[text() = '{character} (lvl 85)']")).WaitForElementToBeClickable().Click();
            new FindBy(By.XPath("/html/body/form/div[3]/div[2]/div[4]/div/div[2]/div[3]/div/table/tbody/tr[2]/td[3]/div/a[1]")).GetElement().Click();
            new FindBy(By.XPath("//a[text() = 'Trade']")).WaitForElementToBeClickable().Click();

            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i])
                {
                    new FindBy(By.Id("ctl00_MainCPH_AddRestrcictionLB")).WaitForElementToBeClickable().Click();
                }
            }

            int o = 0;
            int u = 0;
            int z = 0;

            for (int i = 0; i < collection.Count; i++)
            {
                if (i != 0 && (i % RaceAll.Length) == 0)
                {
                    u = 0;
                    z++;
                }

                if (collection[i])
                {
                    new FindBy(By.XPath($"/html/body/form/div[3]/div[2]/div[4]/div/div[2]/div[3]/div/table/tbody/tr[4]/td[3]/div[{(o * 2) + 1}]/a[1]")).GetElement().Click();
                    new FindBy(By.XPath($"//div[{(o * 2) + 1}]/ul/li/a[text() = '{RaceAll[u]}']")).WaitForElementToBeClickable().Click();
                    new FindBy(By.XPath($"/html/body/form/div[3]/div[2]/div[4]/div/div[2]/div[3]/div/table/tbody/tr[4]/td[3]/div[{(o + 1) * 2}]/a[1]")).GetElement().Click();
                    new FindBy(By.XPath($"//div[{(o + 1) * 2}]/ul/li/a[text() = '{selectedClasses[z]}']")).WaitForElementToBeClickable().Click();

                    o++;
                }

                u++;
            }
        }
    }
}
