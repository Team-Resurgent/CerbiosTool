using ImGuiNET;
using System.Numerics;

namespace CerbiosTool
{
    public class OkDialog
    {
        private bool _showModal;
        private bool _open;

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public void ShowModal()
        {
            _showModal = true;
        }

        private void CloseModal()
        {
            _open = false;
            ImGui.CloseCurrentPopup();
        }

        public bool Render()
        {
            if (_showModal)
            {
                _showModal = false;
                _open = true;
                ImGui.OpenPopup(Title);
            }

            if (!_open)
            {
                return false;
            }

            var open = true;
            if (!ImGui.BeginPopupModal(Title, ref open, ImGuiWindowFlags.NoResize))
            {
                return false;
            }

            var result = false;

            if (ImGui.IsWindowAppearing())
            {
                ImGui.SetWindowSize(new Vector2(400, 160));
            }

            var fontAtlas = ImGui.GetIO().Fonts;
            var largeFont = fontAtlas.Fonts[1];

            ImGui.PushStyleColor(ImGuiCol.Text, ImGui.ColorConvertFloat4ToU32(new Vector4(1.0f, 0.25f, 0.5f, 1.0f)));
            ImGui.PushFont(largeFont);
            ImGui.Text(Message);
            ImGui.PopFont();
            ImGui.PopStyleColor();

            ImGui.Spacing();
            if (ImGui.Button("Ok", new Vector2(100, 30)))
            {
                result = true;
                CloseModal();
            }
            ImGui.EndPopup();

            return result;
        }
    }
}