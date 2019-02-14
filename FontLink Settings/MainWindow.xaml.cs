using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Win32;

using Ookii.Dialogs.Wpf;

namespace FontLinkSettings
{
    public partial class MainWindow : Window
    {
        private RegistryKey                            _parent;
        private ObservableCollection<FontLinkFallback> _fallbacks;

        public void ShowErrorDialog(String heading, String body)
        {
            var dialog = new TaskDialog
                         {
                             AllowDialogCancellation = false,
                             MainIcon                = TaskDialogIcon.Error,
                             WindowTitle             = "FontLink Settings",
                             MainInstruction         = heading,
                             Content = body                +
                                       Environment.NewLine +
                                       Environment.NewLine +
                                       "Try restart the application as administrator.",
                             ButtonStyle = TaskDialogButtonStyle.Standard,
                         };
            dialog.Buttons.Add(new TaskDialogButton
                               {
                                   ButtonType = ButtonType.Ok
                               });
            dialog.ShowDialog(this);
        }

        public void ShowInfoDialog(String heading, String body)
        {
            var dialog = new TaskDialog
                         {
                             AllowDialogCancellation = false,
                             MainIcon                = TaskDialogIcon.Information,
                             WindowTitle             = "FontLink Settings",
                             MainInstruction         = heading,
                             Content                 = body,
                             ButtonStyle             = TaskDialogButtonStyle.Standard,
                         };
            dialog.Buttons.Add(new TaskDialogButton
                               {
                                   ButtonType = ButtonType.Close
                               });
            dialog.ShowDialog(this);
        }

        public MainWindow()
        {
            InitializeComponent();
            _fallbacks = new ObservableCollection<FontLinkFallback>();
        }

        private void OnWindowLoaded(Object sender, RoutedEventArgs e)
        {
            var dialog = new TaskDialog
                         {
                             AllowDialogCancellation = false,
                             MainIcon                = TaskDialogIcon.Warning,
                             WindowTitle             = "FontLink Settings",
                             MainInstruction         = "FontLinks Are System Settings Saved in the Windows Registry",
                             Content = "The application will read from the Windows Registry but will not" +
                                       " perform writes without explicit user interactions."              +
                                       Environment.NewLine                                                +
                                       Environment.NewLine                                                +
                                       "Changing system settings affects all programs and "               +
                                       "all users on this computer. Please note that "                    +
                                       "improper configuration may cause issues, "                        +
                                       "such as jumbled texts, application crashes "                      +
                                       "and severe system instability. "                                  +
                                       "Do not save any change unless you know what you are doing!",
                             ButtonStyle = TaskDialogButtonStyle.CommandLinks
                         };
            dialog.Buttons.Add(new TaskDialogButton
                               {
                                   ButtonType = ButtonType.Custom,
                                   Text       = "I understand the risk!",
                                   CommandLinkNote = "Continue to the application." +
                                                     Environment.NewLine            +
                                                     "Administrative privilege may be required.",
                                   Default = false
                               });
            dialog.Buttons.Add(new TaskDialogButton
                               {
                                   ButtonType      = ButtonType.Custom,
                                   Text            = "Oh shit, quit now!",
                                   CommandLinkNote = "This scares the hell out of me.",
                                   Default         = true
                               });

            if (!dialog.ShowDialog(this).Text.StartsWith("I", StringComparison.InvariantCulture))
            {
                Close();
                return;
            }

            try
            {
                _parent = Registry.LocalMachine?.OpenSubKey("Software")?.OpenSubKey("Microsoft")
                                 ?.OpenSubKey("Windows NT")?.OpenSubKey("CurrentVersion")
                                 ?.OpenSubKey("FontLink")?.OpenSubKey("SystemLink");

                BaseFont.ItemsSource = _parent?.GetValueNames();
            }
            catch (SecurityException exception)
            {
                ShowErrorDialog("System.SecurityException",
                                exception.Message);
                Close();
            }

            AltFonts.ItemsSource = _fallbacks;
        }

