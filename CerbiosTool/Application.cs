using ImGuiNET;
using SharpDX.DXGI;
using SixLabors.Fonts;
using System.Diagnostics;
using System.Numerics;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace CerbiosTool
{
    public class Application
    {
        private Sdl2Window? m_window;
        private GraphicsDevice? m_graphicsDevice;
        private CommandList? m_commandList;
        private ImGuiController? m_controller;
        private PathPicker? m_biosFileOpenPicker;
        private PathPicker? m_biosFileSavePicker;
        private PathPicker? m_configFileOpenPicker;
        private PathPicker? m_configFileSavePicker;
        private OkDialog? m_okDialog;
        private Config m_config = new();
        private Settings m_settings = new();
        private bool m_biosLoaded = false;
        private byte[] m_biosData = Array.Empty<byte>();

        private readonly string m_version;

        public Application(string version)
        {
            m_version = version;
        }

        private static void SetXboxTheme()
        {
            ImGui.StyleColorsDark();
            var style = ImGui.GetStyle();
            var colors = style.Colors;
            colors[(int)ImGuiCol.Text] = new Vector4(0.94f, 0.94f, 0.94f, 1.00f);
            colors[(int)ImGuiCol.TextDisabled] = new Vector4(0.86f, 0.93f, 0.89f, 0.28f);
            colors[(int)ImGuiCol.WindowBg] = new Vector4(0.10f, 0.10f, 0.10f, 1.00f);
            colors[(int)ImGuiCol.ChildBg] = new Vector4(0.06f, 0.06f, 0.06f, 0.98f);
            colors[(int)ImGuiCol.PopupBg] = new Vector4(0.10f, 0.10f, 0.10f, 1.00f);
            colors[(int)ImGuiCol.Border] = new Vector4(0.11f, 0.11f, 0.11f, 0.60f);
            colors[(int)ImGuiCol.BorderShadow] = new Vector4(0.16f, 0.16f, 0.16f, 0.00f);
            colors[(int)ImGuiCol.FrameBg] = new Vector4(0.18f, 0.18f, 0.18f, 1.00f);
            colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.30f, 0.30f, 0.30f, 1.00f);
            colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.28f, 0.71f, 0.25f, 1.00f);
            colors[(int)ImGuiCol.TitleBg] = new Vector4(0.20f, 0.51f, 0.18f, 1.00f);
            colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.26f, 0.66f, 0.23f, 1.00f);
            colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4(0.16f, 0.16f, 0.16f, 0.75f);
            colors[(int)ImGuiCol.MenuBarBg] = new Vector4(0.14f, 0.14f, 0.14f, 0.00f);
            colors[(int)ImGuiCol.ScrollbarBg] = new Vector4(0.16f, 0.16f, 0.16f, 0.00f);
            colors[(int)ImGuiCol.ScrollbarGrab] = new Vector4(0.30f, 0.30f, 0.30f, 1.00f);
            colors[(int)ImGuiCol.ScrollbarGrabHovered] = new Vector4(0.24f, 0.60f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.ScrollbarGrabActive] = new Vector4(0.24f, 0.60f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.CheckMark] = new Vector4(0.26f, 0.66f, 0.23f, 1.00f);
            colors[(int)ImGuiCol.SliderGrab] = new Vector4(0.90f, 0.90f, 0.90f, 1.00f);
            colors[(int)ImGuiCol.SliderGrabActive] = new Vector4(1.00f, 1.00f, 1.00f, 1.00f);
            colors[(int)ImGuiCol.Button] = new Vector4(0.17f, 0.17f, 0.17f, 1.00f);
            colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.24f, 0.60f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.26f, 0.66f, 0.23f, 1.00f);
            colors[(int)ImGuiCol.Header] = new Vector4(0.24f, 0.60f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.24f, 0.60f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.HeaderActive] = new Vector4(0.24f, 0.60f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.Separator] = new Vector4(1.00f, 1.00f, 1.00f, 0.25f);
            colors[(int)ImGuiCol.SeparatorHovered] = new Vector4(0.13f, 0.87f, 0.16f, 0.78f);
            colors[(int)ImGuiCol.SeparatorActive] = new Vector4(0.25f, 0.75f, 0.10f, 1.00f);
            colors[(int)ImGuiCol.ResizeGrip] = new Vector4(0.47f, 0.83f, 0.49f, 0.04f);
            colors[(int)ImGuiCol.ResizeGripHovered] = new Vector4(0.28f, 0.71f, 0.25f, 0.78f);
            colors[(int)ImGuiCol.ResizeGripActive] = new Vector4(0.28f, 0.71f, 0.25f, 1.00f);
            colors[(int)ImGuiCol.Tab] = new Vector4(0.26f, 0.67f, 0.23f, 0.95f);
            colors[(int)ImGuiCol.TabHovered] = new Vector4(0.24f, 0.60f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.TabActive] = new Vector4(0.24f, 0.60f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.TabUnfocused] = new Vector4(0.21f, 0.54f, 0.19f, 0.99f);
            colors[(int)ImGuiCol.TabUnfocusedActive] = new Vector4(0.24f, 0.60f, 0.21f, 1.00f);
            colors[(int)ImGuiCol.PlotLines] = new Vector4(0.86f, 0.93f, 0.89f, 0.63f);
            colors[(int)ImGuiCol.PlotLinesHovered] = new Vector4(0.28f, 0.71f, 0.25f, 1.00f);
            colors[(int)ImGuiCol.PlotHistogram] = new Vector4(0.86f, 0.93f, 0.89f, 0.63f);
            colors[(int)ImGuiCol.PlotHistogramHovered] = new Vector4(0.28f, 0.71f, 0.25f, 1.00f);
            colors[(int)ImGuiCol.TextSelectedBg] = new Vector4(0.26f, 0.66f, 0.23f, 1.00f);
            colors[(int)ImGuiCol.DragDropTarget] = new Vector4(1.00f, 1.00f, 0.00f, 0.90f);
            colors[(int)ImGuiCol.NavHighlight] = new Vector4(0.28f, 0.71f, 0.25f, 1.00f);
            colors[(int)ImGuiCol.NavWindowingHighlight] = new Vector4(1.00f, 1.00f, 1.00f, 0.70f);
            colors[(int)ImGuiCol.NavWindowingDimBg] = new Vector4(0.80f, 0.80f, 0.80f, 0.20f);
            colors[(int)ImGuiCol.ModalWindowDimBg] = new Vector4(0.16f, 0.16f, 0.16f, 0.73f);

            style.WindowRounding = 6;
            style.FrameRounding = 6;
            style.PopupRounding = 6;
        }

        private static void DrawToggle(bool enabled, bool hovered, Vector2 pos, Vector2 size)
        {
            var drawList = ImGui.GetWindowDrawList();

            float radius = size.Y * 0.5f;
            float rounding = size.Y * 0.25f;
            float slotHalfHeight = size.Y * 0.5f;

            var background = hovered ? ImGui.GetColorU32(enabled ? ImGuiCol.FrameBgActive : ImGuiCol.FrameBgHovered) : ImGui.GetColorU32(enabled ? ImGuiCol.CheckMark : ImGuiCol.FrameBg);

            var paddingMid = new Vector2(pos.X + radius + (enabled ? 1 : 0) * (size.X - radius * 2), pos.Y + size.Y / 2);
            var sizeMin = new Vector2(pos.X, paddingMid.Y - slotHalfHeight);
            var sizeMax = new Vector2(pos.X + size.X, paddingMid.Y + slotHalfHeight);
            drawList.AddRectFilled(sizeMin, sizeMax, background, rounding);

            var offs = new Vector2(radius * 0.8f, radius * 0.8f);
            drawList.AddRectFilled(paddingMid - offs, paddingMid + offs, ImGui.GetColorU32(ImGuiCol.SliderGrab), rounding);
        }

        private static bool Toggle(string str_id, ref bool v, Vector2 size)
        {
            ImGui.PushStyleColor(ImGuiCol.Button, ImGui.GetColorU32(new Vector4()));

            var style = ImGui.GetStyle();

            ImGui.PushID(str_id);
            bool status = ImGui.Button("###toggle_button", size);
            if (status)
            {
                v = !v;
            }
            ImGui.PopID();

            var maxRect = ImGui.GetItemRectMax();
            var toggleSize = new Vector2(size.X - 8, size.Y - 8);
            var togglePos = new Vector2(maxRect.X - toggleSize.X - style.FramePadding.X, maxRect.Y - toggleSize.Y - style.FramePadding.Y);
            DrawToggle(v, ImGui.IsItemHovered(), togglePos, toggleSize);

            ImGui.PopStyleColor();

            return status;
        }

        [DllImport("dwmapi.dll", PreserveSig = true)]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, uint attr, ref int attrValue, int attrSize);

        public void OpenUrl(string url)
        {
            try
            {
                if (OperatingSystem.IsWindows())
                {
                    Process.Start("cmd", "/C start" + " " + url);
                }
                else if (OperatingSystem.IsLinux())
                {
                    Process.Start("xdg-open", url);
                }
                else if (OperatingSystem.IsMacOS())
                {
                    Process.Start("open", url);
                }
            }
            catch
            {
                // do nothing
            }
        }

        public void Run()
        {
            VeldridStartup.CreateWindowAndGraphicsDevice(new WindowCreateInfo(50, 50, 556 + 340 + 50, 410 + 130, WindowState.Normal, $"Cerbios Tool - {m_version} (Team Resurgent)"), new GraphicsDeviceOptions(true, null, true, ResourceBindingModel.Improved, true, true), VeldridStartup.GetPlatformDefaultBackend(), out m_window, out m_graphicsDevice);
           
            m_window.Resizable = false;

            m_controller = new ImGuiController(m_graphicsDevice, m_graphicsDevice.MainSwapchain.Framebuffer.OutputDescription, m_window.Width, m_window.Height);

            if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 22000, 0))
            {
                int value = -1;
                uint DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
                DwmSetWindowAttribute(m_window.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref value, sizeof(int));
            }

            SetXboxTheme();

            m_settings = Settings.LoadSettings();

            m_biosFileOpenPicker = new PathPicker
            {
                Title = "Load Bios",
                Mode = PathPicker.PickerMode.FileOpen,
                AllowedFiles = new[] { "*.bin" },
                ButtonName = "Load"
            };

            m_biosFileSavePicker = new PathPicker
            {
                Title = "Save Bios",
                Mode = PathPicker.PickerMode.FileSave,
                AllowedFiles = new[] { "*.bin" },
                SaveName = "Bios.bin",
                ButtonName = "Save"
            };

            m_configFileOpenPicker = new PathPicker
            {
                Title = "Load Config",
                Mode = PathPicker.PickerMode.FileOpen,
                AllowedFiles = new[] { "*.config" },
                ButtonName = "Load"
            };

            m_configFileSavePicker = new PathPicker
            {
                Title = "Save Config",
                Mode = PathPicker.PickerMode.FileSave,
                AllowedFiles = new[] { "*.config" },
                SaveName = "CerbiosTool.config",
                ButtonName = "Save"
            };

            m_okDialog = new();

            m_window.Resized += () =>
            {
                m_graphicsDevice.MainSwapchain.Resize((uint)m_window.Width, (uint)m_window.Height);
                m_controller.WindowResized(m_window.Width, m_window.Height);
            };

            m_commandList = m_graphicsDevice.ResourceFactory.CreateCommandList();

            while (m_window.Exists)
            {
                InputSnapshot snapshot = m_window.PumpEvents();
                if (!m_window.Exists)
                {
                    break;
                }
                m_controller.Update(1f / 60f, snapshot);

                RenderUI();

                m_commandList.Begin();
                m_commandList.SetFramebuffer(m_graphicsDevice.MainSwapchain.Framebuffer);
                m_commandList.ClearColorTarget(0, new RgbaFloat(0.0f, 0.0f, 0.0f, 1f));
                m_controller.Render(m_graphicsDevice, m_commandList);
                m_commandList.End();
                m_graphicsDevice.SubmitCommands(m_commandList);
                m_graphicsDevice.SwapBuffers(m_graphicsDevice.MainSwapchain);
            }

            m_graphicsDevice.WaitForIdle();
            m_controller.Dispose();
            m_commandList.Dispose();
            m_graphicsDevice.Dispose();
        }

        private void RenderUI()
        {
            if (m_window == null ||
                m_controller == null ||
                m_biosFileOpenPicker == null ||
                m_biosFileSavePicker == null ||
                m_configFileOpenPicker == null ||
                m_configFileSavePicker == null ||
                m_okDialog == null)
            {
                return;
            }

            if (m_biosFileOpenPicker.Render() && !m_biosFileOpenPicker.Cancelled)
            {
                m_settings.BiosPath = m_biosFileOpenPicker.SelectedFolder;
                m_settings.BiosFile = m_biosFileOpenPicker.SelectedFile;
                m_biosLoaded = BiosUtility.LoadBiosComfig(Path.Combine(m_biosFileOpenPicker.SelectedFolder, m_biosFileOpenPicker.SelectedFile), ref m_config, ref m_biosData);
                Settings.SaveSattings(m_settings);
            }

            if (m_biosFileSavePicker.Render() && !m_biosFileSavePicker.Cancelled && string.IsNullOrEmpty(m_settings.BiosPath) == false)
            {
                var savePath = Path.Combine(m_biosFileSavePicker.SelectedFolder, m_biosFileSavePicker.SaveName);
                BiosUtility.SaveBiosConfig(m_config, Path.Combine(m_settings.BiosPath, m_settings.BiosFile), savePath, m_biosData);
            }

            if (m_configFileOpenPicker.Render() && !m_configFileOpenPicker.Cancelled)
            {
                m_settings.ConfigPath = m_configFileOpenPicker.SelectedFolder;
                m_config = Config.LoadConfiguration(Path.Combine(m_configFileOpenPicker.SelectedFolder, m_configFileOpenPicker.SelectedFile));
                Settings.SaveSattings(m_settings);
            }

            if (m_configFileSavePicker.Render() && !m_configFileSavePicker.Cancelled)
            {
                m_settings.ConfigPath = m_configFileOpenPicker.SelectedFolder;
                var savePath = Path.Combine(m_configFileSavePicker.SelectedFolder, m_configFileSavePicker.SaveName);
                Config.SaveConfiguration(savePath, m_config);
                Settings.SaveSattings(m_settings);
            }

            m_okDialog.Render();

            ImGui.Begin("Main", ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize);
            ImGui.SetWindowSize(new Vector2(m_window.Width, m_window.Height));
            ImGui.SetWindowPos(new Vector2(0, 0), ImGuiCond.Always);

            ImGui.Spacing();

            ImGui.SetCursorPosY(10);
            ImGui.BeginChild(1, new Vector2(230 + 50, 480), true, ImGuiWindowFlags.AlwaysUseWindowPadding);

            if (!m_biosLoaded)
            {
                ImGui.BeginDisabled();
            }

            var fontAtlas = ImGui.GetIO().Fonts;
            var largeFont = fontAtlas.Fonts[1];

            ImGui.PushStyleColor(ImGuiCol.Text, ImGui.ColorConvertFloat4ToU32(new Vector4(1.0f, 0.25f, 0.5f, 1.0f)));
            ImGui.PushFont(largeFont);
            ImGui.Text("NOTE: If loading from config, the\nsettings below where applicable will be\noverridden by the cerbios.ini file.");
            ImGui.PopFont();
            ImGui.PopStyleColor();


            ImGui.Spacing();

            var loadConfig = m_config.LoadConfig == 1;
            ImGui.Text("Load Config From Harddrive:");
            Toggle("##loadConfig", ref loadConfig, new Vector2(38, 20));
            m_config.LoadConfig = (byte)(loadConfig ? 1 : 0);

            string[] driveSetupModes = new string[] { "HDD & DVD", "HDD & No DVD (Legacy Mode)", "HDD & No DVD (Modern Mode)", "Dual HDD" };
            var driveSetup = (int)m_config.DriveSetup;
            ImGui.Text("Drive Setup:");
            ImGui.PushItemWidth(250);
            ImGui.Combo("##driveSetup", ref driveSetup, driveSetupModes, driveSetupModes.Length);
            ImGui.PopItemWidth();
            m_config.DriveSetup = (byte)driveSetup;

            var avCheck = m_config.AVCheck == 1;
            ImGui.Text("AV Check:");
            Toggle("##avCheck", ref avCheck, new Vector2(38, 20));
            m_config.AVCheck = (byte)(avCheck ? 1 : 0);

            var debug = m_config.Debug == 1;
            ImGui.Text("Debug:");
            Toggle("##debug", ref debug, new Vector2(38, 20));
            m_config.Debug = (byte)(debug ? 1 : 0);

            var cdPath1 = m_config.CdPath1;
            ImGui.Text("Cd Path 1:");
            ImGui.PushItemWidth(250);
            if (ImGui.InputText("##cdPath1", ref cdPath1, 99))
            {
                m_config.CdPath1 = cdPath1;
            }
            ImGui.PopItemWidth();

            var cdPath2 = m_config.CdPath2;
            ImGui.Text("Cd Path 2:");
            ImGui.PushItemWidth(250);
            if (ImGui.InputText("##cdPath2", ref cdPath2, 99))
            {
                m_config.CdPath2 = cdPath2;
            }
            ImGui.PopItemWidth();

            var cdPath3 = m_config.CdPath3;
            ImGui.Text("Cd Path 3:");
            ImGui.PushItemWidth(250);
            if (ImGui.InputText("##cdPath3", ref cdPath3, 99))
            {
                m_config.CdPath3 = cdPath3;
            }
            ImGui.PopItemWidth();

            var dashPath1 = m_config.DashPath1;
            ImGui.Text("Dash Path 1:");
            ImGui.PushItemWidth(250);
            if (ImGui.InputText("##dashPath1", ref dashPath1, 99))
            {
                m_config.DashPath1 = dashPath1;
            }
            ImGui.PopItemWidth();

            var dashPath2 = m_config.DashPath2;
            ImGui.Text("Dash Path 2:");
            ImGui.PushItemWidth(250);
            if (ImGui.InputText("##dashPath2", ref dashPath2, 99))
            {
                m_config.DashPath2 = dashPath2;
            }
            ImGui.PopItemWidth();

            var dashPath3 = m_config.DashPath3;
            ImGui.Text("Dash Path 3:");
            ImGui.PushItemWidth(250);
            if (ImGui.InputText("##dashPath3", ref dashPath3, 99))
            {
                m_config.DashPath3 = dashPath3;
            }
            ImGui.PopItemWidth();

            var bootAnimPath = m_config.BootAnimPath;
            ImGui.Text("Boot Anim Path:");
            ImGui.PushItemWidth(250);
            if (ImGui.InputText("##bootAnimPath", ref bootAnimPath, 99))
            {
                m_config.BootAnimPath = bootAnimPath;
            }
            ImGui.PopItemWidth();

            var frontLed = m_config.FrontLed;
            ImGui.Text("Front LED:");
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                ImGui.TextUnformatted("G = Green, R = Red, A = Amber, O = Off");
                ImGui.PopTextWrapPos();
                ImGui.EndTooltip();
            }
            ImGui.PushItemWidth(250);
            if (ImGui.InputText("##frontLed", ref frontLed, 4))
            {
                var result = string.Empty;
                frontLed = frontLed.ToUpper();
                for (var i =0; i < frontLed.Length; i++)
                {
                    if (!frontLed[i].Equals('G') && !frontLed[i].Equals('R') && !frontLed[i].Equals('A') && !frontLed[i].Equals('O')) {
                        continue;
                    }
                    result += frontLed[i];
                    result.PadRight(4, 'O');
                }
                m_config.FrontLed = result;
            }
            ImGui.PopItemWidth();

            string[] fanSpeeds = new string[] { "Auto", "10", "20", "30", "40", "50", "60", "70", "80", "90", "100" };
            var fanSpeed = m_config.FanSpeed / 10;
            ImGui.Text("Fan Speed:");
            ImGui.PushItemWidth(250);
            ImGui.Combo("##fanSpeed", ref fanSpeed, fanSpeeds, fanSpeeds.Length);
            ImGui.PopItemWidth();
            m_config.FanSpeed = (byte)(fanSpeed * 10);

            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();

            ImGui.PushStyleColor(ImGuiCol.Text, ImGui.ColorConvertFloat4ToU32(new Vector4(1.0f, 0.25f, 0.5f, 1.0f)));
            ImGui.PushFont(largeFont);
            ImGui.Text("NOTE: The following settings are not\napplicable in the cerbios.ini as used\nbefore loading stage.");
            ImGui.PopFont();
            ImGui.PopStyleColor();

            ImGui.Spacing();

            string[] igrMasterPorts = new string[] { "All", "1", "2", "3", "4" };
            var igrMasterPort = (int)m_config.IGRMasterPort;
            ImGui.Text("IGR Master Port:");
            ImGui.PushItemWidth(250);
            ImGui.Combo("##igrMasterPort", ref igrMasterPort, igrMasterPorts, igrMasterPorts.Length);
            ImGui.PopItemWidth();
            m_config.IGRMasterPort = (byte)igrMasterPort;

            var igrDash = m_config.IGRDash;
            ImGui.Text("IGR Dash:");
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                ImGui.TextUnformatted("A = 0, B = 1, X = 2, Y = 3, BLACK = 4, WHITE = 5, L-TRIGGER = 6, R-TRIGGER = 7\nUP = 8, DOWN = 9, LEFT = A, RIGHT = B, START = C, BACK = D, L-THUMB = E, R-THUMB = F");
                ImGui.PopTextWrapPos();
                ImGui.EndTooltip();
            }
            ImGui.PushItemWidth(250);
            if (ImGui.InputText("##igrDash", ref igrDash, 4, ImGuiInputTextFlags.CharsHexadecimal))
            {
                m_config.IGRDash = igrDash;
            }
            ImGui.PopItemWidth();

            var igrGame = m_config.IGRGame;
            ImGui.Text("IGR Game:");
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                ImGui.TextUnformatted("A = 0, B = 1, X = 2, Y = 3, BLACK = 4, WHITE = 5, L-TRIGGER = 6, R-TRIGGER = 7\nUP = 8, DOWN = 9, LEFT = A, RIGHT = B, START = C, BACK = D, L-THUMB = E, R-THUMB = F");
                ImGui.PopTextWrapPos();
                ImGui.EndTooltip();
            }
            ImGui.PushItemWidth(250);
            if (ImGui.InputText("##igrGame", ref igrGame, 4, ImGuiInputTextFlags.CharsHexadecimal))
            {
                m_config.IGRGame = igrGame;
            }
            ImGui.PopItemWidth();

            var igrFull = m_config.IGRFull;
            ImGui.Text("IGR Full:");
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                ImGui.TextUnformatted("A = 0, B = 1, X = 2, Y = 3, BLACK = 4, WHITE = 5, L-TRIGGER = 6, R-TRIGGER = 7\nUP = 8, DOWN = 9, LEFT = A, RIGHT = B, START = C, BACK = D, L-THUMB = E, R-THUMB = F");
                ImGui.PopTextWrapPos();
                ImGui.EndTooltip();
            }
            ImGui.PushItemWidth(250);
            if (ImGui.InputText("##igrFull", ref igrFull, 4, ImGuiInputTextFlags.CharsHexadecimal))
            {
                m_config.IGRFull = igrFull;
            }
            ImGui.PopItemWidth();

            var igrShutdown = m_config.IGRShutdown;
            ImGui.Text("IGR Shutdown:");
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                ImGui.TextUnformatted("A = 0, B = 1, X = 2, Y = 3, BLACK = 4, WHITE = 5, L-TRIGGER = 6, R-TRIGGER = 7\nUP = 8, DOWN = 9, LEFT = A, RIGHT = B, START = C, BACK = D, L-THUMB = E, R-THUMB = F");
                ImGui.PopTextWrapPos();
                ImGui.EndTooltip();
            }
            ImGui.PushItemWidth(250);
            if (ImGui.InputText("##igrFull", ref igrShutdown, 4, ImGuiInputTextFlags.CharsHexadecimal))
            {
                m_config.IGRShutdown = igrShutdown;
            }
            ImGui.PopItemWidth();

            string[] udmaModes = new string[] { "Auto (Startek)", "Auto (Other)", "UDMA 2 (Default)", "UDMA 3", "UDMA 4", "UDMA 5", "UDMA 6" };
            var udmaMode = (int)m_config.UDMAMode;
            ImGui.Text("UDMA Mode:");
            ImGui.PushItemWidth(250);
            if (ImGui.Combo("##udmaMode", ref udmaMode, udmaModes, udmaModes.Length))
            {
                if (udmaMode < 2)
                {
                    m_okDialog.Title = "WARNING";
                    m_okDialog.Message = "Auto modes assume you have a 80 conductor IDE cable\ninstalled. Using without can cause XBOX not to boot.\nSafe Mode will allow you to boot with UDMA 2 and perform\na reflash.";
                    m_okDialog.ShowModal();
                }
            }
            ImGui.PopItemWidth();
            m_config.UDMAMode = (byte)(udmaMode);

            string[] themes = new string[] { "Current", "Red", "Green", "Blue", "Touch of IND", "Red Eyes, White" };
            int theme = 0;
            ImGui.Text("Theme:");
            ImGui.PushItemWidth(250);
            ImGui.Combo("##theme", ref theme, themes, themes.Length);
            ImGui.PopItemWidth();
            if (theme > 0)
            {
                if (theme == 1)
                {
                    m_config.SetRedTheme();
                }
                else if (theme == 2)
                {
                    m_config.SetGreenTheme();
                }
                else if (theme == 3)
                {
                    m_config.SetBlueTheme();
                }
                else if (theme == 4)
                {
                    m_config.SetTouchOfIndTheme();
                }
                else if (theme == 5)
                {
                    m_config.SetRedEyesWhite();
                }
            }

            var splashBackground = Config.RGBToVector3(m_config.SplashBackground);
            ImGui.Text("Splash Background:");
            ImGui.PushItemWidth(250);
            ImGui.ColorEdit3("##splashBackground", ref splashBackground, ImGuiColorEditFlags.DisplayHex);
            ImGui.PopItemWidth();
            m_config.SplashBackground = Config.Vector3ToRGB(splashBackground);

            var splashCerbiosText = Config.RGBToVector3(m_config.SplashCerbiosText);
            ImGui.Text("Splash Cerbios Text:");
            ImGui.PushItemWidth(250);
            ImGui.ColorEdit3("##splashCerbiosText", ref splashCerbiosText, ImGuiColorEditFlags.DisplayHex);
            ImGui.PopItemWidth();
            m_config.SplashCerbiosText = Config.Vector3ToRGB(splashCerbiosText);

            var splashSafeModeText = Config.RGBToVector3(m_config.SplashSafeModeText);
            ImGui.Text("Splash SafeMode Text:");
            ImGui.PushItemWidth(250);
            ImGui.ColorEdit3("##splashSafeModeText", ref splashSafeModeText, ImGuiColorEditFlags.DisplayHex);
            ImGui.PopItemWidth();
            m_config.SplashSafeModeText = Config.Vector3ToRGB(splashSafeModeText);

            var splashLogo1 = Config.RGBToVector3(m_config.SplashLogo1);
            ImGui.Text("Splash Logo1:");
            ImGui.PushItemWidth(250);
            ImGui.ColorEdit3("##splashLogo1", ref splashLogo1, ImGuiColorEditFlags.DisplayHex);
            ImGui.PopItemWidth();
            m_config.SplashLogo1 = Config.Vector3ToRGB(splashLogo1);

            var splashLogo2 = Config.RGBToVector3(m_config.SplashLogo2);
            ImGui.Text("Splash Logo2:");
            ImGui.PushItemWidth(250);
            ImGui.ColorEdit3("##splashLogo2", ref splashLogo2, ImGuiColorEditFlags.DisplayHex);
            ImGui.PopItemWidth();
            m_config.SplashLogo2 = Config.Vector3ToRGB(splashLogo2);

            var splashLogo3 = Config.RGBToVector3(m_config.SplashLogo3);
            ImGui.Text("Splash Logo3:");
            ImGui.PushItemWidth(250);
            ImGui.ColorEdit3("##splashLogo3", ref splashLogo3, ImGuiColorEditFlags.DisplayHex);
            ImGui.PopItemWidth();
            m_config.SplashLogo3 = Config.Vector3ToRGB(splashLogo3);

            var splashLogo4 = Config.RGBToVector3(m_config.SplashLogo4);
            ImGui.Text("Splash Logo4:");
            ImGui.PushItemWidth(250);
            ImGui.ColorEdit3("##splashLogo4", ref splashLogo4, ImGuiColorEditFlags.DisplayHex);
            ImGui.PopItemWidth();
            m_config.SplashLogo4 = Config.Vector3ToRGB(splashLogo4);

            string[] splashScales = new string[] { "Hide", "Large", "Medium", "Small" };
            var splashScale = (int)m_config.SplashScale;
            ImGui.Text("Splash Scale:");
            ImGui.PushItemWidth(250);
            ImGui.Combo("##splashScale", ref splashScale, splashScales, splashScales.Length);
            ImGui.PopItemWidth();
            m_config.SplashScale = (byte)splashScale;

            if (!m_biosLoaded)
            {
                ImGui.EndDisabled();
            }

            ImGui.EndChild();

            var startPos = new Vector2(250 + 48, 10);
            var size = new Vector2(640, 480);
            var drawList = ImGui.GetWindowDrawList();
            drawList.AddRectFilled(startPos, startPos + size, ImGui.ColorConvertFloat4ToU32(Config.RGBToVector4(m_config.SplashBackground)));
            drawList.AddRect(startPos, startPos + size, ImGui.ColorConvertFloat4ToU32(new Vector4(0.5f, 0.5f, 0.5f, 0.5f)));

            if (m_config.SplashScale > 0)
            {
                var scaledSize = size / m_config.SplashScale;
                var scaleOffset = (size - scaledSize) / 2;
                ImGui.SetCursorPos(startPos + scaleOffset);
                ImGui.Image(m_controller.Logo1Texture, scaledSize, Vector2.Zero, Vector2.One, Config.RGBToVector4(m_config.SplashLogo1));
                ImGui.SetCursorPos(startPos + scaleOffset);
                ImGui.Image(m_controller.Logo2Texture, scaledSize, Vector2.Zero, Vector2.One, Config.RGBToVector4(m_config.SplashLogo2));
                ImGui.SetCursorPos(startPos + scaleOffset);
                ImGui.Image(m_controller.Logo3Texture, scaledSize, Vector2.Zero, Vector2.One, Config.RGBToVector4(m_config.SplashLogo3));
                ImGui.SetCursorPos(startPos + scaleOffset);
                ImGui.Image(m_controller.Logo4Texture, scaledSize, Vector2.Zero, Vector2.One, Config.RGBToVector4(m_config.SplashLogo4));
                ImGui.SetCursorPos(startPos + scaleOffset);
                ImGui.Image(m_controller.CerbiosTextTexture, scaledSize, Vector2.Zero, Vector2.One, Config.RGBToVector4(m_config.SplashCerbiosText));
                ImGui.SetCursorPos(startPos + scaleOffset);
                ImGui.Image(m_controller.SafeModeTextTexture, scaledSize, Vector2.Zero, Vector2.One, Config.RGBToVector4(m_config.SplashSafeModeText));
            }
            else 
            {
                ImGui.SetCursorPos(startPos);
                ImGui.Image(m_controller.SafeModeTextTexture, size, Vector2.Zero, Vector2.One, Config.RGBToVector4(m_config.SplashSafeModeText));
            }

            ImGui.SetCursorPosY(m_window.Height - 40);

            if (ImGui.Button("Load Bios", new Vector2(100, 30)))
            {
                var path = Directory.Exists(m_settings.BiosPath) ? m_settings.BiosPath : Directory.GetCurrentDirectory();
                m_biosFileOpenPicker.ShowModal(path);
            }

            if (m_biosLoaded)
            {
                ImGui.SameLine();

                if (ImGui.Button("Save Bios", new Vector2(100, 30)))
                {
                    var path = Directory.Exists(m_settings.BiosPath) ? m_settings.BiosPath : Directory.GetCurrentDirectory();
                    m_biosFileSavePicker.ShowModal(path);
                }

                ImGui.SameLine();

                if (ImGui.Button("Default Config", new Vector2(100, 30)))
                {
                    m_config.SetDefaults();
                }

                ImGui.SameLine();

                if (ImGui.Button("Load Config", new Vector2(100, 30)))
                {
                    var path = Directory.Exists(m_settings.ConfigPath) ? m_settings.ConfigPath : Directory.GetCurrentDirectory();
                    m_configFileOpenPicker.ShowModal(path);
                }

                ImGui.SameLine();

                if (ImGui.Button("Save Config", new Vector2(100, 30)))
                {
                    var path = Directory.Exists(m_settings.ConfigPath) ? m_settings.ConfigPath : Directory.GetCurrentDirectory();
                    m_configFileSavePicker.ShowModal(path);
                }
            }

            ImGui.SameLine();

            ImGui.SetCursorPosX(m_window.Width - 374);

            if (ImGui.Button("Patreon", new Vector2(100, 30)))
            {
                OpenUrl("https://www.patreon.com/teamresurgent");
            }

            ImGui.SameLine();

            if (ImGui.Button("Ko-Fi", new Vector2(100, 30)))
            {
                OpenUrl("https://ko-fi.com/teamresurgent");
            }

            ImGui.SameLine();

            if (ImGui.Button("Coded by EqUiNoX", new Vector2(150, 30)))
            {
                OpenUrl("https://github.com/Team-Resurgent/CerbiosTool");
            }

            ImGui.End();
        }
    }
}
