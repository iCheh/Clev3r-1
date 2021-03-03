using Clever.CommonData;
using Clever.Model.Bplus;
using Clever.Model.Intellisense;
using Clever.Model.Utils;
using Clever.View.Dialogs;
using Clever.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;

namespace Clever
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Dictionary<string, string> Errors = new Dictionary<string, string>();
        public static ObservableCollection<IntellisenseObject> IntellisenseObjects;
        Localizator localizator = new Localizator();
        public static SplashScreen SplashScreen;

        public App()
        {           
        }

        void Aplication_StartUp(object sender, StartupEventArgs e)
        {
            var prc = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);

            if (prc.Length > 1)
            {
                try
                {
                    if (e.Args.Length >= 1)
                    {
                        var client = new CleverClient(e.Args[0]);
                        client.StartClient();
                    }
                    this.Shutdown();
                }
                catch (Exception ex)
                {
                    var message = new MessageWindow(ex.Message);
                    message.ShowDialog();
                    this.Shutdown();
                }
            }
            else
            {
                try
                {
                    var server = new CleverServer();
                    server.Start();
                    SplashScreen = new SplashScreen("Resources/splashscreen_nover.png");
                    SplashScreen.Show(false);
                    Configurations.Install();
                    GUILanguage.Install();
                    BpColors.Install();
                    Errors = localizator.ReadErrors();
                    IntellisenseObjects = new ReadXml().Read();     
                    MainWindow main = new MainWindow();
                    main.Show();
                    main.UpdateGridSize();

                    //new ValidData().GetData();

                    if (e.Args.Length > 0)
                    {
                        MainWindowVM.FileNew(e.Args[0], false);
                    }


                    if (!Configurations.Get.Association && !Configurations.Get.Association_NotShow)
                    {
                        var reg = new FirstAssociationWindow();
                        if (reg.ShowDialog() == true)
                        {
                            var result = new KeyRegistry("r");
                            Configurations.Get.Association = result.IsRegistry;
                        }
                    }

                }
                catch (Exception ex)
                {
                    var message = new MessageWindow(ex.Message);
                    message.ShowDialog();
                    this.Shutdown();
                }
            }
        }
    }
}