        private void OnBaseFontSelected(Object sender, SelectionChangedEventArgs e)
        {
            Save.IsEnabled = false;

            if (!(BaseFont.SelectedItem is String key)) return;

            try
            {
                if (!(_parent.GetValue(key) is String[] strings))
                {
                    throw new InvalidOperationException("Registry format unrecognized.");
                }

                _fallbacks.Clear();
                foreach (var value in strings)
                {
                    var parts = value.Split(',');

                    var alt = new FontLinkFallback
                              {
                                  Collection = parts[0],
                                  Typeface   = parts[1]
                              };

                    if (parts.Length > 2)
                    {
                        alt.GDISize     = Int32.Parse(parts[2], new NumberFormatInfo());
                        alt.DirectXSize = Int32.Parse(parts[3], new NumberFormatInfo());
                    }

                    _fallbacks.Add(alt);

                    Save.IsEnabled = true;
                }
            }
            catch (SecurityException exception)
            {
                ShowErrorDialog("System.SecurityException",
                                exception.Message);
            }
            catch (IOException exception)
            {
                ShowErrorDialog("System.IOException",
                                exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                ShowErrorDialog("System.UnauthorizedAccessException",
                                exception.Message);
            }
            catch (InvalidOperationException exception)
            {
                ShowErrorDialog("System.InvalidOperationException",
                                exception.Message);
            }
        }

        private void OnWindowClosed(Object sender, EventArgs e)
        {
            _parent?.Dispose();
        }

        private void OnSaveSettings(Object sender, RoutedEventArgs e)
        {
            var key     = BaseFont.SelectedItem as String;
            var strings = _fallbacks.Select(x => x.ExportString).ToArray();

            var dialog = new TaskDialog
                         {
                             AllowDialogCancellation = false,
                             MainIcon                = TaskDialogIcon.Warning,
                             WindowTitle             = "FontLink Settings",
                             MainInstruction         = "Save These Settings?",
                             Content = "Registry key:"                                   +
                                       Environment.NewLine                               +
                                       "HKEY_LOCAL_MACHINE\\...\\FontLink\\SystemLink\\" +
                                       key                                               +
                                       Environment.NewLine                               +
                                       Environment.NewLine                               +
                                       "Multi-String (REG_MULTI_SZ) value:"              +
                                       Environment.NewLine                               +
                                       String.Join(Environment.NewLine, strings),
                             ButtonStyle = TaskDialogButtonStyle.CommandLinks
                         };
            dialog.Buttons.Add(new TaskDialogButton
                               {
                                   ButtonType      = ButtonType.Custom,
                                   Text            = "Yes, save to Registry.",
                                   CommandLinkNote = "The specified Registry key will be modified.",
                                   Default         = false
                               });
            dialog.Buttons.Add(new TaskDialogButton
                               {
                                   ButtonType      = ButtonType.Custom,
                                   Text            = "No, go back!",
                                   CommandLinkNote = "I am not ready to risk it."
                               });

            if (!dialog.ShowDialog(this).Text.StartsWith("Yes", StringComparison.InvariantCulture))
            {
                return;
            }

            try
            {
                using (var writable = Registry.LocalMachine?.OpenSubKey("Software")?.OpenSubKey("Microsoft")
                                             ?.OpenSubKey("Windows NT")?.OpenSubKey("CurrentVersion")
                                             ?.OpenSubKey("FontLink")?.OpenSubKey("SystemLink", true))
                {
                    if (writable == null)
                    {
                        throw new InvalidOperationException("Error opening the registry with write access.");
                    }

                    writable.SetValue(key, strings, RegistryValueKind.MultiString);
                }

                ShowInfoDialog($"FontLink Entry for {key} Saved.",
                               "Sign out and back in to see changes; or keep working on other settings.");
            }
            catch (SecurityException exception)
            {
                ShowErrorDialog("System.SecurityException",
                                exception.Message);
            }
            catch (IOException exception)
            {
                ShowErrorDialog("System.IOException",
                                exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                ShowErrorDialog("System.UnauthorizedAccessException",
                                exception.Message);
            }
            catch (InvalidOperationException exception)
            {
                ShowErrorDialog("System.InvalidOperationException",
                                exception.Message);
            }
        }
    }
}
