using Clever.CommonData;
using Clever.Model.Utils;
using Clever.View.Panels;
using Clever.ViewModel.BaseVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Clever.ViewModel.PanelsVM
{
    internal class MediaPanelVM : BaseViewModel
    {
        ImageSource folderOpen;
        ImageSource folderClose;
        ImageSource soundImage;
        ImageSource imageImage;

        internal MediaPanelVM()
        {
            SetSetting();
        }

        #region Bindings
        public ObservableCollection<TreeViewItem> Images { get; private set; }
        public ObservableCollection<TreeViewItem> Sounds { get; private set; }

        private object _content;
        public object Content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged("Content");
            }
        }

        #endregion

        #region Configuration

        private void SetSetting()
        {
            Images = new ObservableCollection<TreeViewItem>();
            Sounds = new ObservableCollection<TreeViewItem>();

            folderOpen = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/folder_open.png"));
            folderClose = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/folder_close.png"));
            soundImage = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/sound.png"));
            imageImage = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/image.png"));

            AddDictionary();
        }

        #endregion

        #region Image Create

        private Dictionary<string, List<string>> GetImagesName
        {
            get
            {
                var images = new Dictionary<string, List<string>>();

                var expressions = new List<string>();
                var eyes = new List<string>();
                var information = new List<string>();
                var lego = new List<string>();
                var objects = new List<string>();
                var progress = new List<string>();
                var system = new List<string>();

                images.Add("Expressions", expressions);
                images.Add("Eyes", eyes);
                images.Add("Information", information);
                images.Add("LEGO", lego);
                images.Add("Objects", objects);
                images.Add("Progress", progress);
                images.Add("System", system);

                expressions.Add("Big smile");
                expressions.Add("Heart large");
                expressions.Add("Heart small");
                expressions.Add("Mouth 1 open");
                expressions.Add("Mouth 1 shut");
                expressions.Add("Mouth 2 open");
                expressions.Add("Mouth 2 shut");
                expressions.Add("Sad");
                expressions.Add("Sick");
                expressions.Add("Smile");
                expressions.Add("Swearing");
                expressions.Add("Talking");
                expressions.Add("Wink");
                expressions.Add("ZZZ");

                eyes.Add("Angry");
                eyes.Add("Awake");
                eyes.Add("Black eye");
                eyes.Add("Bottom left");
                eyes.Add("Bottom right");
                eyes.Add("Crazy 1");
                eyes.Add("Crazy 2");
                eyes.Add("Disappointed");
                eyes.Add("Dizzy");
                eyes.Add("Down");
                eyes.Add("Evil");
                eyes.Add("Hurt");
                eyes.Add("Knocked out");
                eyes.Add("Love");
                eyes.Add("Middle left");
                eyes.Add("Middle right");
                eyes.Add("Neutral");
                eyes.Add("Nuclear");
                eyes.Add("Pinch left");
                eyes.Add("Pinch middle");
                eyes.Add("Pinch right");
                eyes.Add("Sleeping");
                eyes.Add("Tear");
                eyes.Add("Tired left");
                eyes.Add("Tired middle");
                eyes.Add("Tired right");
                eyes.Add("Toxic");
                eyes.Add("Up");
                eyes.Add("Winking");

                information.Add("Accept");
                information.Add("Backward");
                information.Add("Decline");
                information.Add("Forward");
                information.Add("Left");
                information.Add("No go");
                information.Add("Question mark");
                information.Add("Right");
                information.Add("Stop 1");
                information.Add("Stop 2");
                information.Add("Thumbs down");
                information.Add("Thumbs up");
                information.Add("Warning");

                lego.Add("Color sensor");
                lego.Add("EV3 icon");
                lego.Add("EV3");
                lego.Add("Gyro sensor");
                lego.Add("IR beacon");
                lego.Add("IR sensor");
                lego.Add("Large motor");
                lego.Add("LEGO");
                lego.Add("Medium motor");
                lego.Add("MINDSTORMS");
                lego.Add("Sound sensor");
                lego.Add("Temp sensor");
                lego.Add("Touch sensor");
                lego.Add("US sensor");

                objects.Add("Bomb");
                objects.Add("Boom");
                objects.Add("Fire");
                objects.Add("Flowers");
                objects.Add("Forest");
                objects.Add("Light off");
                objects.Add("Light on");
                objects.Add("Lightning");
                objects.Add("Night");
                objects.Add("Pirate");
                objects.Add("Snow");
                objects.Add("Target");

                progress.Add("Bar 0");
                progress.Add("Bar 1");
                progress.Add("Bar 2");
                progress.Add("Bar 3");
                progress.Add("Bar 4");
                progress.Add("Dial 0");
                progress.Add("Dial 1");
                progress.Add("Dial 2");
                progress.Add("Dial 3");
                progress.Add("Dial 4");
                progress.Add("Dots 0");
                progress.Add("Dots 1");
                progress.Add("Dots 2");
                progress.Add("Dots 3");
                progress.Add("Hourglass 0");
                progress.Add("Hourglass 1");
                progress.Add("Hourglass 2");
                progress.Add("Timer 0");
                progress.Add("Timer 1");
                progress.Add("Timer 2");
                progress.Add("Timer 3");
                progress.Add("Timer 4");
                progress.Add("Water level 0");
                progress.Add("Water level 1");
                progress.Add("Water level 2");
                progress.Add("Water level 3");

                system.Add("Accept 1");
                system.Add("Accept 2");
                system.Add("Alert");
                system.Add("Box");
                system.Add("Busy 0");
                system.Add("Busy 1");
                system.Add("Decline 1");
                system.Add("Decline 2");
                system.Add("Dot empty");
                system.Add("Dot full");
                system.Add("EV3 small");
                system.Add("Play");
                system.Add("Slider 0");
                system.Add("Slider 1");
                system.Add("Slider 2");
                system.Add("Slider 3");
                system.Add("Slider 4");
                system.Add("Slider 5");
                system.Add("Slider 6");
                system.Add("Slider 7");
                system.Add("Slider 8");

                return images;
            }
        }

        #endregion

        #region Sound Create
        private Dictionary<string, List<string>> GetSoundsName
        {
            get
            {
                var sound = new Dictionary<string, List<string>>();

                var animals = new List<string>();
                var colors = new List<string>();
                var communication = new List<string>();
                var expressions = new List<string>();
                var information = new List<string>();
                var mechanical = new List<string>();
                var movements = new List<string>();
                var numbers = new List<string>();
                var system = new List<string>();

                sound.Add("Animals", animals);
                sound.Add("Colors", colors);
                sound.Add("Communication", communication);
                sound.Add("Expressions", expressions);
                sound.Add("Information", information);
                sound.Add("Mechanical", mechanical);
                sound.Add("Movements", movements);
                sound.Add("Numbers", numbers);
                sound.Add("System", system);

                animals.Add("Cat purr");
                animals.Add("Dog bark 1");
                animals.Add("Dog bark 2");
                animals.Add("Dog growl");
                animals.Add("Dog sniff");
                animals.Add("Dog whine");
                animals.Add("Elephant call");
                animals.Add("Insect buzz 1");
                animals.Add("Insect buzz 2");
                animals.Add("Insect chirp");
                animals.Add("Snake hiss");
                animals.Add("Snake rattle");
                animals.Add("T-rex roar");

                colors.Add("Black");
                colors.Add("Blue");
                colors.Add("Brown");
                colors.Add("Green");
                colors.Add("Red");
                colors.Add("White");
                colors.Add("Yellow");

                communication.Add("Bravo");
                communication.Add("EV3");
                communication.Add("Fantastic");
                communication.Add("Game over");
                communication.Add("Go");
                communication.Add("Good job");
                communication.Add("Good");
                communication.Add("Goodbye");
                communication.Add("Hello");
                communication.Add("Hi");
                communication.Add("LEGO");
                communication.Add("MINDSTORMS");
                communication.Add("Morning");
                communication.Add("No");
                communication.Add("OKay");
                communication.Add("OKey-dokey");
                communication.Add("Sorry");
                communication.Add("Thank you");
                communication.Add("Yes");

                expressions.Add("Boing");
                expressions.Add("Boo");
                expressions.Add("Cheering");
                expressions.Add("Crying");
                expressions.Add("Fanfare");
                expressions.Add("Kung fu");
                expressions.Add("Laughing 1");
                expressions.Add("Laughing 2");
                expressions.Add("Magic wand");
                expressions.Add("Ouch");
                expressions.Add("Shouting");
                expressions.Add("Smack");
                expressions.Add("Sneezing");
                expressions.Add("Snoring");
                expressions.Add("Uh-oh");

                information.Add("Activate");
                information.Add("Analyze");
                information.Add("Backwards");
                information.Add("Color");
                information.Add("Detected");
                information.Add("Down");
                information.Add("Error alarm");
                information.Add("Error");
                information.Add("Flashing");
                information.Add("Forward");
                information.Add("Left");
                information.Add("Object");
                information.Add("Right");
                information.Add("Searching");
                information.Add("Start");
                information.Add("Stop");
                information.Add("Touch");
                information.Add("Turn");
                information.Add("Up");

                mechanical.Add("Air release");
                mechanical.Add("Airbrake");
                mechanical.Add("Backing alert");
                mechanical.Add("Blip 1");
                mechanical.Add("Blip 2");
                mechanical.Add("Blip 3");
                mechanical.Add("Blip 4");
                mechanical.Add("Horn 1");
                mechanical.Add("Horn 2");
                mechanical.Add("Laser");
                mechanical.Add("Motor idle");
                mechanical.Add("Motor start");
                mechanical.Add("Motor stop");
                mechanical.Add("Ratchet");
                mechanical.Add("Sonar");
                mechanical.Add("Tick tack");
                mechanical.Add("Walk");

                movements.Add("Arm 1");
                movements.Add("Arm 2");
                movements.Add("Arm 3");
                movements.Add("Arm 4");
                movements.Add("Drop load");
                movements.Add("Lift load");
                movements.Add("Servo 1");
                movements.Add("Servo 2");
                movements.Add("Servo 3");
                movements.Add("Servo 4");
                movements.Add("Slide load");
                movements.Add("Snap");
                movements.Add("Speed down");
                movements.Add("Speed idle");
                movements.Add("Speed up");
                movements.Add("Speeding");

                numbers.Add("Zero");
                numbers.Add("One");
                numbers.Add("Two");
                numbers.Add("Three");
                numbers.Add("Four");
                numbers.Add("Five");                
                numbers.Add("Six");
                numbers.Add("Seven");
                numbers.Add("Eight");
                numbers.Add("Nine");
                numbers.Add("Ten");

                system.Add("Click");
                system.Add("Confirm");
                system.Add("Connect");
                system.Add("Download");
                system.Add("General alert");
                system.Add("Overpower");
                system.Add("Power down");
                system.Add("Ready");
                system.Add("Start up");

                return sound;
            }
        }

        #endregion

        #region Tree View Items

        private TreeViewItem GetFolderItem(string text)
        {
            var h = new MediaPanelItemHeader();
            h.Text = text;
            h.Image = folderClose;
            var item = new TreeViewItem();
            item.Style = (Style)Application.Current.Resources["TreeViewItemMediaTwo"];
            item.Header = h;

            return item;
        }

        private TreeViewItem GetFileItem(string text, bool image)
        {
            var h = new MediaPanelItemHeader();
            h.Text = text;
            var item = new TreeViewItem();
            item.Style = (Style)Application.Current.Resources["TreeViewItemMediaThree"];
            item.IsVisibleChanged += Item_IsVisibleChanged;
            
            if (image)
            {
                item.MouseDoubleClick += Image_MouseDoubleClick;
                h.Image = imageImage;
            }
            else
            {
                item.MouseDoubleClick += Sound_MouseDoubleClick;
                h.Image = soundImage;
            }

            item.Header = h;

            return item;
        }

        #endregion

        #region Events

        private void Image_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var tri = sender as TreeViewItem;
            var h = tri.Header as MediaPanelItemHeader;
            //Status.Clear();
            //Status.Add(h);
            var content = new Image();
            content.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Media/png/" + h.Text + ".png"));
            content.ClipToBounds = false;
            content.Stretch = Stretch.None;
            content.VerticalAlignment = VerticalAlignment.Center;
            content.HorizontalAlignment = HorizontalAlignment.Center;
            var border = new Border();
            border.BorderThickness = new Thickness(1, 1, 1, 1);
            border.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            border.Width = 178;
            border.Height = 128;
            border.VerticalAlignment = VerticalAlignment.Center;
            border.HorizontalAlignment = HorizontalAlignment.Center;
            border.Child = content;
            Content = border;
        }

        private void Sound_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var tri = sender as TreeViewItem;
            var h = tri.Header as MediaPanelItemHeader;
            Content = null;
            new WaveSoundPayer().Play(h.Text);
            //Status.Clear();
            //Status.Add(h);
        }

        private void Item_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            foreach (var i in Images)
            {
                var tmpH = i.Header as MediaPanelItemHeader;
                if (i.IsExpanded)
                    tmpH.Image = folderOpen;
                else
                    tmpH.Image = folderClose;
            }

            foreach (var i in Sounds)
            {
                var tmpH = i.Header as MediaPanelItemHeader;
                if (i.IsExpanded)
                    tmpH.Image = folderOpen;
                else
                    tmpH.Image = folderClose;
            }
        }

        #endregion

        #region Utils

        private void AddDictionary()
        {
            var image = GetImagesName;

            foreach (var i in image)
            {
                var folder = i.Key;
                var files = i.Value;
                var folderItem = GetFolderItem(folder);

                foreach (var file in files)
                {
                    folderItem.Items.Add(GetFileItem(file, true));
                }

                Images.Add(folderItem);
            }

            var sound = GetSoundsName;

            foreach (var i in sound)
            {
                var folder = i.Key;
                var files = i.Value;
                var folderItem = GetFolderItem(folder);

                foreach (var file in files)
                {
                    folderItem.Items.Add(GetFileItem(file, false));
                }

                Sounds.Add(folderItem);
            }
        }

        #endregion
    }
}
